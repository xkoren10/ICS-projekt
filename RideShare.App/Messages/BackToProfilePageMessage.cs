using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record BackToProfilePageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
