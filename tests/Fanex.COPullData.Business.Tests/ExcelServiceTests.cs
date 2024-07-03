using Fanex.COPullData.Business.Models;
using Fanex.COPullData.Business.Services;
using NUnit.Framework;
using OfficeOpenXml;

namespace Fanex.COPullData.Business.Tests
{
    public class ExcelServiceTests
    {
        private ExcelService excelService;

        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            AppConfigService.Config = new ConfigSettingModel
            {
                Path = "/Excel"
            };
            excelService = new ExcelServiceImp();
        }

        [Test]
        public void Export_ShouldReturnFilePath_WhenDataIsProvided()
        {
            // Arrange
            var data = new List<COModel>
            {
                new COModel { bet_id = "1"},
                new COModel {bet_id = "2"}
            };
            var beginDate = new DateTime(2022, 1, 1);
            var endDate = new DateTime(2022, 1, 31);

            // Act
            var result = excelService.Export(data, beginDate, endDate);

            // Assert
            Assert.That(result, Is.Not.Null.Or.Empty);
            Assert.That(File.Exists(result), Is.True);
        }
    }
}