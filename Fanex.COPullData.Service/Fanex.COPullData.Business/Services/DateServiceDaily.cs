using System;

namespace Fanex.COPullData.Business.Services;

public class DateServiceDaily : DateService
{
    public DateTime FromDate => DateTimeOffset.Now.DateTime.AddDays(-3);

    public DateTime ToDate => DateTimeOffset.Now.DateTime.AddDays(-2);
}