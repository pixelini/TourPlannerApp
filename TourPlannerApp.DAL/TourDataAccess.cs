using Npgsql;
using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public class TourDataAccess : ITourDataAccess
    {
        private string _connectionString;

        public TourDataAccess()
        {
            _connectionString = SettingsManager.GetSettings().Connection;
        }
        
        
        public List<TourItem> GetAllTours()
        {
            /*
            var tourlist = new List<TourItem>
            {
                new TourItem() { Name = "Coole Tour", StartLocation = "Wien", TargetLocation = "Graz" },
                new TourItem() { Name = "Super Tour", StartLocation = "Wien", TargetLocation = "Graz" }, 
                new TourItem() { Name = "Fade Tour", StartLocation = "Wien", TargetLocation = "Graz" }, 
                new TourItem() { Name = "Top Tour", StartLocation = "Wien", TargetLocation = "Graz" }, 
                new TourItem() { Name = "Steile Tour", StartLocation = "Wien", TargetLocation = "Graz" }
            };

            return tourlist;
            */
            
            var allTours = new List<TourItem>();

            var conn = Connect();
            var sql = "SELECT id, name, start_location, target_location, distance, img_path, type FROM swe2_tourplanner.tour";
            using var cmd = new NpgsqlCommand(sql, conn);
            //cmd.Parameters.Add(new NpgsqlParameter("@tour_name", "Coole Radtour"));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string startLocation = reader.GetString(2);
                    string targetLocation = reader.GetString(3);
                    float distance = reader.GetFloat(4);
                    string imgPath = reader.GetString(5);
                    var tourType = GetTourType(reader.GetString(6));

                    var tour = new TourItem { Id = id, Name = name, StartLocation = startLocation, TargetLocation = targetLocation, Distance = distance, PathToImg = imgPath };
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

        public bool DeleteTour(TourItem tourItem)
        {
            bool success = false;

            var conn = Connect();
            var sql = "DELETE FROM swe2_tourplanner.tour WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter("@id", tourItem.Id));
            cmd.Prepare();

            if (cmd.ExecuteNonQuery() == 1)
            {
                success = true;
            }

            conn.Close();
            return success;
        }

        public List<TourItem> SearchByName(string tourName)
        {
            throw new System.NotImplementedException();
        }


        public bool Exists(TourItem tourItem)
        {
            bool success = false;
            var conn = Connect();
            var sql = "SELECT * FROM swe2_tourplanner.tour WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter("@id", tourItem.Id));
            cmd.Prepare();

            var reader = cmd.ExecuteReader();
            success = reader.HasRows;
            conn.Close();
            return success;
        }


        #region Helper Methods

        private NpgsqlConnection Connect()
        {
            var conn = new NpgsqlConnection(_connectionString);
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