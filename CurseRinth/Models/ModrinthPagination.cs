using System.Text.Json.Serialization;

namespace CurseRinth.Models;

public class ModrinthPagination<T>
{
	[JsonPropertyName("hits")]
	public List<T> Hits { get; }
	[JsonPropertyName("offset")]
	public int Offset { get; }
	[JsonPropertyName("limit")]
	public int Limit { get; }
	[JsonPropertyName("total_hits")]
	public int TotalHits { get; }
	[JsonPropertyName("_comment")]
	public string? Comment { get; }
	
	public ModrinthPagination(List<T> hits, int offset, int limit, int totalHits, string? comment = null)
	{
		Hits = hits;
		Offset = offset;
		Limit = limit;
		TotalHits = totalHits;
		Comment = comment;
	}
}