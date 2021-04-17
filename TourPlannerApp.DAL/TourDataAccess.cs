using Npgsql;
using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public class TourDataAccess : ITourDataAccess
    {
        private string connectionString;

        public TourDataAccess()
        {
            connectionString = SettingsManager.GetSettings().Connection;
        }
        
        
        public List<TourItem> GetAllTours()
        {
            var allTours = new List<TourItem>();

            var conn = Connect();
            var sql = "SELECT name, start_location, target_location, distance, img_path, creation_time, type FROM swe2_tourplanner.tour";
            using var cmd = new NpgsqlCommand(sql, conn);
            //cmd.Parameters.Add(new NpgsqlParameter("@tour_name", "Coole Radtour"));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string startLocation = reader.GetString(1);
                    string targetLocation = reader.GetString(2);
                    float distance = reader.GetFloat(3);
                    string imgPath = reader.GetString(4);
                    //DateTime creationTime = reader.GetString(5);
                    var tourType = GetTourType(reader.GetString(6));

                    var tour = new TourItem { Name = name, StartLocation = startLocation, TargetLocation = targetLocation, Distance = distance, PathToImg = imgPath };
                    allTours.Add(tour);
                }
            }

            conn.Close();
            return allTours;

        }

        public int AddTour(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateTour(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }

        public List<TourItem> Search(string tourName)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteTour(TourItem tourItem)
        {
            return false;
        }

        public bool Exists(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }


        #region Helper Methods

        private NpgsqlConnection Connect()
        {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        private TourItemType GetTourType(string value)
        {
            var type = TourItemType.Undefined;
            switch (value)
            {
                case "Radweg":
                    type = TourItemType.Bicycle;
                    break;
                case "Fussweg":
                    type = TourItemType.Pedestrian;
                    break;
                case "Autostrecke":
                    type = TourItemType.Car;
                    break;
                default:
                    break;
            }

            return type;
        }

        #endregion

    }
}