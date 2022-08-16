using CurseForge.APIClient.Models.Mods;
using CurseRinth.Models;

namespace CurseRinth;

// TODO: move this into a SQLite file
public class SlugMapper
{
	private Dictionary<string, uint> SlugToIdMapping = new();

	public bool TryGetId(string slug, out uint id)
	{
		if (uint.TryParse(slug, out id))
			return true;
		if (GetId(slug) != null)
		{
			id = GetId(slug)!.Value;
			return true;
		}

		return false;
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