using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record NewMessage<T> : Message<T>
        where T : IModel
    {
    }
}
