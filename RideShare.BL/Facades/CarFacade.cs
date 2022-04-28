using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RideShare.BL.Models;
using RideShare.DAL.Entities;
using RideShare.DAL.UnitOfWork;

namespace RideShare.BL.Facades
{
    public class CarFacade : CRUDFacade<CarEntity, CarListModel, CarDetailModel>
    {
        public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
        }

        public async Task CreateNewCar(UserDetailModel Owner, DateTime RegDate, string Brand, string Type, int Seats, string ImagePath = null)
        {
            var car = new CarDetailModel(
                Id: Guid.NewGuid(),
                RegDate: DateTime.Now,
                Brand: Brand,
                Type: Type,
                Seats: Seats,
                ImagePath: ImagePath,
                UserId: Owner.Id);

            //todo pridat do zoznamu aut usera
            await SaveAsync(car);
        }

        public async Task EditCar(CarDetailModel Car, int Seats, string Brand = null, string Type = null, string ImagePath = null)
        {
            Car.Seats = Seats;
            if (Brand != null)
            {
                Car.Brand = Brand;
            }
            if (Type != null)
            {
                Car.Type = Type;
            }
            if (ImagePath != null)
            {
                Car.ImagePath = ImagePath;
            }
            await SaveAsync(Car);
        }
    }
}
