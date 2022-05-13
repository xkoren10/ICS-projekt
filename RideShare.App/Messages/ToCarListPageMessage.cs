using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToCarListPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
