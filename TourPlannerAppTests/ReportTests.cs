using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using TourPlannerApp.BL.Reports;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;

namespace TourPlannerAppTests
{
    public class ReportTests
    {
        private List<TourItem> _tourlist;


        [SetUp]
        public void Setup()
        {
            _tourlist = new List<TourItem>()
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
        }
        

        [Test]
        public void HeaderComponent_Constructor_SpecifiesDateToday()
        {
            // Arrange
            var title = "Title";
            var logo = new byte[] {1, 2, 3};
            
            // Act
            var headerComponent = new HeaderComponent(title, logo);
            var dateToday = headerComponent.CreationTime;

            // Assert
            Assert.That(DateTime.Now.Date == dateToday.Date);
        }
        
        
        [Test]
        public void TourReportMetaDataCreationDataIsSet()
        {
            // Arrange
            var title = "Title";

            // Act
            var tourReport = new TourReport(title, _tourlist.First());
            var metadata = tourReport.GetMetadata();

            // Assert
            Assert.That(DateTime.Now.Date == metadata.CreationDate.Date);
        }

        [Test]
        public void SummaryReportMetaDataCreationDataIsSet()
        {
            // Arrange
            var title = "Title";

            // Act
            var summaryReport = new SummaryReport(title, _tourlist);
            var metadata = summaryReport.GetMetadata();

            // Assert
            Assert.That(DateTime.Now.Date == metadata.CreationDate.Date);
        }
        
        [Test]
        public void TourReport_Constructor_LogoPathNotEmpty()
        {
            // Arrange
            var title = "Title";

            // Act
            var tourReport = new TourReport(title, _tourlist.First());
            var logoPath = tourReport.PathToLogo;

            // Assert
            Assert.IsNotEmpty(logoPath);
        }
        
        [Test]
        public void SummaryReport_Constructor_LogoPathNotEmpty()
        {
            // Arrange
            var title = "Title";

            // Act
            var summaryReport = new SummaryReport(title, _tourlist);
            var logoPath = summaryReport.PathToLogo;

            // Assert
            Assert.IsNotEmpty(logoPath);
        }
        
        
    }
}