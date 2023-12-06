using System.Collections.Generic;

namespace Fanex.COPullData.Business.Services;

public interface DateServiceProvider
{
    DateService DateService();
}

public class DateServiceProviderImp : DateServiceProvider
{
    private static Dictionary<bool, DateService> Date
    {
        get
        {
            var result = new Dictionary<bool, DateService>
            {
                { true, new DateServiceManual() },
                { false, new DateServiceDaily() }
            };

            return result;
        }
    }

    public DateService DateService()
    {
        return Date[AppConfigService.Config.IsManual];
    }
}