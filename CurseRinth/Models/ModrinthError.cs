namespace CurseRinth.Models;

public class ModrinthError
{
	public string Error { get; }
	public string Description { get; }

	public ModrinthError(string error, string description)
	{
		Error = error;
		Description = description;
	}

	public ModrinthError(string error, Exception e)
	{
		Error = error;
		Description = e.Message;
	}

	public ModrinthError(Exception e)
	{
		Error = "internal_error";
		Description = e.Message;
	}
}
