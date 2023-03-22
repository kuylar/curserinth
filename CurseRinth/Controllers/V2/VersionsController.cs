using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;
using Microsoft.AspNetCore.Mvc;
using File = CurseForge.APIClient.Models.Files.File;

namespace CurseRinth.Controllers.V2;

[Route("/v2")]
[ApiController]
public class VersionsController : Controller
{
	private ApiClient Api;
	private SlugMapper Slug;

	public VersionsController(ApiClient api, SlugMapper slug)
	{
		Api = api;
		Slug = slug;
	}

	[HttpGet]
	[Route("project/{slug}/version")]
	public async Task<IEnumerable<ModrinthVersion>> GetProjectVersions(string slug, bool featured = false)
	{
		if (!Slug.TryGetId(slug, out int cfId))
		{
			Response.StatusCode = 404;
			return null!;
		}

		GenericResponse<Mod>? projectResponse = await Api.GetModAsync(cfId);
		Mod project = projectResponse.Data;
		GenericListResponse<File> files = await Api.GetModFilesAsync(cfId, pageSize: 20);
		return featured
			? files.Data
				.OrderBy(x => x.ReleaseType)
				.DistinctBy(x => x.GameVersions.First(y =>
					!y.Contains("forge", StringComparison.OrdinalIgnoreCase) &&
					!y.Contains("fabric", StringComparison.OrdinalIgnoreCase) &&
					!y.Contains("quilt", StringComparison.OrdinalIgnoreCase)))
				.Take(3)
				.Select(x => new ModrinthVersion(project, x, Api))
			: files.Data
				.Select(x => new ModrinthVersion(project, x, Api));
	}
}