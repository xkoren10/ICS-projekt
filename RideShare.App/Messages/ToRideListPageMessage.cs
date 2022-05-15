using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToRideListPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
