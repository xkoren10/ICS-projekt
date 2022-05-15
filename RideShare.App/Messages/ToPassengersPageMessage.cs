using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToPassengersPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
