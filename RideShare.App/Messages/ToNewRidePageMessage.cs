using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToNewRidePageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
