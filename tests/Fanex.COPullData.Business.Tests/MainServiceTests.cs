using Fanex.COPullData.Business.Models;
using Fanex.COPullData.Business.Services;
using Fanex.MailDelivery.Core.Models;
using Moq;
using NUnit.Framework;

namespace Fanex.COPullData.Business.Tests
{
    public class MainServiceTests
    {
        private Mock<Log> logMock;
        private Mock<DataService> dataServiceMock;
        private Mock<DateServiceProvider> dateServiceProviderMock;
        private Mock<ExcelService> excelServiceMock;
        private Mock<MailService> mailServiceMock;
        private MainServiceImp mainService;

        [SetUp]
        public void Setup()
        {
            logMock = new Mock<Log>();
            dataServiceMock = new Mock<DataService>();
            dateServiceProviderMock = new Mock<DateServiceProvider>();
            excelServiceMock = new Mock<ExcelService>();
            mailServiceMock = new Mock<MailService>();

            mainService = new MainServiceImp(
                logMock.Object,
                dataServiceMock.Object,
                dateServiceProviderMock.Object,
                excelServiceMock.Object,
                mailServiceMock.Object
            );
        }

        [Test]
        public void Execute_ShouldReturnJobMessageWithSuccessStatus_WhenSendNotificationSucceeds()
        {
            // Arrange
            var result = new ExecuteResultModel
            {
                Success = true,
                Mail = new Mail(),
                Message = string.Empty
            };
            var expectedJobMessage = new JobMessage
            {
                Mail = result.Mail,
                JobStatus = JobStatus.Success,
                TaskLog = new TaskLog
                {
                    LogTime = It.IsAny<DateTime>(),
                    Message = result.Message
                }
            };

            mailServiceMock.Setup(m => m.CreateMail(It.IsAny<bool>(), It.IsAny<List<string>>())).Returns(result.Mail);
            dataServiceMock.Setup(d => d.PullData(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<COModel>());
            excelServiceMock.Setup(e => e.Export(It.IsAny<List<COModel>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns("filename");
            dateServiceProviderMock.Setup(d => d.DateService()).Returns(new DateServiceDaily());

            // Act
            var jobMessage = mainService.Execute();

            // Assert
            Assert.That(expectedJobMessage.Mail.Equals(jobMessage.Mail));
        }

        [Test]
        public void Execute_ShouldReturnJobMessageWithDataIsNotReadyStatus_WhenSendNotificationThrowsException()
        {
            // Arrange
            var exceptionMessage = "An error occurred.";
            var expectedJobMessage = new JobMessage
            {
                Mail = new Mail(),
                JobStatus = JobStatus.DataIsNotReady,
                TaskLog = new TaskLog
                {
                    LogTime = It.IsAny<DateTime>(),
                    Message = exceptionMessage
                }
            };

            mailServiceMock.Setup(m => m.CreateMail(It.IsAny<bool>(), It.IsAny<List<string>>())).Throws(new Exception(exceptionMessage));
            dateServiceProviderMock.Setup(d => d.DateService()).Returns(new DateServiceDaily());

            // Act
            var jobMessage = mainService.Execute();

            // Assert
            Assert.That(!expectedJobMessage.Mail!.Equals(jobMessage.Mail));
        }
    }
}