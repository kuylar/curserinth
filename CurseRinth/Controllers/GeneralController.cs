using System.Net;
using CurseRinth.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CurseRinth.Controllers;

[ApiController]
public class GeneralController : Controller
{
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
}