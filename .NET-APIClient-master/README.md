# CurseForge API Client

[![Nuget](https://img.shields.io/nuget/v/CurseForge.APIClient?style=for-the-badge)](https://www.nuget.org/packages/CurseForge.APIClient/)
[![GitHub issues](https://img.shields.io/github/issues/CurseForgeCommunity/.NET-APIClient?style=for-the-badge)](https://github.com/CurseForgeCommunity/.NET-APIClient/issues)
[![GitHub stars](https://img.shields.io/github/stars/CurseForgeCommunity/.NET-APIClient?style=for-the-badge)](https://github.com/CurseForgeCommunity/.NET-APIClient/stargazers)
[![GitHub license](https://img.shields.io/github/license/CurseForgeCommunity/.NET-APIClient?style=for-the-badge)](https://github.com/CurseForgeCommunity/.NET-APIClient)


This project aims to be a fully functional .NET client that allows you to use the CurseForge API,

All you need is an API key, your partner ID and a contact email to get started.

## Initialize the ApiClient

Please make note that the ApiClient inheirits of `IDisposable`, so that you dispose of it when you're done with it.

```csharp
var cfApiClient = new CurseForge.APIClient.ApiClient(apiKey, partnerId, contactEmail);
```

## How to call the API with the client

### Games

#### Get Games

Get all games that are available to the provided API key.

`GetGamesAsync(int? index = null, int? pageSize = null)`

```csharp
var games = await cfApiClient.GetGamesAsync();
```

#### Get Game

Get a single game. A private game is only accessible by its respective API key.

`GetGameAsync(int gameId)`

```csharp
var game = await cfApiClient.GetGameAsync(gameId);
```

#### Get Game Versions

Get all available versions for each known version type of the specified game. A private game is only accessible to its respective API key.

`GetGameVersionsAsync(int gameId)`

```csharp
var gameVersions = await cfApiClient.GetGameVersionsAsync(gameId);
```

#### Get Game Version Types

`GetGameVersionTypesAsync(int gameId)`

Get all available version types of the specified game.

A private game is only accessible to its respective API key.

Currently, when creating games via the CurseForge Core Console, you are limited to a single game version type. This means that this endpoint is probably not useful in most cases and is relevant mostly when handling existing games that have multiple game versions such as World of Warcraft and Minecraft (e.g. 517 for wow_retail).

```csharp
var gameVersionTypes = await cfApiClient.GetGameVersionTypesAsync(gameId);
```

---

### Categories

#### Get Categories

Get all available classes and categories of the specified game. Specify a game id for a list of all game categories, or a class id for a list of categories under that class.

`GetCategoriesAsync(int? gameId = null, int? classId = null)`

Requires either `gameId` or `classId` for the method to work.

```csharp
var categories = await cfApiClient.GetCategoriesAsync(gameId, classId);
```

---

### Mods

#### Search Mods

Get all mods that match the search criteria.

```csharp
SearchModsAsync(
    int? gameId = null, int? classId = null, int? categoryId = null,
    string gameVersion = null, string searchFilter = null,
    ModsSearchSortField? sortField = null, ModsSearchSortOrder sortOrder = ModsSearchSortOrder.Descending,
    ModLoaderType? modLoaderType = null, int? gameVersionTypeId = null,
    int? index = null, int? pageSize = null
)
```

Requires at least one filter to be filled in.

```csharp
var searchedMods = await cfApiClient.SearchModsAsync(gameId);
```

#### Get Mod

Get a single mod.

`GetModAsync(int modId)`

```csharp
var mod = await cfApiClient.GetModAsync(modId);
```

#### Get Mods

Get a list of mods.

`GetModsAsync(GetModsByIdsListRequestBody body)`

```csharp
var mods = await cfApiClient.GetModsAsync(new GetModsByIdsListRequestBody {
    ModIds = new List<long>() { modId }
});
```

#### Get Featured Mods

Get a list of featured, popular and recently updated mods.

`GetFeaturedModsAsync(GetFeaturedModsRequestBody body)`

```csharp
var featuredMods = await cfApiClient.GetFeaturedModsAsync(new GetFeaturedModsRequestBody {
    GameId = gameId,
    ExcludedModIds = new List<long>(),
    GameVersionTypeId = null
});
```

#### Get Mod Description

Get the full description of a mod in HTML format.

`GetModDescriptionAsync(int modId)`

```csharp
var modDescription = await cfApiClient.GetModDescriptionAsync(modId);
```

---

### Files

#### Get Mod File

Get a single file of the specified mod.

`GetModFileAsync(int modId, int fileId)`

```csharp
var modFile = await cfApiClient.GetModFileAsync(modId, fileId);
```

#### Get Mod Files

Get all files of the specified mod.

`GetModFilesAsync(int modId)`

```csharp
var modFiles = await cfApiClient.GetModFilesAsync(modId);
```

#### Get Files

Get a list of files.

`GetFilesAsync(GetModFilesRequestBody body)`

```csharp
var files = await cfApiClient.GetFilesAsync(new GetModFilesRequestBody {
    FileIds = new List<long> { fileId }
});
```

#### Get Mod File Changelog

Get the changelog of a file in HTML format.

`GetModFileChangelogAsync(int modId, int fileId)`

```csharp
var modFileChangelog = await cfApiClient.GetModFileChangelogAsync(modId, fileId);
```

#### Get Mod File Download URL

Get a download url for a specific file.

`GetModFileDownloadUrlAsync(int modId, int fileId)`

```csharp
var modFileDownloadUrl = await cfApiClient.GetModFileDownloadUrlAsync(modId, fileId);
```

---

### Fingerprints

#### Get Fingerprints Matches

Get mod files that match a list of fingerprints.

`GetFingerprintMatchesAsync(GetFingerprintMatchesRequestBody body)`

```csharp
var modFile = await cfApiClient.GetFingerprintMatchesAsync(new GetFingerprintMatchesRequestBody {
    Fingerprints = new List<long>() { fingerprint }
});
```

#### Get Fingerprints Fuzzy Matches

Get mod files that match a list of fingerprints using fuzzy matching.

`GetFingerprintsFuzzyMatchesAsync(GetFuzzyMatchesRequestBody body)`

```csharp
var modFile = await cfApiClient.GetFingerprintsFuzzyMatchesAsync(new GetFuzzyMatchesRequestBody {
    GameId = gameId,
    Fingerprints = new List<FolderFingerprint> { 
        new FolderFingerprint {
            Foldername = folderName,
            Fingerprints = new List<long> { fingerprint }
        }
    }
});
```

---

### Minecraft

#### Get Minecraft Versions

This method allows you to fetch the Minecraft versions available to CurseForge

`GetMinecraftVersions(bool sortDescending)`

#### Get Specific Minecraft Version

Get information about a specific Minecraft version

`GetSpecificMinecraftVersionInfo(string gameVersion)`

#### Get Minecraft ModLoaders

Get all modloaders for Minecraft

`GetMinecraftModloaders(string version, bool includeAll)`

#### Get Specific Minecraft ModLoader

Gets information about a specific modloader

`GetSpecificMinecraftModloaderInfo(string modloaderName)`