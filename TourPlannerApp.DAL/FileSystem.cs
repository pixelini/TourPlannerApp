using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public class FileSystem : IFileSystem
    {
        private string _imgPath;

        public FileSystem()
        {
            _imgPath = Environment.CurrentDirectory + @"\img";
        }

        public byte[] GetImg(TourItem tourItem)
        {
            throw new NotImplementedException();
        }

        public string SaveImg(TourItem tourItem, string fileName)
        {
            if (!Directory.Exists(_imgPath))
            {
                Directory.CreateDirectory(_imgPath);
            }

            throw new NotImplementedException();
        }
    }
}
