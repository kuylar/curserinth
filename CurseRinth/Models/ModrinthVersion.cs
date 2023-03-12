using System.Text.Json.Serialization;
using System.Web;
using CurseForge.APIClient;
using CurseForge.APIClient.Models.Mods;
using Html2Markdown;
using File = CurseForge.APIClient.Models.Files.File;

namespace CurseRinth.Models;

public class ModrinthVersion
{
	[JsonPropertyName("name")]
	public string Name { get; }

	[JsonPropertyName("version_number")]
	public string VersionNumber { get; }

	[JsonPropertyName("changelog")]
	public string Changelog { get; }

	[JsonPropertyName("dependencies")]
	public ModrinthDependency[] Dependencies { get; }

	[JsonPropertyName("game_versions")]
	public List<string> GameVersions { get; }

	[JsonPropertyName("version_type")]
	public string VersionType { get; }

	[JsonPropertyName("loaders")]
	public string[] Loaders { get; }

	[JsonPropertyName("featured")]
	public bool Featured { get; }

	[JsonPropertyName("id")]
	public string Id { get; }

	[JsonPropertyName("project_id")]
	public string ProjectId { get; }

	[JsonPropertyName("author_id")]
	public string AuthorId { get; }

	[JsonPropertyName("date_published")]
	public string DatePublished { get; }

	[JsonPropertyName("downloads")]
	public ulong Downloads { get; }

	[JsonPropertyName("changelog_url")]
	public string? ChangelogUrl { get; set; }

	[JsonPropertyName("files")]
	public ModrinthFile[] Files { get; }

	public ModrinthVersion(string name, string versionNumber, string changelog, ModrinthDependency[] dependencies, List<string> gameVersions, string versionType, string[] loaders, string id, string projectId, string authorId, string datePublished, ModrinthFile[] files)
	{
		Name = name;
		VersionNumber = versionNumber;
		Changelog = changelog;
		Dependencies = dependencies;
		GameVersions = gameVersions;
		VersionType = versionType;
		Loaders = loaders;
		Id = id;
		ProjectId = projectId;
		AuthorId = authorId;
		DatePublished = datePublished;
		Files = files;
	}

	public ModrinthVersion(Mod project, File file, ApiClient api)
	{
		Name = file.FileName;
		VersionNumber = file.Id.ToString();
		string changelog = "";//api.GetModFileChangelogAsync(project.Id, file.Id).Result.Data;
		try
		{
			Changelog = new Converter().Convert(changelog).Replace("\r\n", "\n");
			Changelog = Utils.LinkoutRegex.Replace(Changelog,
				match => $"({HttpUtility.UrlDecode(HttpUtility.UrlDecode(match.Groups[1].ToString()))})");
		}
		catch
		{
			Changelog = changelog;
		}
		Dependencies = file.Dependencies.Select(x => new ModrinthDependency(x)).ToArray();
		GameVersions = file.GameVersions;
		VersionType = file.ReleaseType.ToString().ToLower();
		
		string? loader = project.LatestFilesIndexes.FirstOrDefault(x => x.FileId == file.Id)?.ModLoader.ToString()
			?.ToLower();
		if (loader == null)
			Loaders = Array.Empty<string>();
		else
			Loaders = new[]
			{
				 loader
			};
		
		Featured = (project.LatestFilesIndexes.LastOrDefault()?.FileId ?? 0) == file.Id;
		Id = file.Id.ToString();
		ProjectId = project.Id.ToString();
		AuthorId = project.Authors.FirstOrDefault()?.Id.ToString() ?? "";
		DatePublished = file.FileDate.ToString("O");
		Downloads = file.DownloadCount;
		Files = new ModrinthFile[]
		{
			new()
			{
				Hashes = file.Hashes.ToDictionary(x => x.Algo.ToString().ToLower(), x => x.Value),
				Url = file.DownloadUrl,
				Filename = file.FileName,
				Primary = true,
				Size = file.FileLength
			}
		};
	}
}