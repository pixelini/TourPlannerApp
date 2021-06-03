using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Moq;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.Models.Models;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerAppTests
{
    public class ModelTests
    {
        
        [SetUp]
        public void Setup()
        {
          
        }

        #region TourItem

        [Test]
        public void LocationStringWithAllProperties_Returns_True()
        {
            // Arrange
            var tour = new TourItem
            {
                StartLocation = new TourItem.Address()
                {
                    Street = "Street 1",
                    PostalCode = "8010",
                    City = "Graz", 
                    County = "Steiermark", 
                    Country = "AT"
                }
            };

            // Act
            var expectedString = tour.GetStartLocationAsString();
            
            // Assert
            Assert.AreEqual("Street 1, 8010, Graz, Steiermark, AT", expectedString);

        }
        
        [Test]
        public void LocationStringWithoutStreet_Returns_True()
        {
            // Arrange
            var tour = new TourItem
            {
                StartLocation = new TourItem.Address()
                {
                    PostalCode = "8010",
                    City = "Graz", 
                    County = "Steiermark", 
                    Country = "AT"
                }
            };

            // Act
            var expectedString = tour.GetStartLocationAsString();
            
            // Assert
            Assert.AreEqual("8010, Graz, Steiermark, AT", expectedString);
        }
        
        
        [Test]
        public void LocationStringWithoutPostalCode_Returns_True()
        {
            // Arrange
            var tour = new TourItem
            {
                StartLocation = new TourItem.Address()
                {
                    Street = "Street 1",
                    City = "Graz", 
                    County = "Steiermark", 
                    Country = "AT"
                }
            };

            // Act
            var expectedString = tour.GetStartLocationAsString();
            
            // Assert
            Assert.AreEqual("Street 1, Graz, Steiermark, AT", expectedString);
        }

        #endregion

        
        
        #region TourLookup

        [Test]
        public void ConvertTourLookupToTourItem_Returns_True()
        {
            // Arrange
            var locations = new List<Location>()
            {
                new Location() { AdminArea1 = "AT", AdminArea3 = "Styria", AdminArea4 = "Graz", AdminArea5 = "Graz", PostalCode = "8010", Street = "Einspinnergasse" },
                new Location() { AdminArea1 = "AT", AdminArea3 = "Vienna", AdminArea4 = "Vienna", AdminArea5 = "Vienna", PostalCode = "1010", Street = "Stephansplatz 1" }
            };

            var tourLookup = new TourLookupItem()
            {
                Route = new Route()
                {
                    Locations = locations,
                    Distance = 200
                }, 
            };

            // Act
            var tourItem = new TourItem();
            tourItem = ConvertTourLookupToTourItem(tourLookup);

            // Assert
            Assert.AreEqual(tourItem.Distance, tourLookup.Route.Distance);
            Assert.AreEqual(tourItem.StartLocation.Street, tourLookup.Route.Locations[0].Street);
            Assert.AreEqual(tourItem.StartLocation.PostalCode, tourLookup.Route.Locations[0].PostalCode);
            Assert.AreEqual(tourItem.StartLocation.City, tourLookup.Route.Locations[0].AdminArea5);
            Assert.AreEqual(tourItem.StartLocation.Country, tourLookup.Route.Locations[0].AdminArea1);
            Assert.AreEqual(tourItem.StartLocation.County, tourLookup.Route.Locations[0].AdminArea4);
            Assert.AreEqual(tourItem.TargetLocation.Street, tourLookup.Route.Locations[1].Street);
            Assert.AreEqual(tourItem.TargetLocation.PostalCode, tourLookup.Route.Locations[1].PostalCode);
            Assert.AreEqual(tourItem.TargetLocation.City, tourLookup.Route.Locations[1].AdminArea5);
            Assert.AreEqual(tourItem.TargetLocation.Country, tourLookup.Route.Locations[1].AdminArea1);
            Assert.AreEqual(tourItem.TargetLocation.County, tourLookup.Route.Locations[1].AdminArea4);
        }

        #endregion
        
        
    }
}