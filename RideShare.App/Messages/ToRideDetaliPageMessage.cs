using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToRideDetailPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
