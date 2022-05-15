using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record DeleteMessage<T> : Message<T>
        where T : IModel
    {
    }
}