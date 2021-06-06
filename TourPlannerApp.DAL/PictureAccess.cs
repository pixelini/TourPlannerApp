using System;
using System.IO;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public class PictureAccess : IPictureAccess
    {
        private string _pictureFolder;

        public PictureAccess()
        {
            _pictureFolder = SettingsManager.GetSettings().BasePath + @"\SavedImages";
        }

        public string SavePicture(byte[] image)
        {
            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + ".png";

            if (!Directory.Exists(_pictureFolder))
            {
                Directory.CreateDirectory(_pictureFolder);
            }

            var fullPath = _pictureFolder + "\\" + fileName;

            try
            {
                File.WriteAllBytes(fullPath, image); // exception abfangen --> if not successfull --> return ""?
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
            

            return fullPath;

        }

        public bool DeletePicture(string path)
        {
            bool success = false;

            if (File.Exists(path))
            {
                File.Delete(path);
                success = true;
            }

            return success;
        }

        public bool Exists(string path)
        {
            bool success = false;

            if (File.Exists(path))
            {
                success = true;
            }

            return success;
        }
    }
}
