using CurseForge.APIClient;
using CurseForge.APIClient.Models;

namespace CurseRinth;

public static class CategoryMapping
{
	private static Dictionary<string, uint> ModCategoriesMapping;
	private static Dictionary<string, uint> ModpackCategoriesMapping;
	private static Dictionary<string, uint> MapCategoriesMapping;
	private static Dictionary<string, uint> ResourcePackCategoriesMapping;

	public static List<Category> ModCategories;
	public static List<Category> ModpackCategories;
	public static List<Category> MapCategories;
	public static List<Category> ResourcePackCategories;

	public static async Task Set(ApiClient api)
	{
		GenericListResponse<Category> modcats = await api.GetCategoriesAsync(432, 6);
		GenericListResponse<Category> mpcats = await api.GetCategoriesAsync(432, 4471);
		GenericListResponse<Category> mapcats = await api.GetCategoriesAsync(432, 17);
		GenericListResponse<Category> rpcats = await api.GetCategoriesAsync(432, 12);

		ModCategoriesMapping = modcats.Data.ToDictionary(Utils.GetCategoryName, x => x.Id);
		ModpackCategoriesMapping = mpcats.Data.ToDictionary(Utils.GetCategoryName, x => x.Id);
		MapCategoriesMapping = mapcats.Data.ToDictionary(Utils.GetCategoryName, x => x.Id);
		ResourcePackCategoriesMapping = rpcats.Data.ToDictionary(Utils.GetCategoryName, x => x.Id);
		
		ModCategories = modcats.Data;
		ModpackCategories = mpcats.Data;
		MapCategories = mapcats.Data;
		ResourcePackCategories = rpcats.Data;
	}

	public static uint GetInt(uint classId, string? slug)
	{
		if (slug is null) return 0;
		try
		{
			switch (classId)
			{
				case 6:
					return ModCategoriesMapping[slug];
				case 4471:
					return ModpackCategoriesMapping[slug];
				case 17:
					return MapCategoriesMapping[slug];
				case 12:
					return ResourcePackCategoriesMapping[slug];
			}
		}
		catch
		{
			return 0;
		}

		return 0;
	}
}