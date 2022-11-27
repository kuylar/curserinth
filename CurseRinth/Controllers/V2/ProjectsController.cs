using System.Text;
using System.Text.Json;
using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Files;
using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CurseRinth.Controllers.V2;

[Route("/v2")]
[ApiController]
public class ProjectsController : Controller
{
	private ApiClient ApiClient;
	private SlugMapper Slug;

	public ProjectsController(SlugMapper slug, ApiClient apiClient)
	{
		Slug = slug;
		ApiClient = apiClient;
	}

	[HttpGet]
	[Route("search")]
	public async Task<ModrinthPagination<ModrinthSearchResult>> Search(string? query = "", ModrinthFacets? facets = null,
		SearchIndex index = SearchIndex.RELEVANCE, uint offset = 0, uint limit = 20, string? filters = null)
	{
		facets ??= new ModrinthFacets();
		StringBuilder comment = new();
		if (!string.IsNullOrWhiteSpace(filters))
			comment.AppendLine("WARN: Filters are not supported, ignoring them");
		if (facets.HasMultipleOfAnything())
			comment.AppendLine(
				"WARN: CurseForge API does not support AND/OR filters in their search, using only the first facets");

		GenericListResponse<Mod> mods = await ApiClient.SearchModsAsync(432, facets.GetProjectType(), (uint?)facets.GetCategory(), facets.GetGameVersion(),
			query, Utils.GetSortField(index), ModsSearchSortOrder.Descending, facets.GetModLoader(), index: offset, pageSize: Math.Clamp(limit, 0, 50));

		return new ModrinthPagination<ModrinthSearchResult>(
			mods.Data.Select(x =>
			{
				Slug.SaveId(x);
				return new ModrinthSearchResult(x);
			}).ToList(),
			(int)mods.Pagination.Index,
			(int)mods.Pagination.PageSize,
			(int)mods.Pagination.TotalCount,
			comment.Length > 0 ? comment.ToString() : null
		);
	}

	[HttpGet]
	[Route("project/{slug}")]
	public async Task<ModrinthProject> GetProject(string slug)
	{
		if (!Slug.TryGetId(slug, out uint cfId))
		{
			Response.StatusCode = 404;
			return null!;
		}

		GenericResponse<Mod> projectResponse = await ApiClient.GetModAsync(cfId);
		Mod project = projectResponse.Data;
		Slug.SaveId(project);
		return new ModrinthProject(project, ApiClient);
	}

	[HttpGet]
	[Route("projects")]
	public async Task<List<ModrinthProject>> GetProjects(string slugs)
	{
		List<ModrinthProject> projects = new();
		string[] realIds = JsonSerializer.Deserialize<string[]>(slugs)!;
		List<uint> ids = new();
		foreach (string id in realIds)
		{
			if (!Slug.TryGetId(id, out uint cfId))
				continue;
			ids.Add(cfId);
		}

		GenericListResponse<Mod> mods = await ApiClient.GetModsByIdListAsync(new GetModsByIdsListRequestBody
		{
			ModIds = ids
		});

		foreach (Mod mod in mods.Data)
		{
			Slug.SaveId(mod);
			projects.Add(new ModrinthProject(mod, ApiClient));
		}

		return projects;
	}

	[HttpGet]
	[Route("project/{slug}/check")]
	public async Task<Dictionary<string, string>> CheckSlugValidity(string slug)
	{
		if (!Slug.TryGetId(slug, out uint cfId))
		{
			Response.StatusCode = 404;
			return null!;
		}

		GenericResponse<Mod> projectResponse = await ApiClient.GetModAsync(cfId);
		Mod project = projectResponse.Data;
		Slug.SaveId(project);
		return new Dictionary<string, string>
		{
			["id"] = project.Id.ToString()
		};
	}

	[HttpGet]
	[Route("project/{slug}/dependencies")]
	public async Task<ModrinthDependencies> GetProjectDependencies(string slug)
	{
		if (!Slug.TryGetId(slug, out uint cfId))
		{
			Response.StatusCode = 404;
			return null!;
		}

		//fixme: this endpoint sometimes throws a 500, find the reason and fix it
		try
		{
			GenericResponse<Mod> projectResponse = await ApiClient.GetModAsync(cfId);
			Mod project = projectResponse.Data;
			Slug.SaveId(project);
			IEnumerable<FileDependency> dependencies = project.LatestFiles.Last().Dependencies
				.Where(x => x.RelationType == FileRelationType.RequiredDependency).ToArray();

			GenericListResponse<Mod> projectsResponse = await ApiClient.GetModsByIdListAsync(
				new GetModsByIdsListRequestBody
				{
					ModIds = dependencies.Select(x => x.ModId).ToList()
				});

			List<ModrinthProject> projects =
				projectsResponse.Data.Select(x => new ModrinthProject(x, ApiClient)).ToList();
			List<ModrinthVersion> versions =
				(from d in dependencies
					select projectsResponse.Data.FirstOrDefault(x => x.Id == d.ModId)
					into dependentMod
					select new ModrinthVersion(dependentMod, dependentMod.LatestFiles.Last(), ApiClient)).ToList();

			return new ModrinthDependencies(projects, versions);
		}
		catch (Exception e)
		{
			Log.Error(e, "Failed to load dependencies for project {0}", cfId);
			return new ModrinthDependencies(new List<ModrinthProject>(), new List<ModrinthVersion>());
		}
	}
}