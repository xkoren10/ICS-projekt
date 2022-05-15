using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToCarDetailPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
