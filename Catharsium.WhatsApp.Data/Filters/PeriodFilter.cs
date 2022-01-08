using Catharsium.Util.Filters;
using Catharsium.WhatsApp.Terminal.Models;
namespace Catharsium.WhatsApp.Data.Filters;

public class PeriodFilter : IFilter<Message>
{
    private readonly Period period;


    public PeriodFilter(Period period)
    {
        this.period = period;
    }


    public bool Includes(Message item)
    {
        return item.Timestamp.Date >= this.period.From.Date &&
               item.Timestamp.Date <= this.period.To.Date;
    }
}