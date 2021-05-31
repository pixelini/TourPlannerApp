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
    public class FileSystem : IFileSystem
    {
        private string _imgPath;

        public FileSystem()
        {
            _imgPath = SettingsManager.GetSettings().BasePath + @"\SavedImages";
        }

        public string SaveImg(byte[] image)
        {
            Guid guid = Guid.NewGuid();
            string fileName = guid.ToString() + ".png";

            if (!Directory.Exists(_imgPath))
            {
                Directory.CreateDirectory(_imgPath);

            }

            var fullPath = _imgPath + "\\" + fileName;

            File.WriteAllBytes(fullPath, image); // exception abfangen

            return fullPath;

        }

        public bool DeleteImg(string path)
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
