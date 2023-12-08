using Fanex.COPullData.Business.Services;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Fanex.COPullData.Service.Services;

public class ExecuteService : JobBase
{
    protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
    {
        var services = ServiceCollectionExtensions.Services;
        var serviceProvider = services.BuildServiceProvider();

        var mainService = serviceProvider.GetService<MainService>();
        var result = mainService!.Execute();
        return new JobMessage
        {
            JobStatus = result.JobStatus,
            Schedule = schedule,
            Mail = result.Mail
        };
    }
}