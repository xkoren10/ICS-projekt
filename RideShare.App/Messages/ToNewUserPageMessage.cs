using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record ToNewUserPageMessage<T> : Message<T>
        where T : IModel
    {
    }


}
