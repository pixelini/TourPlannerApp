using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Docnet.Core;
using Docnet.Core.Models;
using Moq;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;

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
                    Distance = 200,
                    Description = "Toll."
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
                    Country = "AT",
                },
                Distance = 200,
                Description = "Toll."
            };

            _logEntries = new List<LogEntry>()
            {
                new LogEntry(){ Rating = 5, EndTime = DateTime.Now, Distance = 200 }
            };
        }
        

        [Test]
        public void SaveSummaryReport_SavesPDF()
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
        public void SaveTourReport_SavesPDF()
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
        public void SaveTourReport_ReportContainsProperties()
        {
            // Arrange
            _mockedTourDataAccess.Setup(da => da.GetAllLogsForTour(_tourList.First())).Returns(_logEntries);
            var filepath = Environment.CurrentDirectory + @"\TestTourReport.pdf";
            var allProperties = new List<string>()
            {
                _tourList.First().Name,
                _tourList.First().Description,
                _tourList.First().Distance.ToString()
            };
            
            // Act
            _tourService.SaveTourReport(_tourList.First(), filepath);
            var containsAllProperties = PdfContainsAllTextsInGivenList(filepath, allProperties);
            
            // Assert
            Assert.IsTrue(containsAllProperties);
            
            // Cleanup
            DeleteIfExists(filepath);
        }
        
        [Test]
        public void SaveTourReport_ReportContainsCorrectTitle()
        {
            // Arrange
            _mockedTourDataAccess.Setup(da => da.GetAllLogsForTour(_tourList.First())).Returns(_logEntries);
            var filepath = Environment.CurrentDirectory + @"\TestTourReport.pdf";

            // Act
            _tourService.SaveTourReport(_tourList.First(), filepath);
            var containsAllProperties = PdfContainsText(filepath, "Report von Tour");
            
            // Assert
            Assert.IsTrue(containsAllProperties);
            
            // Cleanup
            DeleteIfExists(filepath);
        }
        
        
        [Test]
        public void SaveSummaryReport_ReportContainsStatistikInTitle()
        {
            // Arrange
            _mockedTourDataAccess.Setup(da => da.GetAllTours()).Returns(_tourList);
            _mockedTourDataAccess.Setup(da => da.GetAllLogsForTour(_tourList.First())).Returns(_logEntries);
            var filepath = Environment.CurrentDirectory + @"\TestSummaryReport.pdf";
            
            // Act
            _tourService.SaveSummaryReport(filepath);
            var containsStatistikInTitle = PdfContainsText(filepath, "Statistik");

            // Assert
            Assert.IsTrue(containsStatistikInTitle);
            
            // Cleanup
            DeleteIfExists(filepath);
        }
        
        [Test]
        public void GetAllTours_FillEmptyPathsWithDefaultPaths()
        {
            // Arrange
            var tourListWithOutImgPaths = _tourList;
            
            // Act
            _mockedTourDataAccess.Setup(da => da.GetAllTours()).Returns(tourListWithOutImgPaths);
            var allTours = _tourService.GetAllTours();

            // Assert
            Assert.AreEqual("/Images/default.png", tourListWithOutImgPaths.First().PathToImg);
        }
        
        [Test]
        public void GetAllTours_DoNotFillAssignedPathsWithDefaultPaths()
        {
            // Arrange
            var tourListWithImgPaths = _tourList;
            _tourList.First().PathToImg = "Path/that/should/not/be/replaced.png";

            _mockedTourDataAccess.Setup(da => da.GetAllTours()).Returns(tourListWithImgPaths);
            _mockedPictureAccess.Setup(pa => pa.Exists(tourListWithImgPaths.First().PathToImg)).Returns(true);
            
            // Act
            var allTours = _tourService.GetAllTours();

            // Assert
            Assert.AreEqual("Path/that/should/not/be/replaced.png", allTours.First().PathToImg);
        }
        
        [Test]
        public void GetAllTours_DoNotExistingPathsWithDefaultPaths()
        {
            // Arrange
            var tourListWithImgPaths = _tourList;
            _tourList.First().PathToImg = "Path/that/does/not/exist.png";

            _mockedTourDataAccess.Setup(da => da.GetAllTours()).Returns(tourListWithImgPaths);
            _mockedPictureAccess.Setup(pa => pa.Exists(tourListWithImgPaths.First().PathToImg)).Returns(false);
            
            // Act
            var allTours = _tourService.GetAllTours();

            // Assert
            Assert.AreEqual("/Images/default.png", allTours.First().PathToImg);
        }
        
        [Test]
        public void AddTour_SavingImageNotSuccessful_PathReplacedWithMinus()
        {
            // Arrange
            var tourWithoutPath = _tour;
            _tour.PathToImg = "";

            _mockedPictureAccess.Setup(pa => pa.SavePicture(tourWithoutPath.Image)).Returns("");
            _mockedTourDataAccess.Setup(da => da.AddTour(tourWithoutPath)).Returns(1);
            
            // Act
            var id = _tourService.AddTour(tourWithoutPath);

            // Assert
            Assert.AreEqual("-", tourWithoutPath.PathToImg);
        }
        
        [Test]
        public void AddTour_SavingImageSuccessful_PathShouldBeStoredInTourItem()
        {
            // Arrange
            var tourWithoutPath = _tour;
            _tour.PathToImg = "valid/path/to/img.png";
            _tour.Image = new byte[]{ 0, 100, 120, 210, 255 };

            _mockedPictureAccess.Setup(pa => pa.SavePicture(tourWithoutPath.Image)).Returns("valid/path/to/img.png");
            _mockedTourDataAccess.Setup(da => da.AddTour(tourWithoutPath)).Returns(1);
            
            // Act
            var id = _tourService.AddTour(tourWithoutPath);

            // Assert
            Assert.AreEqual("valid/path/to/img.png", tourWithoutPath.PathToImg);
        }

        private void DeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        
        
        private bool PdfContainsAllTextsInGivenList(string path, List<string> allTextsToFind)
        {
            return allTextsToFind.Select(text => PdfContainsText(path, text)).All(itemFound => itemFound);
        }

        private bool PdfContainsText(string path, string textToFind)
        {
            using (var docReader = DocLib.Instance.GetDocReader(path, new PageDimensions()))
            {
                for (var i = 0; i < docReader.GetPageCount(); i++)
                {
                    using (var pageReader = docReader.GetPageReader(i))
                    {
                        var text = pageReader.GetText();
                        if (text.Contains(textToFind))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        
        
    }
}