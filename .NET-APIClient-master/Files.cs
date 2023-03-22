using CurseForge.APIClient.Models;
using CurseForge.APIClient.Models.Files;
using System.Threading.Tasks;

namespace CurseForge.APIClient
{
    public partial class ApiClient
    {
        public async Task<GenericResponse<File>> GetModFileAsync(int modId, int fileId)
        {
            return await GET<GenericResponse<File>>($"/v1/mods/{modId}/files/{fileId}");
        }

        public async Task<GenericListResponse<File>> GetModFilesAsync(int modId, string gameVersionFlavor = null, int? index = null, int? pageSize = null)
        {
            return await GET<GenericListResponse<File>>($"/v1/mods/{modId}/files",
                ("gameVersionFlavor", gameVersionFlavor), ("index", index), ("pageSize", pageSize)
            );
        }

        public async Task<GenericListResponse<File>> GetFilesAsync(GetModFilesRequestBody body)
        {
            return await POST<GenericListResponse<File>>("/v1/mods/files", body);
        }

        public async Task<GenericResponse<string>> GetModFileChangelogAsync(int modId, int fileId)
        {
            return await GET<GenericResponse<string>>($"/v1/mods/{modId}/files/{fileId}/changelog");
        }

        public async Task<GenericResponse<string>> GetModFileDownloadUrlAsync(int modId, int fileId)
        {
            return await GET<GenericResponse<string>>($"/v1/mods/{modId}/files/{fileId}/download-url");
        }
    }
}
