namespace CurseRinth.Models;

public class ModrinthLicense
{
	public string Id { get; }
	public string Name { get; }
	public string Url { get; }

	public ModrinthLicense()
	{
		Id = "IHNIL";
		Name = "I have no idea license";
		Url = "https://kuylar.dev/curselinth-ihnil.html";
	}

	public ModrinthLicense(string id, string name, string url)
	{
		Id = id;
		Name = name;
		Url = url;
	}
}