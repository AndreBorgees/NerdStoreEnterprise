using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Application.Events
{
    public class CustomerEventHandler : INotificationHandler<RegisteredCustomerEvent>
    {
        public Task Handle(RegisteredCustomerEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
