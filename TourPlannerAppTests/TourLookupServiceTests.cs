using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerAppTests
{
    public class TourLookupServiceTests
    {
        private TourLookupItem _tourLookupItem;
        private Mock<ITourLookupDataAccess> _mockedTourLookupDataAccess { get; set; }
        private TourLookupService _tourLookupService { get; set; }
        
        
        [SetUp]
        public void Setup()
        {
            _mockedTourLookupDataAccess = new Mock<ITourLookupDataAccess>();
            _tourLookupService = new TourLookupService(_mockedTourLookupDataAccess.Object);
            _tourLookupItem = new TourLookupItem()
            {
                Route = new Route()
                {
                    Locations = new List<Location>()
                    {
                        new Location() { AdminArea1 = "AT", AdminArea3 = "Styria", AdminArea4 = "Graz", AdminArea5 = "Graz", PostalCode = "8010", Street = "Einspinnergasse" },
                        new Location() { AdminArea1 = "AT", AdminArea3 = "Vienna", AdminArea4 = "Vienna", AdminArea5 = "Vienna", PostalCode = "1010", Street = "Stephansplatz 1" }
                    },
                    Distance = 200
                }, 
            };
        }
        

        [Test]
        public void GetTour_StatusCodeNot0()
        {
            var tourLookupWithoutResult = new TourLookupItem()
            {
                Info = new Info()
                {
                    Statuscode = 200
                }
            };

            // Arrange
            _mockedTourLookupDataAccess.Setup(da => da.GetTour("a", "b")).Returns(tourLookupWithoutResult);

            // Act
            var tourItem = _tourLookupService.GetTour("a", "b");

            // Assert
            Assert.IsNull(tourItem);
        }
        
        [Test]
        public void GetTour_StatusCode0()
        {
            var tourLookupWithResult = _tourLookupItem;
            tourLookupWithResult.Info = new Info()
                {
                    Statuscode = 0
                };
            
            var tourImage = new byte[] {0, 100, 120, 210, 255};

            // Arrange
            _mockedTourLookupDataAccess.Setup(da => da.GetTour("a", "b")).Returns(tourLookupWithResult);
            _mockedTourLookupDataAccess.Setup(da => da.GetTourImage(tourLookupWithResult)).Returns(tourImage);

            // Act
            var tourItem = _tourLookupService.GetTour("a", "b");

            // Assert
            Assert.AreEqual(tourImage, tourItem.Image);
        }
        
        
        
    }
}