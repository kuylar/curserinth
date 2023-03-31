using Newtonsoft.Json;

namespace CurseRinth;

public class BetaMod
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("gameId")]
    public long GameId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; }

    [JsonProperty("author")]
    public BetaAuthor? Author { get; set; }

    [JsonProperty("hasComments")]
    public bool HasComments { get; set; }

    [JsonProperty("licenseType")]
    public string LicenseType { get; set; }

    [JsonProperty("wikiUrl")]
    public string WikiUrl { get; set; }

    [JsonProperty("donationUrl")]
    public Uri DonationUrl { get; set; }

    [JsonProperty("issuesUrl")]
    public Uri IssuesUrl { get; set; }

    [JsonProperty("issuesSource")]
    public long IssuesSource { get; set; }

    [JsonProperty("sourceUrl")]
    public Uri SourceUrl { get; set; }

    [JsonProperty("sourceSource")]
    public long SourceSource { get; set; }

    [JsonProperty("hasPages")]
    public bool HasPages { get; set; }

    [JsonProperty("gameVersion")]
    public string GameVersion { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("updateDate")]
    public long UpdateDate { get; set; }

    [JsonProperty("creationDate")]
    public long CreationDate { get; set; }

    [JsonProperty("releaseDate")]
    public long ReleaseDate { get; set; }

    [JsonProperty("downloads")]
    public long Downloads { get; set; }

    [JsonProperty("avatarUrl")]
    public Uri AvatarUrl { get; set; }

    [JsonProperty("fileSize")]
    public long FileSize { get; set; }

    [JsonProperty("latestFileId")]
    public long LatestFileId { get; set; }

    [JsonProperty("class")]
    public Class Class { get; set; }

    [JsonProperty("primaryCategoryId")]
    public long PrimaryCategoryId { get; set; }

    [JsonProperty("members")]
    public Member[] Members { get; set; }

    [JsonProperty("categories")]
    public Class[] Categories { get; set; }

    [JsonProperty("screenshots")]
    public object Screenshots { get; set; }

    [JsonProperty("mainFile")]
    public MainFile MainFile { get; set; }

    [JsonProperty("isClientCompatible")]
    public bool IsClientCompatible { get; set; }
}

public class BetaAuthor
{
    [JsonProperty("id")] public int Id;
    [JsonProperty("isEarlyAccessAuthor")] public bool IsEarlyAccessAuthor;
    [JsonProperty("name")] public string Name;
}

public class Class
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonProperty("gameId")]
    public long GameId { get; set; }

    [JsonProperty("iconUrl")]
    public Uri IconUrl { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("classId")]
    public long? ClassId { get; set; }

    [JsonProperty("isClass")]
    public bool IsClass { get; set; }

    [JsonProperty("parentCategoryId")]
    public long? ParentCategoryId { get; set; }

    [JsonProperty("displayIndex", NullValueHandling = NullValueHandling.Ignore)]
    public long? DisplayIndex { get; set; }
}

public class MainFile
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("changelogBody")]
    public string ChangelogBody { get; set; }

    [JsonProperty("childFileType")]
    public long ChildFileType { get; set; }

    [JsonProperty("claimedByUserId")]
    public object ClaimedByUserId { get; set; }

    [JsonProperty("dateClaimed")]
    public object DateClaimed { get; set; }

    [JsonProperty("dateCreated")]
    public DateTimeOffset DateCreated { get; set; }

    [JsonProperty("dateModified")]
    public DateTimeOffset DateModified { get; set; }

    [JsonProperty("displayName")]
    public object DisplayName { get; set; }

    [JsonProperty("fileLength")]
    public long FileLength { get; set; }

    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("gameIdentifier")]
    public object GameIdentifier { get; set; }

    [JsonProperty("parentProjectFileId")]
    public object ParentProjectFileId { get; set; }

    [JsonProperty("projectFileTypeId")]
    public object ProjectFileTypeId { get; set; }

    [JsonProperty("gameVersions")]
    public string[] GameVersions { get; set; }

    [JsonProperty("gameVersionTypeIds")]
    public long[] GameVersionTypeIds { get; set; }

    [JsonProperty("projectId")]
    public long ProjectId { get; set; }

    [JsonProperty("releaseType")]
    public long ReleaseType { get; set; }

    [JsonProperty("status")]
    public long Status { get; set; }

    [JsonProperty("totalDownloads")]
    public long TotalDownloads { get; set; }

    [JsonProperty("uploadSource")]
    public long UploadSource { get; set; }

    [JsonProperty("user")]
    public User User { get; set; }
}

public class User
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("twitchAvatarUrl")]
    public Uri TwitchAvatarUrl { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
}

public class Member
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("twitchId")]
    public long TwitchId { get; set; }

    [JsonProperty("avatarUrl")]
    public Uri AvatarUrl { get; set; }
}