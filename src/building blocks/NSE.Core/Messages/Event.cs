using MediatR;
using System;

namespace NSE.Core.Messages
{
    public class Event : Message, INotification
    {
        public DateTime TImestamp { get; private set; }

        protected Event()
        {
            TImestamp = DateTime.Now;
        }
    }
}
