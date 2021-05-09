using Npgsql;
using System.Collections.Generic;
using TourPlannerApp.Models;
using static TourPlannerApp.Models.TourItem;

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
            //var sql = "SELECT id, name, start_location, target_location, distance, img_path, type FROM swe2_tourplanner.tour";

            var sql = "SELECT id, name, sl_street, sl_zip, sl_county, sl_country, tl_street, tl_zip, tl_county, tl_country, distance, img_path, type FROM swe2_tourplanner.tour";


            using var cmd = new NpgsqlCommand(sql, conn);
            //cmd.Parameters.Add(new NpgsqlParameter("@tour_name", "Coole Radtour"));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);

                    var startAddress = new Address();
                    startAddress.Street = reader.GetString(2);
                    startAddress.PostalCode = reader.GetString(3);
                    startAddress.County = reader.GetString(4);
                    startAddress.Country = reader.GetString(5);
                    var targetAddress = new Address();
                    targetAddress.Street = reader.GetString(6);
                    targetAddress.PostalCode = reader.GetString(7);
                    targetAddress.County = reader.GetString(8);
                    targetAddress.Country = reader.GetString(9);
                    float distance = reader.GetFloat(10);
                    string imgPath = reader.GetString(11);
                    var tourType = GetTourType(reader.GetString(12));

                    var tour = new TourItem { Id = id, Name = name, StartLocation = startAddress, TargetLocation = targetAddress, Distance = distance, PathToImg = imgPath };
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