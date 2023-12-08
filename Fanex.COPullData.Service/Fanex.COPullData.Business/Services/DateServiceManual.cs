using System;

namespace Fanex.COPullData.Business.Services;

public class DateServiceManual : DateService
{
    public DateTime FromDate => AppConfigService.Config.FromDate;

    public DateTime ToDate => AppConfigService.Config.ToDate;
}