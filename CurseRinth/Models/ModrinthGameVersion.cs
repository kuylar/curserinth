using System.Text.Json.Serialization;
using CurseForge.APIClient.Models.Minecraft;

namespace CurseRinth.Models;

public class ModrinthGameVersion
{
	[JsonPropertyName("version")]
	public string Version { get; }

	[JsonPropertyName("version_type")]
	public string VersionType { get; }

	[JsonPropertyName("date")]
	public string Date { get; }

	[JsonPropertyName("major")]
	public bool Major { get; }

	public ModrinthGameVersion(MinecraftVersionInfo version)
	{
		Version = version.VersionString;
		VersionType = "release";
		Date = version.DateModified.ToString("O");
		Major = version.VersionString.Length < 5;
	}
}