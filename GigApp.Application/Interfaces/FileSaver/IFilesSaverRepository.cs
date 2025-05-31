using Microsoft.AspNetCore.Http;

namespace GigApp.Application.Interfaces.FileSaver
{
    public interface IFilesSaverRepository
    {
        Task<string> SaveImage(string fileName, string folder, IFormFile file, string baseUrl);
    }
}
