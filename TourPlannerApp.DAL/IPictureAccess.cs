namespace TourPlannerApp.DAL
{
    public interface IPictureAccess
    {
        public string SavePicture(byte[] image);

        public bool DeletePicture(string path);

        public bool Exists(string path);
    }
}
