using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using File = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveClient
{
    public class DriveClient
    {
        private readonly string[] _scopes = {DriveService.Scope.Drive};
        private const string ApplicationName = "Motion Detector";
        private readonly DriveService _service;

        public DriveClient(string serviceAccountEmail, string keyFilePath,
            string password)
        {
            _service = AuthenticateServiceAccount(serviceAccountEmail, keyFilePath, password);
        }

        public async Task<List<File>> ListFiles()
        {
            var listRequest = _service.Files.List();
            listRequest.Fields =
                "files/thumbnailLink, files/name, files/mimeType, files/id, files/webViewLink, files/webContentLink, files/size, files/createdTime";
            var execution = await listRequest.ExecuteAsync();
            Debug.WriteLine(execution.Files.Count > 0 ? $"{execution.Files.Count} files found" : "No files found");
            return execution.Files.ToList();
        }

        public async Task<List<string>> ListFileIds()
        {
            var files = await ListFiles();
            return files.Select(f => f.Id).ToList();
        }

        public void UploadFile(string filePath)
        {
            var fileMetadata = new File
            {
                Name = "photo.jpg",
                Parents = new List<string>
                {
                    "1cnSgFlewdvvYVJ3x_vtdlP9c4ECox5Vv"
                }
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                request = _service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            Debug.WriteLine("File ID: " + file.Id);
        }

        private DriveService AuthenticateServiceAccount(string serviceAccountEmail, string keyFilePath,
            string password)
        {
            var certificate = new X509Certificate2(keyFilePath, password, X509KeyStorageFlags.Exportable);
            try
            {
                var credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = _scopes
                    }.FromCertificate(certificate));

                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return null;
        }
    }
}