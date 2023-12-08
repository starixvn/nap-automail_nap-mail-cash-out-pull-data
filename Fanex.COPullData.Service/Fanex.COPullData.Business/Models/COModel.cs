using System;

namespace Fanex.COPullData.Business.Models;

public class COModel
{
    public string bet_id { get; set; }

    public DateTime cashout_time { get; set; }

    public DateTime result_time { get; set; }

    public decimal stake { get; set; }

    public decimal original_payout { get; set; }

    public decimal jt_profit { get; set; }

    public string currency_code { get; set; }

    public decimal odds { get; set; }

    public decimal total_cashout { get; set; }

    public decimal exchange_rate { get; set; }
}