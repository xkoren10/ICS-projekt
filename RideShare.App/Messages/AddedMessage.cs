using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record AddedMessage<T> : Message<T>
        where T : IModel
    {
    }
}