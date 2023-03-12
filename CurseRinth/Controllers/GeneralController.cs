using System.Net;
using System.Web;
using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseRinth.Models;
using Html2Markdown;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CurseRinth.Controllers;

[ApiController]
public class GeneralController : Controller
{
	private ApiClient Api;
	private SlugMapper Slug;

	public GeneralController(ApiClient api, SlugMapper slug)
	{
		Api = api;
		Slug = slug;
	}

	[Route("/")]
	[HttpGet]
	public ModrinthApiInfo Index() =>
		new("The bridge that no one asked for", "https://docs.modrinth.com", "curserinth", "1.0.0");


	[Route("/error")]
	[HttpGet]
	public async Task ErrorHandler()
	{
		IExceptionHandlerFeature exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
		Exception e = exceptionHandlerFeature.Error;
		Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		await Response.WriteAsJsonAsync(new ModrinthError(e));
	}


	[Route("/changelog/{projectId}/{fileId}")]
	[HttpGet]
	public async Task<ContentResult> Changelog(uint projectId, uint fileId)
	{
		GenericResponse<string> modFileChangelogAsync = await Api.GetModFileChangelogAsync(projectId, fileId);
		try
		{
			string changelog = new Converter().Convert(modFileChangelogAsync.Data).Replace("\r\n", "\n");
			changelog = Utils.LinkoutRegex.Replace(changelog,
				match => $"({HttpUtility.UrlDecode(HttpUtility.UrlDecode(match.Groups[1].ToString()))})");
			return Content(changelog, "text/markdown");
		}
		catch
		{
			return Content(modFileChangelogAsync.Data, "text/html");
		}
	}
}