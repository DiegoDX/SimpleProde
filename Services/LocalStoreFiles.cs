
using Microsoft.AspNetCore.Connections.Features;

namespace SimpleProde.Services
{
    public class LocalStoreFiles : IStoreFiles
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LocalStoreFiles(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) 
        { 
            this.environment = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Store(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(environment.WebRootPath, container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);
            using (var ms = new MemoryStream()) 
            {
                await file.CopyToAsync(ms);
                var contenido = ms.ToArray();
                await File.WriteAllBytesAsync(path, contenido);
            }

            var request = httpContextAccessor.HttpContext!.Request;

            var url = $"{request.Scheme}://{request.Host}";
            var urlFile = Path.Combine(url, container, fileName).Replace("\\", "/");
            return urlFile;
        }


        public Task Delete(string? path, string container)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Task.CompletedTask;
            }

            var nombreArchivo = Path.GetFileName(path);
            var directorioArchivo = Path.Combine(environment.WebRootPath, container, nombreArchivo);

            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.CompletedTask;
        }
    }
}
