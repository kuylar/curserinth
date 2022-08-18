using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using CurseForge.APIClient.Models.Mods;

namespace CurseRinth.Models;

[TypeConverter(typeof(ModrinthFacetsConverter))]
public class ModrinthFacets
{
	private static string[] _validLoaders = {
		"forge",
		"fabric",
		"quilt",
		"liteloader",
		"cauldron"
	};
	
	public List<string> Categories { get; } = new();
	public List<string> Loaders { get; } = new();
	public List<string> Versions { get; } = new();
	public List<string> Licenses { get; } = new();
	public List<string> ProjectTypes { get; } = new();

	public static bool TryParse(string s, out ModrinthFacets result)
	{
		string[][] deserialize = JsonSerializer.Deserialize<string[][]>(s)!;
		result = new ModrinthFacets();
		foreach (string[] strings in deserialize)
		{
			foreach (string s1 in strings)
			{
				string[] kv = s1.Split(":");
				if (kv.Length != 2)
					continue;
				string key = kv[0];
				string value = kv[1];
				switch (key)
				{
					case "categories":
						if (_validLoaders.Contains(value))
							result.Loaders.Add(value);
						result.Categories.Add(value);
						break;
					case "versions":
						result.Versions.Add(value);
						break;
					case "license":
						result.Licenses.Add(value);
						break;
					case "project_type":
						result.ProjectTypes.Add(value);
						break;
				}
			}
		}

		return true;
	}

	public bool HasMultipleOfAnything()
	{
		return Categories.Count > 1 || Versions.Count > 1 || Licenses.Count > 1 || ProjectTypes.Count > 1;
	}

	public uint GetProjectType() =>
		ProjectTypes.FirstOrDefault() switch
		{
			"modpack" => 4471,
			"mod" => 6,
			// following 2 may not be true
			"map" => 17,
			"resourcepack" => 12,
			var _ => 6
		};

	public uint GetCategory() => 
		CategoryMapping.GetInt(GetProjectType(), Categories.FirstOrDefault());

	public string? GetGameVersion() => Versions.FirstOrDefault();

	public ModLoaderType? GetModLoader()
	{
		if (Loaders.Count != 1)
			return ModLoaderType.Any;
		return Loaders[0] switch
		{
			"fabric" => (ModLoaderType)4,
			"forge" => ModLoaderType.Forge,
			"cauldron" => ModLoaderType.Cauldron,
			"liteloader" => ModLoaderType.LiteLoader,
			"quilt" => ModLoaderType.Quilt,
			_ => ModLoaderType.Any
		};
	}
}

public class ModrinthFacetsConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context,
		CultureInfo culture, object value)
	{
		if (value is string s)
		{
			ModrinthFacets facets;
			if (ModrinthFacets.TryParse(s, out facets))
				return facets;
		}

		return base.ConvertFrom(context, culture, value);
	}
}