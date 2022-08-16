using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Minecraft;
using CurseRinth.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurseRinth.Controllers.V2;

[Route("/v2/tag/")]
[ApiController]
public class TagsController : Controller
{
	private ApiClient ApiClient;
	private IEnumerable<ModrinthLoader>? ModLoaderCache;
	private IEnumerable<ModrinthGameVersion>? VersionCache;
	
	public TagsController(ApiClient apiClient)
	{
		ApiClient = apiClient;
	}

	[HttpGet]
	[Route("category")]
	public IEnumerable<ModrinthCategory> GetCategories()
	{
		List<ModrinthCategory> categories = new();
		categories.AddRange(CategoryMapping.ModCategories.Select(x => new ModrinthCategory(x, "mod")));
		categories.AddRange(CategoryMapping.ModpackCategories.Select(x => new ModrinthCategory(x, "modpack")));
		categories.AddRange(CategoryMapping.ResourcePackCategories.Select(x => new ModrinthCategory(x, "resourcepack")));
		return categories;
	}

	[HttpGet]
	[Route("loader")]
	public async Task<IEnumerable<ModrinthLoader>> GetLoaders()
	{
		if (ModLoaderCache is null)
		{
			GenericListResponse<MinecraftModloaderInfoListItem>? loaders = await ApiClient.GetMinecraftModloaders(includeAll: true);
			ModLoaderCache = loaders.Data.DistinctBy(x => x.Type).Select(x => new ModrinthLoader(x));
		}
		return ModLoaderCache;
	}

	[HttpGet]
	[Route("game_version")]
	public async Task<IEnumerable<ModrinthGameVersion>> GetVersions()
	{
		if (VersionCache is null)
		{
			GenericListResponse<MinecraftVersionInfo> loaders = await ApiClient.GetMinecraftVersions();
			VersionCache = loaders.Data.Select(x => new ModrinthGameVersion(x));
		}
		return VersionCache;
	}

	[HttpGet]
	[Route("license")]
	public IEnumerable<Dictionary<string, string>> GetLicenses()
	{
		return new []
		{
			new Dictionary<string, string>
			{
				["short"] = "ihnol",
				["name"] = "I Have No Idea License"
			}
		};
	}

	[HttpGet]
	[Route("donation_platform")]
	public IEnumerable<Dictionary<string, string>> GetDonationPlatforms() => Array.Empty<Dictionary<string, string>>();

	[HttpGet]
	[Route("report_type")]
	public string[] GetReportTypes() => Array.Empty<string>();
}