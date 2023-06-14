using Newtonsoft.Json;

namespace CurseRinth;

public class ModrinthModpackIndex
{
	[JsonProperty("formatVersion")] public long FormatVersion { get; set; }
	[JsonProperty("game")] public string Game { get; set; }
	[JsonProperty("versionId")] public string VersionId { get; set; }
	[JsonProperty("name")] public string Name { get; set; }
	[JsonProperty("summary")] public string Summary { get; set; }
	[JsonProperty("files")] public File[] Files { get; set; }
	[JsonProperty("dependencies")] public Dictionary<string, string> Dependencies { get; set; }

	public class File
	{
		[JsonProperty("path")] public string Path { get; set; }
		[JsonProperty("hashes")] public Dictionary<string, string> Hashes { get; set; }
		[JsonProperty("downloads")] public string[] Downloads { get; set; }
		[JsonProperty("fileSize")] public ulong FileSize { get; set; }
	}
}