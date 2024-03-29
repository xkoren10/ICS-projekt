﻿using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace RideShare.Common.Tests.Seeds
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
            ImagePath: null);

        //To ensure that no tests reuse these clones for non-idempotent operations
        public static readonly UserEntity DriverUpdate = Driver with { Id = Guid.Parse("143332B9-080E-4953-AEA5-BEF64679B052") };
        public static readonly UserEntity DriverDelete = Driver with { Id = Guid.Parse("274D0CC9-A948-4818-AADB-A8B4C0506619") };

        public static UserEntity UserEntity1 = new(
            Id: Guid.Parse(input: "df935095-8709-4040-a2bb-b6f97cb416dc"),
            Name: "Marek",
            Surname: "Križan",
            Contact: "xkriza08@studfit.vutbr.cz",
            ImagePath: null);

        public static UserEntity UserEntity2 = new(
            Id: Guid.Parse(input: "23b3902d-7d4f-4213-9cf0-112348f56238"),
            Name: "Lukasz",
            Surname: "Pycz",
            Contact: "xpyczl00@studfit.vutbr.cz",
            ImagePath: "https://www.google.com/search?q=lukasz+pycz&client=firefox-b-d&sxsrf=ALiCzsaRFZClb4yxv3tj_DADfIkGxE7jbg:1652520799768&source=lnms&tbm=isch&sa=X&ved=2ahUKEwjP8YPa1973AhVdwAIHHdKJCMMQ_AUoAXoECAEQAw&biw=1536&bih=778&dpr=1.25#imgrc=mWLKJimJxWa-qM");

        public static UserEntity UserEntity3 = new(
            Id: Guid.Parse(input: "23b3902d-7d4f-1234-9cf0-112348f56238"),
            Name: "Michal",
            Surname: "Hroššo",
            Contact: "xhrosso00@studfit.vutbr.cz",
            ImagePath: null);



        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(
                UserEntity1,
                UserEntity2,
                UserEntity3,
                Driver,
                DriverUpdate,
                DriverDelete);
        }
    }
}