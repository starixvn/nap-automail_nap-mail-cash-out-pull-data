using System;

namespace Fanex.COPullData.Business.Services;

public interface DateService
{
    DateTime FromDate { get; }

    DateTime ToDate { get; }
}