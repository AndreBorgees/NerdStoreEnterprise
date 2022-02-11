using NSE.Core.Messages;
using System.Threading.Tasks;
using FluentValidation.Results;


namespace NSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T eventData) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T: Command;
    }
}
