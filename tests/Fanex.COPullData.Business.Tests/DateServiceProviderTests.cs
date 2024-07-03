using Fanex.COPullData.Business.Models;
using Fanex.COPullData.Business.Services;
using NUnit.Framework;

namespace Fanex.COPullData.Business.Tests
{
    public class DateServiceProviderTests
    {
        private DateServiceProvider dateServiceProvider;

        [SetUp]
        public void Setup()
        {
            dateServiceProvider = new DateServiceProviderImp();
        }

        [Test]
        public void DateService_ShouldReturnDateServiceManual_WhenIsManualIsTrue()
        {
            // Arrange
            AppConfigService.Config = new ConfigSettingModel { IsManual = true };

            // Act
            DateService dateService = dateServiceProvider.DateService();

            // Assert
            Assert.That(dateService, Is.InstanceOf<DateServiceManual>());
        }

        [Test]
        public void DateService_ShouldReturnDateServiceDaily_WhenIsManualIsFalse()
        {
            // Arrange
            AppConfigService.Config = new ConfigSettingModel { IsManual = false };

            // Act
            DateService dateService = dateServiceProvider.DateService();

            // Assert
            Assert.That(dateService, Is.InstanceOf<DateServiceDaily>());
        }
    }
}