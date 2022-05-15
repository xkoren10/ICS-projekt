using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToNewCarPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
