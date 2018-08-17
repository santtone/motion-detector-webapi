using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
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
            listRequest.PageSize = 1000;
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

        public async Task<File> UploadFile(string filePath)
        {
            var fileMetadata = new File
            {
                Name = Path.GetFileName(filePath),
                Parents = new List<string>
                {
                    "1cnSgFlewdvvYVJ3x_vtdlP9c4ECox5Vv"
                }
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, System.IO.FileMode.Open))
            {
                request = _service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                await request.UploadAsync();
            }

            var file = request.ResponseBody;
            Debug.WriteLine("File ID: " + file.Id);
            return file;
        }

        public async Task DeleteFile(string id)
        {
            var deleteRequest = _service.Files.Delete(id);
            var task = await deleteRequest.ExecuteAsync();
        }

        public async Task DeleteAllFiles()
        {
            var ids = await ListFileIds();
            if (ids.Count == 0)
            {
                return;
            }

            var batch = new BatchRequest(_service);

            void Callback(Permission permission, RequestError error, int index, HttpResponseMessage message)
            {
                if (error != null)
                {
                    Console.WriteLine(error.Message);
                }
            }

            ids.ForEach(id =>
            {
                var deleteRequest = _service.Files.Delete(id);
                batch.Queue(deleteRequest, (BatchRequest.OnResponse<Permission>) Callback);
            });
            await batch.ExecuteAsync();
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