using System.Threading.Tasks;

using MassTransit;

using MassTransitDemo.Contract;

namespace MassTransitDemo.TrafficLightServer.Stateless.Handlers
{
    public static class HandlerExtensions
    {
        public static Task RespondResultAsync<T>(this ConsumeContext<T> ctx, bool fail, string message)
            where T : class, ICommand
            => ctx.RespondAsync<ICommandValidationResult>(new
            {
                ctx.Message.CommandId,
                ctx.Message.CorrelationId,
                Fail = fail,
                Message = message
            });
    }
}