using Fanex.Data.Repository;

namespace Fanex.COPullData.Business.Criteria;

public class CoCommand : NonQueryCommand
{
    private readonly string db;

    public CoCommand(string db)
    {
        this.db = db;
    }

    public string Json { get; set; }

    public override string GetSettingKey()
    {
        return $"{db}_Task_JTCashoutTicket_Import";
    }

    public override bool IsValid()
    {
        return true;
    }
}