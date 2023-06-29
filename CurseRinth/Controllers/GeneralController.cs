using System.Net;
using System.Text;
using System.Web;
using CurseForge.APIClient;
using CurseForge.APIClient.Models;
using CurseRinth.Models;
using Html2Markdown;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using File = CurseForge.APIClient.Models.Files.File;
using ZipFile = ICSharpCode.SharpZipLib.Zip.ZipFile;

namespace CurseRinth.Controllers;

[ApiController]
public class GeneralController : Controller
{
	private ApiClient Api;
	private SlugMapper Slug;
	private HttpClient _client;

	public GeneralController(ApiClient api, SlugMapper slug)
	{
		Api = api;
		Slug = slug;
		_client = new();
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
	public async Task<ContentResult> Changelog(int projectId, int fileId)
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

	[Route("/convertModpack/{projectId}/{fileId}")]
	[HttpGet]
	public async Task<NotFoundResult> ConvertModpack(int projectId, int fileId)
	{
		// disabled for now
		return NotFound();
		try
		{
			// download the zip archive
			GenericResponse<File> fileInfo = await Api.GetModFileAsync(projectId, fileId);
			HttpResponseMessage response = await _client.GetAsync(fileInfo.Data.DownloadUrl);

			// copy it to memorystream so we can send it back to the user
			using MemoryStream zipStream = new();
			await (await response.Content.ReadAsStreamAsync()).CopyToAsync(zipStream);
			using ZipFile zipArchive = new(zipStream);

			// get CF manifest.json
			ZipEntry manifest = zipArchive.GetEntry("manifest.json");
			if (manifest is null) throw new Exception("manifest.json not found");
			await using Stream manifestStr = zipArchive.GetInputStream(manifest);
			
			// read CF manifest into an object
			using TextReader manifestReader = new StreamReader(manifestStr);
			string manifestJson = await manifestReader.ReadToEndAsync();
			CurseForgeModpackManifest cfManifest =
				JsonConvert.DeserializeObject<CurseForgeModpackManifest>(manifestJson)!;

			// write MR manifest
			zipArchive.BeginUpdate();
			using MemoryStream modrinthIndexStream = new();
			modrinthIndexStream.Write(Encoding.UTF8.GetBytes(
				JsonConvert.SerializeObject(await cfManifest.ToModrinthIndexJson(Api))));
			zipArchive.Add(new StreamDataSource(modrinthIndexStream), "modrinth.index.json",
				CompressionMethod.Stored);
			zipArchive.CommitUpdate();
			
			// send the file
			Response.Headers.Add("Content-Disposition",
				$"attachment; filename=\"{cfManifest.Name}.mrpack\"");
			Response.Headers.Add("Content-Type", "application/zip");
			await Response.StartAsync();
			zipStream.Position = 0;
			await zipStream.CopyToAsync(Response.Body, HttpContext.RequestAborted);
		}
		catch (Exception e)
		{
			Response.StatusCode = 500;
			await Response.StartAsync();
			await Response.WriteAsync(e.ToString());
		}
	}
}