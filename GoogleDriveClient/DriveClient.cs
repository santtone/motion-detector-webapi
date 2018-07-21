using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using File = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveClient
{
    public class GDrive
    {
        private readonly string[] _scopes = {DriveService.Scope.Drive};
        private const string ApplicationName = "Motion Detector";
        private readonly DriveService _service;

        public GDrive(string serviceAccountEmail, string keyFilePath,
            string password)
        {
            _service = AuthenticateServiceAccount(serviceAccountEmail, keyFilePath, password);
        }

        public List<File> ListFiles()
        {
            var listRequest = _service.Files.List();
            var files = listRequest.Execute().Files;
            Debug.WriteLine(files.Count > 0 ? $"{files.Count} files found" : "No files found");
            return files.ToList();
        }

        public void UploadFile(string filePath)
        {
            var fileMetadata = new File
            {
                Name = "photo.jpg"
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(filePath,
                System.IO.FileMode.Open))
            {
                request = _service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
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