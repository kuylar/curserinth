using System.Text.RegularExpressions;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;
using HtmlAgilityPack;

namespace CurseRinth;

public static class Utils
{
	public static Regex LinkoutRegex = new("\\(\\/linkout\\?remoteUrl=(.+?)\\)");
	private static HtmlWeb _web = new();

	// isnt 1:1 but it works
	public static ModsSearchSortField GetSortField(SearchIndex index) =>
		index switch
		{
			SearchIndex.RELEVANCE => ModsSearchSortField.Featured,
			SearchIndex.DOWNLOADS => ModsSearchSortField.TotalDownloads,
			SearchIndex.FOLLOWS => ModsSearchSortField.Popularity,
			SearchIndex.NEWEST => ModsSearchSortField.LastUpdated,
			SearchIndex.UPDATED => ModsSearchSortField.LastUpdated,
			_ => ModsSearchSortField.Featured
		};

	public static string GetCategoryName(Category category) => GetCategoryName(category.Name);
	public static string GetCategoryName(string name) => name.Replace(' ', '-').ToLower();

	public static ModrinthLicense GetLicenseFromMod(uint projectType, string id, string slug)
	{
		return new ModrinthLicense("CUSTOM", "Unknown License", $"https://www.curseforge.com/project/{id}/license");
		// cloudflare makes this not work :(
		try
		{
			string cfpt = projectType switch
			{
				6 => "mc-mods",
				4471 => "modpacks",
			};
			HtmlDocument htmlDocument = _web.Load($"https://www.curseforge.com/minecraft/{cfpt}/{slug}");
			string title = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/main/div[1]/div[2]/section/aside/div[2]/div/div[1]/div[2]/div[5]/a").InnerText;
			return new ModrinthLicense(title.Split(" ").First().ToUpper(), title, $"https://www.curseforge.com/project/{id}/license");
		}
		catch
		{
			return new ModrinthLicense("CUSTOM", "Unknown License", $"https://www.curseforge.com/project/{id}/license");
		}
	}
}