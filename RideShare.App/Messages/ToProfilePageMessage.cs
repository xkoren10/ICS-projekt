using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToProfilePageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
