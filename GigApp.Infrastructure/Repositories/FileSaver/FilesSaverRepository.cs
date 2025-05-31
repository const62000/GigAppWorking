using GigApp.Application.Interfaces.FileSaver;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LMS.Repositories.Implementation.FileSaver
{
    public class FilesSaverRepository : IFilesSaverRepository
    {
        private readonly IWebHostEnvironment _environment;

        public FilesSaverRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveImage(string fileName,string folder, IFormFile file,string baseUrl)
        {
            fileName = fileName + Path.GetExtension(file.FileName);
            return await SaveFile(fileName, folder, file, baseUrl);
        }

        private async Task<string> SaveFile(string fileName, string folder,IFormFile file, string baseUrl, string subFolder = "" )
        {
            if (string.IsNullOrEmpty(subFolder))
            {
                if (file == null || file.Length == 0)
                    return string.Empty;

                // Create a folder path to save the file (ensure the folder exists)
                var uploadsFolderPath = Path.Combine(_environment.WebRootPath, folder);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Generate a unique file name
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, folder, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                filePath = $"{baseUrl}/{folder}/{fileName}";
                // Return success response
                return filePath;
            }
            else
            {
                if (file == null || file.Length == 0)
                    return string.Empty;

                // Create a folder path to save the file (ensure the folder exists)
                var uploadsFolderPath = Path.Combine(_environment.WebRootPath, folder, subFolder);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Generate a unique file name
                //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, folder, subFolder, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                filePath = $"{baseUrl}/{folder}/{subFolder}/{fileName}";
                // Return success response
                return filePath;
            }
        }
    }
}
