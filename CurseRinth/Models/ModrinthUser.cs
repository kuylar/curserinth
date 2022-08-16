using System.Text.Json.Serialization;
using CurseForge.APIClient.Models.Mods;

namespace CurseRinth.Models;

public class ModrinthUser
{
	[JsonPropertyName("username")]
	public string Username { get; }

	[JsonPropertyName("name")]
	public string? Name { get; }

	[JsonPropertyName("email")]
	public string? Email { get; }

	[JsonPropertyName("bio")]
	public string Bio { get; }

	[JsonPropertyName("id")]
	public string Id { get; }

	[JsonPropertyName("github_id")]
	public long GithubId { get; }

	[JsonPropertyName("avatar_url")]
	public Uri AvatarUrl { get; }

	[JsonPropertyName("created")]
	public string Created { get; }

	[JsonPropertyName("role")]
	public string Role { get; }

	public ModrinthUser(ModAuthor author)
	{
		Username = author.Name;	
		Name = null;
		Email = null;
		Bio = "";
		Id = author.Id.ToString();
		GithubId = 0;
		AvatarUrl = new Uri("https://via.placeholder.com/256");
		Created = "1970-01-01T00:00:00Z"; // CF doesnt give this to us /shrug
		Role = "developer";
	}
}