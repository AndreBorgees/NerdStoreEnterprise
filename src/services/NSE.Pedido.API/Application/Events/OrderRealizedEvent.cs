using NSE.Core.Messages;
using System;

namespace NSE.Pedido.API.Application.Events
{
    public class OrderRealizedEvent : Event
    {
        public Guid PeriodId { get; private set; }
        public Guid ClinetId { get; private set; }

        public OrderRealizedEvent(Guid periodId, Guid clinetId)
        {
            PeriodId = periodId;
            ClinetId = clinetId;
        }
    }
}
