using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;
using System.Drawing;

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

            File.WriteAllBytes(fullPath, image); // exception abfangen

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
    }
}
