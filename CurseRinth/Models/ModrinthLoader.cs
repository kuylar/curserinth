using System.Text.Json.Serialization;
using CurseForge.APIClient.Models.Enums;
using CurseForge.APIClient.Models.Minecraft;
using CurseForge.APIClient.Models.Mods;

namespace CurseRinth.Models;

public class ModrinthLoader
{
	[JsonPropertyName("icon")]
	public string Icon { get; }
	[JsonPropertyName("name")]
	public string Name { get; }
	[JsonPropertyName("supported_project_types")]
	public string[] SupportedProjectTypes =>
		new []
		{
			"mod", "modpack"
		};

	private static Dictionary<CoreModloaderType, string> ModrinthSvgs = new()
	{
		[CoreModloaderType.Forge] = "<svg xml:space=\"preserve\" fill-rule=\"evenodd\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-miterlimit=\"1.5\" clip-rule=\"evenodd\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0z\"></path><path fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" d=\"M2 7.5h8v-2h12v2s-7 3.4-7 6 3.1 3.1 3.1 3.1l.9 3.9H5l1-4.1s3.8.1 4-2.9c.2-2.7-6.5-.7-8-6Z\"></path></svg>",
		[CoreModloaderType.Fabric] = "<svg xmlns=\"http://www.w3.org/2000/svg\" xml:space=\"preserve\" fill-rule=\"evenodd\" stroke-linecap=\"round\" stroke-linejoin=\"round\" clip-rule=\"evenodd\" viewBox=\"0 0 24 24\"><path fill=\"none\" d=\"M0 0h24v24H0z\"/><path fill=\"none\" stroke=\"currentColor\" stroke-width=\"23\" d=\"m820 761-85.6-87.6c-4.6-4.7-10.4-9.6-25.9 1-19.9 13.6-8.4 21.9-5.2 25.4 8.2 9 84.1 89 97.2 104 2.5 2.8-20.3-22.5-6.5-39.7 5.4-7 18-12 26-3 6.5 7.3 10.7 18-3.4 29.7-24.7 20.4-102 82.4-127 103-12.5 10.3-28.5 2.3-35.8-6-7.5-8.9-30.6-34.6-51.3-58.2-5.5-6.3-4.1-19.6 2.3-25 35-30.3 91.9-73.8 111.9-90.8\" transform=\"matrix(.08671 0 0 .0867 -49.8 -56)\"/></svg>",
		[CoreModloaderType.Quilt] = "<svg xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" fill-rule=\"evenodd\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-miterlimit=\"2\" clip-rule=\"evenodd\" viewBox=\"0 0 24 24\"><defs><path id=\"quilt\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"65.6\" d=\"M442.5 233.9c0-6.4-5.2-11.6-11.6-11.6h-197c-6.4 0-11.6 5.2-11.6 11.6v197c0 6.4 5.2 11.6 11.6 11.6h197c6.4 0 11.6-5.2 11.6-11.7v-197Z\"></path></defs><path fill=\"none\" d=\"M0 0h24v24H0z\"></path><use xlink:href=\"#quilt\" stroke-width=\"65.6\" transform=\"matrix(.03053 0 0 .03046 -3.2 -3.2)\"></use><use xlink:href=\"#quilt\" stroke-width=\"65.6\" transform=\"matrix(.03053 0 0 .03046 -3.2 7)\"></use><use xlink:href=\"#quilt\" stroke-width=\"65.6\" transform=\"matrix(.03053 0 0 .03046 6.9 -3.2)\"></use><path fill=\"none\" stroke=\"currentColor\" stroke-width=\"70.4\" d=\"M442.5 234.8c0-7-5.6-12.5-12.5-12.5H234.7c-6.8 0-12.4 5.6-12.4 12.5V430c0 6.9 5.6 12.5 12.4 12.5H430c6.9 0 12.5-5.6 12.5-12.5V234.8Z\" transform=\"rotate(45 3.5 24) scale(.02843 .02835)\"></path></svg>",
		[CoreModloaderType.LiteLoader] = "<svg clip-rule=\"evenodd\" fill-rule=\"evenodd\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-miterlimit=\"1.5\" version=\"1.1\" viewBox=\"0 0 24 24\" xml:space=\"preserve\" xmlns=\"http://www.w3.org/2000/svg\"><rect width=\"24\" height=\"24\" fill=\"none\"/><path d=\"m3.924 21.537s3.561-1.111 8.076-6.365c2.544-2.959 2.311-1.986 4-4.172\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2px\"/><path d=\"m7.778 19s1.208-0.48 4.222 0c2.283 0.364 6.037-4.602 6.825-6.702 1.939-5.165 0.894-10.431 0.894-10.431s-4.277 4.936-6.855 7.133c-5.105 4.352-6.509 11-6.509 11\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2px\"/></svg>",
	};

	public static readonly ModrinthLoader Forge = new(CoreModloaderType.Forge);
	public static readonly ModrinthLoader Fabric = new(CoreModloaderType.Fabric);
	public static readonly ModrinthLoader Quilt = new(CoreModloaderType.Quilt);
	public static readonly ModrinthLoader LiteLoader = new(CoreModloaderType.LiteLoader);

	public ModrinthLoader(MinecraftModloaderInfoListItem modLoader)
	{
		Icon = ModrinthSvgs.TryGetValue(modLoader.Type, out string? svg)
			? svg
			: "<svg viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\"></svg>";
		Name = modLoader.Type.ToString().ToLower();
	}

	public ModrinthLoader(CoreModloaderType type)
	{
		Icon = ModrinthSvgs.TryGetValue(type, out string? svg)
			? svg
			: "<svg viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\"></svg>";
		Name = type.ToString().ToLower();
	}

	public ModrinthLoader(string icon, string name)
	{
		Icon = icon;
		Name = name;
	}
}