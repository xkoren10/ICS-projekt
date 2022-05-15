using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record BackToLogPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
