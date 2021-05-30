using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            
            var allTours = new List<TourItem>();

            var conn = Connect();
            var sql = "SELECT id, name, sl_street, sl_zip, sl_county, sl_country, tl_street, tl_zip, tl_county, tl_country, distance, img_path, type, description FROM swe2_tourplanner.tour";

            using var cmd = new NpgsqlCommand(sql, conn);

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
                    var description = reader.GetString(13);

                    var tour = new TourItem { Id = id, Name = name, StartLocation = startAddress, TargetLocation = targetAddress, Distance = distance, PathToImg = imgPath, Description = description };
                    allTours.Add(tour);
                }
            }

            conn.Close();
            return allTours;

        }

        public int AddTour(TourItem tourItem)
        {
            int success = 1;

            var conn = Connect();
            var sql = "INSERT INTO swe2_tourplanner.tour (name, sl_street, sl_zip, sl_country, sl_county, tl_street, tl_zip, tl_country, tl_county, distance, img_path, type, description) VALUES (@name, @sl_street, @sl_postalcode, @sl_country, @sl_county, @tl_street, @tl_postalcode, @tl_country, @tl_county, @distance, @img_path, @type, @description)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter("@name", tourItem.Name));
            cmd.Parameters.Add(new NpgsqlParameter("@sl_street", tourItem.StartLocation.Street));
            cmd.Parameters.Add(new NpgsqlParameter("@sl_postalcode", tourItem.StartLocation.PostalCode));
            cmd.Parameters.Add(new NpgsqlParameter("@sl_country", tourItem.StartLocation.Country));
            cmd.Parameters.Add(new NpgsqlParameter("@sl_county", tourItem.StartLocation.County));
            cmd.Parameters.Add(new NpgsqlParameter("@tl_street", tourItem.TargetLocation.Street));
            cmd.Parameters.Add(new NpgsqlParameter("@tl_postalcode", tourItem.TargetLocation.PostalCode));
            cmd.Parameters.Add(new NpgsqlParameter("@tl_country", tourItem.TargetLocation.Country));
            cmd.Parameters.Add(new NpgsqlParameter("@tl_county", tourItem.TargetLocation.County));
            cmd.Parameters.Add(new NpgsqlParameter("@distance", tourItem.Distance));
            cmd.Parameters.Add(new NpgsqlParameter("@img_path", "/img/test.png"));
            cmd.Parameters.Add(new NpgsqlParameter("@type", "Fussweg"));
            cmd.Parameters.Add(new NpgsqlParameter("@description", tourItem.Description));
            cmd.Prepare();

            if (cmd.ExecuteNonQuery() == 1)
            {
                success = 1;
            }

            conn.Close();
            return success;
        }

        public bool UpdateTour(TourItem tourItem)
        {
            bool success = false;

            var conn = Connect();
            var sql = "UPDATE swe2_tourplanner.tour SET name=@name, description=@description WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter("@name", tourItem.Name));
            cmd.Parameters.Add(new NpgsqlParameter("@description", tourItem.Description));
            cmd.Parameters.Add(new NpgsqlParameter("@id", tourItem.Id));
            cmd.Prepare();

            if (cmd.ExecuteNonQuery() == 1)
            {
                success = true;
            }

            conn.Close();
            return success;
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

        public int AddTourLog(int id, LogEntry newLogEntry)
        {
            int success = 1;

            var conn = Connect();
            var sql = "INSERT INTO swe2_tourplanner.log (tour_id, start_time, end_time, description, distance, overall_time, rating, altitude) VALUES (@tour_id, @start_time, @end_time, @description, @distance, @overall_time, @rating, @altitude)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter("@tour_id", id));
            cmd.Parameters.Add(new NpgsqlParameter("@start_time", newLogEntry.StartTime));
            cmd.Parameters.Add(new NpgsqlParameter("@end_time", newLogEntry.EndTime));
            cmd.Parameters.Add(new NpgsqlParameter("@description", newLogEntry.Description));
            cmd.Parameters.Add(new NpgsqlParameter("@distance", newLogEntry.Distance));
            cmd.Parameters.Add(new NpgsqlParameter("@overall_time", newLogEntry.OverallTime));
            cmd.Parameters.Add(new NpgsqlParameter("@rating", newLogEntry.Rating));
            cmd.Parameters.Add(new NpgsqlParameter("@altitude", newLogEntry.Altitude));
            cmd.Prepare();

            if (cmd.ExecuteNonQuery() == 1)
            {
                success = 1;
            }

            conn.Close();
            return success;
        }

        public List<LogEntry> GetAllLogsForTour(TourItem selectedTour)
        {
            var allLogs = new List<LogEntry>();

            var conn = Connect();
            var sql = "SELECT tour_id, start_time, end_time, description, distance, overall_time, rating, altitude FROM swe2_tourplanner.log WHERE tour_id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.Add(new NpgsqlParameter("@id", selectedTour.Id));
            cmd.Prepare();

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DateTime startTime = reader.GetDateTime(1);
                    DateTime endTime = reader.GetDateTime(2);
                    string description = reader.GetString(3);
                    float distance = reader.GetFloat(4);
                    TimeSpan overallTime = reader.GetTimeSpan(5);
                    int rating = reader.GetInt32(6);
                    float altitude = reader.GetFloat(7);

                    var logEntry = new LogEntry { Id = id, StartTime = startTime, EndTime = endTime, Description = description, Distance = distance, OverallTime = overallTime, Rating = rating, Altitude = altitude };
                    allLogs.Add(logEntry);
                }
            }

            conn.Close();
            return allLogs;
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
