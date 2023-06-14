using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Files;
using CurseForge.APIClient.Models.Mods;
using Newtonsoft.Json;

namespace CurseRinth;

public class CurseForgeModpackManifest
{
	[JsonProperty("minecraft")] public Minecraft MinecraftInfo { get; set; }
	[JsonProperty("manifestType")] public string ManifestType { get; set; }
	[JsonProperty("manifestVersion")] public long ManifestVersion { get; set; }
	[JsonProperty("name")] public string Name { get; set; }
	[JsonProperty("version")] public string Version { get; set; }
	[JsonProperty("author")] public string Author { get; set; }
	[JsonProperty("files")] public File[] Files { get; set; }
	[JsonProperty("overrides")] public string Overrides { get; set; }

	public class File
	{
		[JsonProperty("projectID")] public long ProjectId { get; set; }
		[JsonProperty("fileID")] public long FileId { get; set; }
		[JsonProperty("required")] public bool FileRequired { get; set; }
	}

	public class Minecraft
	{
		[JsonProperty("version")] public string Version { get; set; }
		[JsonProperty("modLoaders")] public ModLoader[] ModLoaders { get; set; }
	}

	public class ModLoader
	{
		[JsonProperty("id")] public string Id { get; set; }
		[JsonProperty("primary")] public bool Primary { get; set; }
	}

	public async Task<ModrinthModpackIndex> ToModrinthIndexJson(ApiClient api)
	{
		ModrinthModpackIndex index = new();
		index.FormatVersion = 1;
		index.Game = "minecraft";
		index.Name = Name;
		index.Summary =
			"Converted from CurseForge using CurseRinth @ curserinth.kuylar.dev (don't tell anyone 🤫)";
		index.VersionId = Version;
		index.Dependencies = new Dictionary<string, string>();
		index.Dependencies.Add("minecraft", MinecraftInfo.Version);
		foreach (ModLoader ml in MinecraftInfo.ModLoaders)
		{
			switch (ml.Id.Split("-")[0])
			{
				case "fabric":
					index.Dependencies.Add("fabric-loader", ml.Id[(ml.Id.IndexOf('-') + 1)..]);
					break;
				default:
					index.Dependencies.Add(ml.Id.Split("-")[0], ml.Id[(ml.Id.IndexOf('-') + 1)..]);
					break;
			}
		}

		GenericListResponse<CurseForge.APIClient.Models.Files.File> files = await api.GetFilesAsync(
			new GetModFilesRequestBody
			{
				FileIds = Files.Select(x => (uint)x.FileId).ToList()
			});
		GenericListResponse<Mod> projectsResponse = await api.GetModsByIdListAsync(
			new GetModsByIdsListRequestBody
			{
				ModIds = Files.Select(x => (int)x.ProjectId).ToList()
			});
		Dictionary<int, Mod> projects = projectsResponse.Data.ToDictionary(x => x.Id, x => x);

		index.Files = files.Data.Select(x =>
		{
			string parentPath = projects[(int)x.ModId].ClassId switch
			{
				6 => "mods",
				12 => "resourcepacks",
				17 => "saves",
				4471 => "modpacks",
				var _ => "LOST_FOUND"
			};
			return new ModrinthModpackIndex.File
			{
				Path = $"{parentPath}/{x.FileName}",
				Hashes = x.Hashes.ToDictionary(h => h.Algo.ToString().ToLower(), h => h.Value),
				Downloads = new[]
				{
					x.DownloadUrl
				},
				FileSize = x.FileLength
			};
		}).ToArray();

		return index;
	}
}