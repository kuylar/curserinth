using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurseRinth.Controllers.V2;

[Route("/v2")]
[ApiController]
public class TeamsController : Controller
{
	private ApiClient Api;
	private SlugMapper Slug;

	public TeamsController(ApiClient api, SlugMapper slug)
	{
		Api = api;
		Slug = slug;
	}
	
	[HttpGet]
	[Route("project/{slug}/members")]
	public async Task<List<ModrinthTeamMember>> GetProject(string slug)
	{
		if (!Slug.TryGetId(slug, out int cfId))
		{
			Response.StatusCode = 404;
			return null!;
		}

		GenericResponse<Mod> projectResponse = await Api.GetModAsync(cfId);
		Mod project = projectResponse.Data;
		Slug.SaveId(project);
		return project.Authors.Select(author => new ModrinthTeamMember(project.Slug, "Member", new ModrinthUser(author))).ToList();
	}
}