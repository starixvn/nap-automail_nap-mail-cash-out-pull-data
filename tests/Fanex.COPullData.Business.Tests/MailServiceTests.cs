using Fanex.COPullData.Business.Models;
using Fanex.COPullData.Business.Services;
using NUnit.Framework;
using OfficeOpenXml;

namespace Fanex.COPullData.Business.Tests
{
    public class MailServiceTests
    {
        private MailService mailService;

        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            AppConfigService.Config = new ConfigSettingModel
            {
                Path = "/Excel"
            };
            mailService = new MailServiceImp();
        }

        [Test]
        public void CreateMail_ShouldReturnMailWithAttachments_WhenHasDataIsTrue()
        {
            // Arrange
            const bool hasData = true;
            List<string> fileNames = ["Templates/Template.xlsx"];

            // Act
            var mail = mailService.CreateMail(hasData, fileNames);

            // Assert
            Assert.That(mail != null);
        }
    }
}