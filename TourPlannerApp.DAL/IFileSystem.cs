using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public interface IFileSystem
    {
        public string SaveImg(byte[] image);

        public bool DeleteImg(string path);
    }
}
