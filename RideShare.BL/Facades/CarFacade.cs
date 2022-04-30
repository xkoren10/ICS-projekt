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
        private readonly UserFacade _userFacadeSUT;
        public CarFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
        {
            _userFacadeSUT = new UserFacade(unitOfWorkFactory, mapper);
        }

        //Creates new car and adds it to list of user's cars
        //Returns new car's id
        public async Task<Guid> CreateCar(UserDetailModel owner, DateTime regDate, string brand, string type, int seats, string? imagePath = null)
        {
            var newCar = new CarDetailModel(
                Id: Guid.NewGuid(),
                RegDate: regDate,
                Brand: brand,
                Type: type,
                Seats: seats,
                ImagePath: imagePath,
                UserId: owner.Id);

            await SaveAsync(newCar);
            owner.Cars.Add(newCar);
            await _userFacadeSUT.SaveAsync(owner);
            return newCar.Id;
        }

        public async Task EditCar(CarDetailModel car, int seats, string? brand = null, string? type = null, string? imagePath = null)
        {
            car.Seats = seats;
            if (brand != null)
            {
                car.Brand = brand;
            }
            if (type != null)
            {
                car.Type = type;
            }
            if (imagePath != null)
            {
                car.ImagePath = imagePath;
            }
            await SaveAsync(car);
        }
    }
}
