using System.Text.RegularExpressions;
using CurseForge.APIClient.Models.Mods;

namespace CurseRinth;

public static class Utils
{
	public static Regex LinkoutRegex = new("\\(\\/linkout\\?remoteUrl=(.+?)\\)");
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
}