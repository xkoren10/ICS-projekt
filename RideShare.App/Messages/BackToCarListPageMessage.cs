using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record BackToCarListPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
