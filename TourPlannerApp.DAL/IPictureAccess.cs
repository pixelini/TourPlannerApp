using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public interface IPictureAccess
    {
        public string SavePicture(byte[] image);

        public bool DeletePicture(string path);

        public bool Exists(string path);
    }
}
