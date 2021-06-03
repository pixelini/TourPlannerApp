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
    public class TourServiceTests
    {
        private List<TourItem> _tourList;
        private TourItem _tour;
        private List<LogEntry> _logEntries;
        
        private Mock<ITourDataAccess> _mockedTourDataAccess { get; set; }
        private Mock<IPictureAccess> _mockedPictureAccess { get; set; }
        private TourService _tourService { get; set; }
        
        
        [SetUp]
        public void Setup()
        {
            _mockedTourDataAccess = new Mock<ITourDataAccess>();
            _mockedPictureAccess = new Mock<IPictureAccess>();
            _tourService = new TourService(_mockedTourDataAccess.Object, _mockedPictureAccess.Object);
            
            _tourList = new List<TourItem>()
            {
                new TourItem()
                { 
                    Name = "Coole Tour",  
                    StartLocation = new TourItem.Address()
                    {
                        Street = "Street 1",
                        City = "Graz", 
                        County = "Steiermark", 
                        Country = "AT"
                    }, 
                    TargetLocation = new TourItem.Address()
                    {
                        Street = "Street 2",
                        City = "Wien", 
                        County = "Wien", 
                        Country = "AT"
                    }, 
                }
            };
            
            _tour = new TourItem
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

            _logEntries = new List<LogEntry>()
            {
                new LogEntry(){ Rating = 5, EndTime = DateTime.Now, Distance = 200 }
            };
        }
        

        [Test]
        public void SaveSummaryReport_SavesPDF_Returns_True()
        {
            // Arrange
            _mockedTourDataAccess.Setup(da => da.GetAllTours()).Returns(_tourList);
            _mockedTourDataAccess.Setup(da => da.GetAllLogsForTour(_tourList.First())).Returns(_logEntries);
            var filepath = Environment.CurrentDirectory + @"\TestSummaryReport.pdf";
            
            // Act
            _tourService.SaveSummaryReport(filepath);

            // Assert
            Assert.That(File.Exists(filepath));

            // Cleanup
            DeleteIfExists(filepath);
        }
        
        
        [Test]
        public void SaveTourReport_SavesPDF_Returns_True()
        {
            // Arrange
            _mockedTourDataAccess.Setup(da => da.GetAllLogsForTour(_tourList.First())).Returns(_logEntries);
            var filepath = Environment.CurrentDirectory + @"\TestTourReport.pdf";
            
            // Act
            _tourService.SaveTourReport(_tourList.First(), filepath);

            // Assert
            Assert.That(File.Exists(filepath));

            // Cleanup
            DeleteIfExists(filepath);
        }


        [Test]
        public void TestDraft()
        {

            // Arrange

            // Act

            // Assert
        }
        
        
        private void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        
        
        
    }
}