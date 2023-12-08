using System;

namespace Fanex.COPullData.Business.Services;

public class DateServiceDaily : DateService
{
    public DateTime FromDate => DateTime.Today.AddDays(-3);

    public DateTime ToDate => DateTime.Today.AddDays(-2);
}