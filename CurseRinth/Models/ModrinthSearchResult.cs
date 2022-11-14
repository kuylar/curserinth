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
	public uint? Followers { get; }

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
			6 => "mod",
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
}