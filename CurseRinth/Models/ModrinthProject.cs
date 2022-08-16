using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using CurseForge.APIClient;
using CurseForge.APIClient.Models.Mods;
using Html2Markdown;

namespace CurseRinth.Models;

public class ModrinthProject
{
	[JsonPropertyName("slug")]
	public string Slug { get; }

	[JsonPropertyName("title")]
	public string Title { get; }

	[JsonPropertyName("description")]
	public string Description { get; }

	[JsonPropertyName("categories")]
	public string[] Categories { get; }

	[JsonPropertyName("additional_categories")]
	public string[] AdditionalCategories { get => Categories; }

	[JsonPropertyName("client_side")]
	public string ClientSide { get; }

	[JsonPropertyName("server_side")]
	public string ServerSide { get; }

	[JsonPropertyName("body")]
	public string Body { get; }

	[JsonPropertyName("issues_url")]
	public string? IssuesUrl { get; }

	[JsonPropertyName("source_url")]
	public string? SourceUrl { get; }

	[JsonPropertyName("wiki_url")]
	public string? WikiUrl { get; }

	[JsonPropertyName("discord_url")]
	public Uri? DiscordUrl { get; }

	[JsonPropertyName("donation_urls")]
	public object[] DonationUrls { get; }

	[JsonPropertyName("project_type")]
	public string ProjectType { get; }

	[JsonPropertyName("downloads")]
	public double Downloads { get; }

	[JsonPropertyName("icon_url")]
	public string IconUrl { get; }

	[JsonPropertyName("id")]
	public string Id { get; }

	[JsonPropertyName("team")]
	public string Team { get; }

	[JsonPropertyName("body_url")]
	public object BodyUrl { get; }

	[JsonPropertyName("moderator_message")]
	public object ModeratorMessage { get; }

	[JsonPropertyName("published")]
	public string Published { get; }

	[JsonPropertyName("approved")]
	public string Approved { get; }

	[JsonPropertyName("updated")]
	public string Updated { get; }

	[JsonPropertyName("followers")]
	public uint? Followers { get; }

	[JsonPropertyName("status")]
	public string Status { get; }

	[JsonPropertyName("license")]
	public ModrinthLicense License { get; }

	[JsonPropertyName("versions")]
	public string[] Versions { get; }

	[JsonPropertyName("gallery")]
	public ModrinthImage[] Gallery { get; }

	public ModrinthProject(Mod mod, ApiClient api)
	{
		Slug = mod.Slug;
		Title = mod.Name;
		Description = mod.Summary;
		Categories = mod.Categories.Select(Utils.GetCategoryName).ToArray();
		ClientSide = "optional";
		ServerSide = "optional";
		string body = api.GetModDescriptionAsync(mod.Id).Result.Data;
		try
		{
			Body = new Converter().Convert(body).Replace("\r\n", "\n");
			Body = Utils.LinkoutRegex.Replace(Body, match => $"({HttpUtility.UrlDecode(HttpUtility.UrlDecode(match.Groups[1].ToString()))})");
		}
		catch
		{
			Body = body;
		}
		IssuesUrl = string.IsNullOrWhiteSpace(mod.Links.IssuesUrl) ? null : mod.Links.IssuesUrl;
		SourceUrl = string.IsNullOrWhiteSpace(mod.Links.SourceUrl) ? null : mod.Links.SourceUrl;
		WikiUrl = string.IsNullOrWhiteSpace(mod.Links.WikiUrl) ? null : mod.Links.WikiUrl;
		DiscordUrl = null;
		DonationUrls = Array.Empty<object>();
		ProjectType = mod.ClassId switch
		{
			6 => "mod",
			4471 => "modpack",
			var _ => ""
		};
		Downloads = mod.DownloadCount;
		IconUrl = mod.Logo.Url;
		Id = mod.Id.ToString();
		Team = mod.Authors.First().Id.ToString();
		Published = mod.DateCreated.ToString("O");
		Updated = mod.DateModified.ToString("O");
		Approved = mod.DateCreated.ToString("O");
		Followers = mod.ThumbsUpCount;
		Status = "approved";
		License = new ModrinthLicense();
		Versions = mod.LatestFiles.Select(x => x.Id.ToString()).ToArray();
		Gallery = mod.Screenshots.Select(x => new ModrinthImage(x)).ToArray();
	}
}