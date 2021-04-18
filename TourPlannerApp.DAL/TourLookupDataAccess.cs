using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

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


        public TourItem GetTour(string from, string to)
        {
            var content = GetTourAsync(from, to);

            return null;
        }


        private static async Task<TourItem> GetTourAsync(string from, string to)
        {
            var content = "";

            var bodyAsString = GetPostRequestBody(from, to);
           
            var bodyAsJson = new StringContent(bodyAsString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_baseAddress + "directions/v2/route?key=" + _apiKey, bodyAsJson);

            Debug.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successful");
                content = await response.Content.ReadAsStringAsync();
                var test = JObject.Parse(content);
                Debug.WriteLine(content);
            }

          
            return null;
        }

        private static string GetPostRequestBody(string from, string to)
        {
            string json = @"{
                            'locations': [
                                'Wien',
                                'Graz'
                            ],
                            'options': {
                                        'avoids': [],
                                'avoidTimedConditions': false,
                                'doReverseGeocode': true,
                                'shapeFormat': 'raw',
                                'generalize': 0,
                                'routeType': 'bicycle',
                                'timeType': 1,
                                'locale': 'de_DE',
                                'unit': 'k',
                                'enhancedNarrative': false,
                                'drivingStyle': 2,
                                'highwayEfficiency': 21.0,
                                    }
                                }
                                ";


            return json;
        }

    }
}
