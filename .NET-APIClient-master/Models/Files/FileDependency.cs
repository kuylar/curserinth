namespace CurseForge.APIClient.Models.Files
{
    public class FileDependency
    {
        public int ModId { get; set; }
        public int FileId { get; set; }
        public FileRelationType RelationType { get; set; }
    }
}
