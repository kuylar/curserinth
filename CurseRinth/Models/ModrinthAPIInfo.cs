namespace CurseRinth.Models;

public class ModrinthApiInfo
{
	public string About { get; }
	public string Documentation { get; }
	public string Name { get; }
	public string Version { get; }

	public ModrinthApiInfo(string about, string documentation, string name, string version)
	{
		About = about;
		Documentation = documentation;
		Name = name;
		Version = version;
	}
}