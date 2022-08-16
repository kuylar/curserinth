namespace CurseRinth.Models;

public class ModrinthDependencies
{
	public ModrinthDependencies(List<ModrinthProject> projects, List<ModrinthVersion> versions)
	{
		Projects = projects.ToArray();
		Versions = versions.ToArray();
	}

	public ModrinthProject[] Projects { get; }
	public ModrinthVersion[] Versions { get; }
}