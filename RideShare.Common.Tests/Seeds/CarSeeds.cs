using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace RideShare.Common.Tests.Seeds;

public static class CarSeeds
{
    public static readonly CarEntity EmptyCar = new(
        Id: default,
        RegDate: default,
        Brand: default,
        Type: default,
        ImagePath: default,
        Seats: default,
        UserId: default)
    {
        User = default,
        Rides = default
    };
    
    public static readonly CarEntity Car1 = new(
        Id: Guid.Parse(input: "0d4fa150-ad80-4d46-a511-4c666166ec5e"),
        RegDate: DateTime.Parse("5/1/2008 8:30:52 AM",System.Globalization.CultureInfo.InvariantCulture),
        Brand: "Audi",
        Type: "A8 1.9 TDi",
        ImagePath: null,
        Seats: 5,
        UserId: UserSeeds.UserEntity1.Id)
    {
        Recipe = RidesSeeds.RecipeEntity,
        Ingredient = UserSeeds.UserEntity1
    };

    public static readonly CarEntity Car2 = new(
        Id: Guid.Parse(input: "87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        Amount: 2.0,
        Unit: Unit.L,
        RecipeId: RidesSeeds.RecipeEntity.Id,
        IngredientId: UserSeeds.IngredientEntity2.Id)
    {
        Recipe = RidesSeeds.RecipeEntity,
        Ingredient = UserSeeds.IngredientEntity2
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly CarEntity CarEntityUpdate = Car1 with { Id = Guid.Parse("A2E6849D-A158-4436-980C-7FC26B60C674"), Ingredient = null, Recipe = null, RecipeId = RidesSeeds.RecipeForIngredientAmountEntityUpdate.Id};
    public static readonly CarEntity CarEntityDelete = Car1 with { Id = Guid.Parse("30872EFF-CED4-4F2B-89DB-0EE83A74D279"), Ingredient = null, Recipe = null, RecipeId = RidesSeeds.RecipeForIngredientAmountEntityDelete.Id };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarEntity>().HasData(
            Car1 with { User = null, Rides = null },
            Car2 with { User = null, Rides = null },
            CarEntityUpdate,
            CarEntityDelete
        );
    }
}