using Firebase.Auth;
using Firebase.Storage;

using IntelXLWeb.Models;

namespace IntelXLWeb.Utilities
{
    public static class StorageHelper
    {
        public static async Task<string> UploadFileToStorage(IFormFile file, FireBaseStorageConfig config)
        {
            string filePath = "";
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(config.ApiKey));
                var authResult = await auth.SignInWithEmailAndPasswordAsync(config.AuthEmail, config.AuthPassword);

                var storage = new FirebaseStorage(
                config.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authResult.FirebaseToken),
                    ThrowOnCancel = true
                });

                string folderName = "TutorProfiles";
                string fileName = Path.GetFileName(file.FileName);

                // Upload the file to Firebase Storage
                using (var stream = file.OpenReadStream())
                {
                    filePath = await storage
                        .Child(folderName)
                        .Child(fileName)
                        .PutAsync(stream);
                }
            }
           catch (Exception ex)
            {
              
            }
            return filePath;
        }
    }
}
