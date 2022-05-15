using RideShare.BL.Models;

namespace RideShare.App.Messages
{
    public record SelectedMessage<T> : Message<T>
        where T : IModel
    {
    }
}
