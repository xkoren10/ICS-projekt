using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record OpenMessage<T> : Message<T>
        where T : IModel
    {
    }


}
