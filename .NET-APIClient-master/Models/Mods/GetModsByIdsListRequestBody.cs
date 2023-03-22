using System.Collections.Generic;

namespace CurseForge.APIClient.Models.Mods
{
    public class GetModsByIdsListRequestBody
    {
        public List<int> ModIds { get; set; } = new List<int>();
    }
}
