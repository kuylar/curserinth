using System.ComponentModel;
using System.Text.Json.Serialization;
using CurseForge.APIClient.Models.Mods;

namespace CurseRinth.Models;

public class ModrinthSearchResult
{
	[JsonPropertyName("slug")]
	public string Slug { get; }

	[JsonPropertyName("title")]
	public string Title { get; }

	[JsonPropertyName("description")]
	public string Description { get; }

	[JsonPropertyName("categories")]
	public string[] Categories { get; }

	[JsonPropertyName("display_categories")]
	public string[] DisplayCategories { get; }

	[JsonPropertyName("client_side")]
	public string ClientSide { get; }

	[JsonPropertyName("server_side")]
	public string ServerSide { get; }

	[JsonPropertyName("project_type")]
	public string ProjectType { get; }

	[JsonPropertyName("downloads")]
	public double Downloads { get; }

	[JsonPropertyName("icon_url")]
	public string IconUrl { get; }

	[JsonPropertyName("project_id")]
	public string Id { get; }

	[JsonPropertyName("author")]
	public string Author { get; }

	[JsonPropertyName("date_created")]
	public string DateCreated { get; }

	[JsonPropertyName("date_modified")]
	public string DateModified { get; }

	[JsonPropertyName("follows")]
	public int? Followers { get; }

	[JsonPropertyName("latest_version")]
	public string LatestVersion { get; }

	[JsonPropertyName("license")]
	public object License { get; } // todo: get the HTML page because the API doesnt provide the license

	[JsonPropertyName("gallery")]
	public ModrinthImage[] Gallery { get; }

	public ModrinthSearchResult(Mod mod)
	{
		Slug = SlugMapper.FormatSlug(mod);
		Title = mod.Name;
		Description = mod.Summary;
		Categories = mod.Categories.Select(Utils.GetCategoryName).ToArray();
		DisplayCategories = mod.Categories.Select(Utils.GetCategoryName).ToArray();
		ClientSide = "optional";
		ServerSide = "optional";
		ProjectType = mod.ClassId switch
		{
			5 => "mod",
			6 => "mod",
			12 => "resourcepack",
			4471 => "modpack",
			var _ => ""
		};
		Downloads = mod.DownloadCount;
		IconUrl = mod.Logo.Url;
		Id = mod.Id.ToString();
		Author = mod.Authors.First().Name;
		DateCreated = mod.DateReleased.ToString("O");
		DateModified = mod.DateModified.ToString("O");
		Followers = mod.ThumbsUpCount;
		License = new ModrinthLicense();
		LatestVersion = mod.LatestFiles.Last().Id.ToString();
		Gallery = mod.Screenshots.Select(x => new ModrinthImage(x)).ToArray();
	}

	public ModrinthSearchResult(BetaMod mod)
	{
		Slug = SlugMapper.FormatSlug(mod);
		Title = mod.Name;
		Description = mod.Summary;
		Categories = mod.Categories.Select(Utils.GetCategoryName).ToArray();
		DisplayCategories = mod.Categories.Select(Utils.GetCategoryName).ToArray();
		ClientSide = "optional";
		ServerSide = "optional";
		ProjectType = mod.Class.Id switch
		{
			5 => "mod",
			6 => "mod",
			12 => "resourcepack",
			4471 => "modpack",
			var _ => ""
		};
		Downloads = mod.Downloads;
		IconUrl = mod.AvatarUrl.ToString();
		Id = mod.Id.ToString();
		Author = mod.Author;
		DateCreated = DateTimeOffset.FromUnixTimeSeconds(mod.CreationDate).ToString("O");
		DateModified = DateTimeOffset.FromUnixTimeSeconds(mod.UpdateDate).ToString("O");
		Followers = 0;
		License = new ModrinthLicense("CUSTOM", mod.LicenseType, $"https://www.curseforge.com/project/{Id}/license");
		LatestVersion = null;
		Gallery = Array.Empty<ModrinthImage>();
	}
}