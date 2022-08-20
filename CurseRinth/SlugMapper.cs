using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;

namespace CurseRinth;

// TODO: move this into a SQLite file
public class SlugMapper
{
	public ApiClient ApiClient;
	private Dictionary<string, uint> SlugToIdMapping = new();

	public SlugMapper(ApiClient apiClient) => ApiClient = apiClient;

	public bool TryGetId(string slug, out uint id)
	{
		if (uint.TryParse(slug, out id))
			return true;
		if (GetId(slug) != null)
		{
			id = GetId(slug)!.Value;
			return true;
		}

		if (GetIdFromCurseForge(slug).Result != null)
		{
			id = GetId(slug)!.Value;
			return true;
		}

		return false;
	}

	public async Task<uint?> GetIdFromCurseForge(string slug)
	{
		GenericListResponse<Mod> response = await ApiClient.SearchModsAsync(432, slug: slug);
		uint? id = response.Data.FirstOrDefault()?.Id;
		if (id is not null) SaveId(slug, id.Value);
		return id;
	}
	
	public uint? GetId(string slug) => SlugToIdMapping.TryGetValue(slug, out uint id) ? id : null;

	public void SaveId(string slug, uint id)
	{
		if (SlugToIdMapping.ContainsKey(slug)) SlugToIdMapping[slug] = id;
		else SlugToIdMapping.Add(slug, id);
	}

	public void SaveId(Mod project) => SaveId(project.Slug, project.Id);
	public void SaveId(ModrinthProject project) => SaveId(project.Slug, uint.Parse(project.Id));
	public void SaveId(ModrinthSearchResult project) => SaveId(project.Slug, uint.Parse(project.Id));
}