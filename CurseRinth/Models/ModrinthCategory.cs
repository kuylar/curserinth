using System.Text.Json.Serialization;
using CurseForge.APIClient.Models;

namespace CurseRinth.Models;

public class ModrinthCategory
{
	[JsonPropertyName("icon")]
	public string Icon { get; }
	[JsonPropertyName("name")]
	public string Name { get; }
	[JsonPropertyName("project_type")]
	public string ProjectType { get; }

	public ModrinthCategory(Category category, string projectType)
	{
		Icon =
			$"<svg viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\"><image x=\"0\" y=\"0\" width=\"24\" height=\"24\" xlink:href=\"{category.IconUrl}\"/></svg>";
		Name = category.Slug;
		ProjectType = projectType;
	}
}