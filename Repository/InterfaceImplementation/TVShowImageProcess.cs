using OTTMyPlatform.Models;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Repository.Interface.Context;

namespace OTTMyPlatform.Repository.InterfaceImplementation
{
    public class TVShowImageProcess : ITVShowImageProcess
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDBContext _dBContext;
        public TVShowImageProcess(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IDBContext dBContext)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _dBContext = dBContext;
        }
        public async Task RemoveTVShowImage(int showId)
        {
            string imagePath = "";
            string imageFolderPath = GetImageFolderPath();

            using (var context = new OttplatformContext())
            {
                var getData = await context.Tvshows.FindAsync(showId);
                imagePath = imageFolderPath + getData.tvShowImage;

                if (System.IO.File.Exists(imagePath))
                {
                    //if old image is there then it will delete it
                    System.IO.File.Delete(imagePath);
                }
            }
        }
        private string GetImageFolderPath()
        {
            return _webHostEnvironment.WebRootPath + "/Uploads/TVShowImages/";
        }

        public bool UploadeFiles(IFormFileCollection uploadeFile)
        {
            bool result = false;
            foreach (IFormFile source in uploadeFile)
            {
                result = UploadeFile(source);
            }
            return result;
        }
        private bool UploadeFile(IFormFile source)
        {
            string fileName = source.FileName;
            string fileFolderPath = GetImageFolderPath();

            try
            {
                if (!System.IO.Directory.Exists(fileFolderPath))
                {
                    System.IO.Directory.CreateDirectory(fileFolderPath);
                }

                string imagePath = fileFolderPath + fileName;

                if (System.IO.File.Exists(imagePath))
                {
                    //if old image is there then it will delete it
                    System.IO.File.Delete(imagePath);
                }

                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    // it will uploade actual binary data into dummy file
                    source.CopyTo(stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetTVShowImage(string tvShowImage)
        {
            string imageUrl = string.Empty;
            string hostUrl = "https://localhost:7005//";

            string imaheFolderPath = GetImageFolderPath();
            string imagePath = imaheFolderPath + tvShowImage;

            if (!System.IO.File.Exists(imagePath))
            {
                imageUrl = hostUrl + "//Uploads//TVShowCommanImage/defaultImage.jpg";
            }
            else
            {
                imageUrl = hostUrl + "/Uploads/TVShowImages/" + tvShowImage;
            }

            return imageUrl;

        }
    }
}
