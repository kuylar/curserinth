using CurseForge.APIClient.Models.Files;
using System;
using System.Collections.Generic;

namespace CurseForge.APIClient.Models.Mods
{
    public class Mod
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ModLinks Links { get; set; }
        public string Summary { get; set; }
        public ModStatus Status { get; set; }
        public double DownloadCount { get; set; }
        public bool IsFeatured { get; set; }
        public int PrimaryCategoryId { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public int? ClassId { get; set; }
        public List<ModAuthor> Authors { get; set; } = new List<ModAuthor>();
        public ModAsset Logo { get; set; }
        public List<ModAsset> Screenshots { get; set; } = new List<ModAsset>();
        public int MainFileId { get; set; }
        public List<File> LatestFiles { get; set; } = new List<File>();
        public List<FileIndex> LatestFilesIndexes { get; set; } = new List<FileIndex>();
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public DateTimeOffset DateReleased { get; set; }
        public bool? AllowModDistribution { get; set; }
        public int GamePopularityRank { get; set; }
        public bool IsAvailable { get; set; }
        public int? ThumbsUpCount { get; set; }
    }
}
