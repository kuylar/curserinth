using System.Text.Json.Serialization;
using CurseForge.APIClient.Models.Mods;

namespace CurseRinth.Models;

public class ModrinthImage
{
	[JsonPropertyName("url")]
	public string Url { get; }

	[JsonPropertyName("featured")]
	public bool Featured { get; }

	[JsonPropertyName("title")]
	public string Title { get; }

	[JsonPropertyName("description")]
	public string Description { get; }

	[JsonPropertyName("created")]
	public string Created { get; }

	public ModrinthImage(ModAsset screenshot)
	{
		Url = screenshot.Url;
		Featured = true;
		Title = screenshot.Title;
		Description = screenshot.Description;
		Created = "1970-01-01T00:00:00Z"; // CF doesnt give this to us /shrug
	}
}