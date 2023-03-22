using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;

namespace CurseRinth;

// TODO: move this into a SQLite file
public class SlugMapper
{
	public ApiClient ApiClient;
	private Dictionary<(ProjectType projectType, string slug), int> SlugToIdMapping = new();

	public SlugMapper(ApiClient apiClient) => ApiClient = apiClient;

	public bool TryGetId(string slug, out int id)
	{
		if (int.TryParse(slug, out id))
			return true;

		string[] parts = slug.Split("__");
		ProjectType type = parts[0] switch
		{
			"mod" => ProjectType.Mod,
			"modpack" => ProjectType.Modpack,
			"plugin" => ProjectType.Plugin,
			"respack" => ProjectType.ResourcePack,
			var _ => throw new ArgumentOutOfRangeException()
		};
		
		if (GetId(type, parts[1]) != null)
		{
			id = GetId(type, parts[1])!.Value;
			return true;
		}

		int? idFromCurseForge = GetIdFromCurseForge(type, parts[1]).Result;
		if (idFromCurseForge != null)
		{
			id = idFromCurseForge.Value;
			return true;
		}

		return false;
	}

	public async Task<int?> GetIdFromCurseForge(ProjectType projectType, string slug)
	{
		GenericListResponse<Mod> response = await ApiClient.SearchModsAsync(432, slug: slug, classId: (int)projectType);
		int? id = response.Data.FirstOrDefault()?.Id;
		if (id is not null) SaveId(projectType, slug, id.Value);
		return id;
	}

	public int? GetId(ProjectType projectType, string slug) => SlugToIdMapping.TryGetValue((projectType, slug), out int id) ? id : null;

	public void SaveId(int projectType, string slug, int id) =>
		SaveId((ProjectType)projectType, slug, id);

	public void SaveId(ProjectType projectType, string slug, int id)
	{
		if (SlugToIdMapping.ContainsKey((projectType, slug))) SlugToIdMapping[(projectType, slug)] = id;
		else SlugToIdMapping.Add((projectType, slug), id);
	}

	public void SaveId(Mod project) => SaveId(project.ClassId!.Value, project.Slug, project.Id);

	public void SaveId(BetaMod project) => SaveId((int)project.Class.Id, project.Slug, (int)project.Id);

	public static string FormatSlug(Mod mod)
	{
		string type = mod.ClassId switch
		{
			5 => "plugin",
			6 => "mod",
			12 => "respack",
			4471 => "modpack",
			_ => throw new ArgumentOutOfRangeException()
		}; 
		return $"{type}__{mod.Slug}";
	}

	public static string? FormatSlug(BetaMod mod)
	{
		string type = mod.Class.Id switch
		{
			5 => "plugin",
			6 => "mod",
			12 => "respack",
			4471 => "modpack",
			_ => throw new ArgumentOutOfRangeException()
		}; 
		return $"{type}__{mod.Slug}";
	}
}