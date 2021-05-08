using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;
using TourPlannerApp.Models.Models;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.DAL
{
    public class TourLookupDataAccess : ITourLookupDataAccess
    {
        private static HttpClient _client { get; set; }

        private static string _baseAddress = "https://www.mapquestapi.com/";

        private static string _apiKey;

        public TourLookupDataAccess()
        {
            _client = new HttpClient();
            _apiKey = SettingsManager.GetSettings().ApiKey;
            _client.BaseAddress = new Uri(_baseAddress);
        }


        public TourLookupItem GetTour(string from, string to)
        {
            TourLookupItem lookupTaskResult = null;

            Task.Run(async () => { 
                lookupTaskResult = await GetTourAsync(from, to); 
            }).Wait();

            return lookupTaskResult;
        }


        private static async Task<TourLookupItem> GetTourAsync(string from, string to)
        {
            var content = "";
            var bodyAsString = GetPostRequestBodyAsJson(from, to);
            var bodyAsJson = new StringContent(bodyAsString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync(_baseAddress + "directions/v2/route?key=" + _apiKey, bodyAsJson);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successful");
                content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                var tourResult = JsonConvert.DeserializeObject<TourLookupItem>(content);
                return tourResult;
            }

            return null;
        }

        private static string GetPostRequestBodyAsJson(string from, string to)     
        {
            var requestBody = new TourLookupPostRequestBody.RequestBodyContent
            {
                locations = new List<string> { from, to },
                options = new TourLookupPostRequestBody.Options { routeType = "bicycle", locale = "de_DE", unit = "k" }
            };

            var json = JsonConvert.SerializeObject(requestBody);

            return json;
        }

        public byte[] GetTourImage(TourLookupItem tour)
        {
            byte[] lookupImgTaskResult = null;

            Task.Run(async () => {
                lookupImgTaskResult = await GetTourImageAsync(tour);
            }).Wait();

            return lookupImgTaskResult;
        }

        private static async Task<byte[]> GetTourImageAsync(TourLookupItem tour)
        {
            byte[] content = null;

            string coordinates = GetFormattedCoordinatesString(tour.Route.BoundingBox);

            string requestString = String.Format(
                "{0}staticmap/v5/map?key={1}&size={2}&defaultMarker={3}&routeColor={4}&zoom={5}&rand={6}&session={7}&boundingBox={8}&type={9}&routeWidth={10}",
                _baseAddress,
                _apiKey,
                "400,400",
                "marker-BE1931-sm",
                "4DB5FF",
                "11",
                "737758036",
                tour.Route.SessionId, //session id
                coordinates, // latitude and longitude of start and target location
                "light",
                "5"
                );

            Debug.WriteLine(requestString);

            HttpResponseMessage response = await _client.GetAsync(requestString);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successful");
                content = await response.Content.ReadAsByteArrayAsync();
                Debug.WriteLine(content);
                return content;
            }

            return null;
        }

        private static string GetFormattedCoordinatesString(BoundingBox boundingBox)
        {
            string coordinatesString = String.Format("{0},{1},{2},{3}",
                boundingBox.Ul.Lat.ToString(CultureInfo.GetCultureInfo("en-EN").NumberFormat),
                boundingBox.Ul.Lng.ToString(CultureInfo.GetCultureInfo("en-EN").NumberFormat),
                boundingBox.Lr.Lat.ToString(CultureInfo.GetCultureInfo("en-EN").NumberFormat),
                boundingBox.Lr.Lng.ToString(CultureInfo.GetCultureInfo("en-EN").NumberFormat)
                );

            return coordinatesString;
        }
    }
}
