using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record BackToMainPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
