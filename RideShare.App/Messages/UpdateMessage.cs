using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record UpdateMessage<T> : Message<T>
        where T : IModel
    {
    }
}
