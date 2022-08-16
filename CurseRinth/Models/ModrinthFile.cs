using System.Text.Json.Serialization;

namespace CurseRinth.Models;

public class ModrinthFile
{
	[JsonPropertyName("hashes")]
	public Dictionary<string, string> Hashes { get; set; }

	[JsonPropertyName("url")]
	public string Url { get; set; }

	[JsonPropertyName("filename")]
	public string Filename { get; set; }

	[JsonPropertyName("primary")]
	public bool Primary { get; set; }

	[JsonPropertyName("size")]
	public ulong Size { get; set; }
}