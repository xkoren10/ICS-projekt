using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToMyRidesPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
