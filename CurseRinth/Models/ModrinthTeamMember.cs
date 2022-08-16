namespace CurseRinth.Models;

public class ModrinthTeamMember
{
	public string TeamId { get; }
	public ModrinthUser User { get; }
	public string Role { get; }
	public int Permissions { get; }
	public bool Accepted { get; }

	public ModrinthTeamMember(string teamId, string role, ModrinthUser user)
	{
		TeamId = teamId;
		User = user;
		Role = role;
		Permissions = 127;
		Accepted = true;
	}
}