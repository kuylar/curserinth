using System.Text.Json.Serialization;
using CurseForge.APIClient.Models.Files;

namespace CurseRinth.Models;

public class ModrinthDependency
{
	[JsonPropertyName("version_id")]
	public string VersionId { get; }

	[JsonPropertyName("project_id")]
	public string ProjectId { get; }

	[JsonPropertyName("dependency_type")]
	public string DependencyType { get; }

	public ModrinthDependency(FileDependency fileDependency)
	{
		VersionId = fileDependency.FileId.ToString();
		ProjectId = fileDependency.ModId.ToString();
		DependencyType = fileDependency.RelationType.ToString();
	}
}