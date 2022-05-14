using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace RideShare.DAL.Seeds
{

    public static class UserSeeds
    {
        public static readonly UserEntity EmptyUser = new(
            Id: default,
            Name: default,
            Surname: default,
            ImagePath: default,
            Contact: default);

        public static readonly UserEntity Driver = new(
            Id: Guid.Parse(input: "06a8a2cf-ea03-4095-a3e4-aa0291fe9c75"),
            Name: "Matej",
            Surname: "Hložek",
            Contact: "xhloze02@studfit.vutbr.cz",
            ImagePath: default);

        //To ensure that no tests reuse these clones for non-idempotent operations
        public static readonly UserEntity DriverUpdate = Driver with { Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052") };
        public static readonly UserEntity DriverDelete = Driver with { Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619") };

        public static UserEntity UserEntity1 = new(
            Id: Guid.Parse(input: "df935095-8709-4040-a2bb-b6f97cb416dc"),
            Name: "Marek",
            Surname: "Križan",
            Contact: "xkriza08@studfit.vutbr.cz",
            ImagePath: "https://media-exp1.licdn.com/dms/image/C4E03AQHcpLbkyPZqdA/profile-displayphoto-shrink_200_200/0/1650705609585?e=1656547200&v=beta&t=PeDqePVKsJ4fkARifRxzZNt9-X_5P8NpCwKqMlYJxgQ");

        public static UserEntity UserEntity2 = new(
            Id: Guid.Parse(input: "23b3902d-7d4f-4213-9cf0-112348f56238"),
            Name: "Lukasz",
            Surname: "Pycz",
            Contact: "xpyczl00@studfit.vutbr.cz",
            ImagePath: "https://api.sportnet.online/v1/users/5d6578c786dc8b72382fa135/photo/6e2dbc7f-f447-4712-8567-62815a6fba5b");

        public static UserEntity UserEntity3 = new(
            Id: Guid.Parse(input: "23b3902d-7d4f-1234-9cf0-112348f56238"),
            Name: "Michal",
            Surname: "Hroššo",
            Contact: "xhrosso00@studfit.vutbr.cz",
            ImagePath: default);



        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(
                UserEntity1,
                UserEntity2,
                Driver,
                DriverUpdate,
                DriverDelete);
        }
    }
}