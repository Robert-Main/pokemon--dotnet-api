using api.Data;
using api.Dtos;
using api.Mapping;
using api.models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Tests;

public class PokemonProfileTests
{
    private readonly IMapper _mapper;

    public PokemonProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PokemonProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        // This test catches missing maps, typos, and circular references at startup
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PokemonProfile>();
        });
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_Pokemon_WithAllNavigationProperties_ToPokemonResponseDtos()
    {
        // Arrange - build a full Pokemon entity with all navigation properties populated
        var pokemon = new Pokemon
        {
            Id = 1,
            Name = "Pikachu",
            BirthDate = new DateTime(2023, 1, 1),
            CreatedAt = DateTime.UtcNow,
            Reviews = new List<api.models.Review>
            {
                new api.models.Review
                {
                    Id = 1,
                    Title = "Great",
                    Text = "Electric type is awesome",
                    Rating = 5,
                    ReviewerId = 1,
                    PokemonId = 1,
                    Reviewer = new api.models.Reviewer
                    {
                        Id = 1,
                        FirstName = "Teddy",
                        LastName = "Smith"
                    }
                }
            },
            PokemonCategories = new List<PokemonCategory>
            {
                new PokemonCategory
                {
                    PokemonId = 1,
                    CategoryId = 1,
                    Category = new api.models.Category
                    {
                        Id = 1,
                        Name = "Electric"
                    }
                }
            },
            PokemonOwners = new List<PokemonOwner>
            {
                new PokemonOwner
                {
                    PokemonId = 1,
                    OwnerId = 1,
                    Owner = new api.models.Owner
                    {
                        Id = 1,
                        FirstName = "Jack",
                        LastName = "London",
                        Gym = "Brocks Gym"
                    }
                }
            }
        };

        // Act
        var result = _mapper.Map<PokemonResponseDtos>(pokemon);

        // Assert - basic Pokemon fields
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Pikachu", result.Name);

        // Assert - Reviews mapped with nested Reviewer
        Assert.NotNull(result.Reviews);
        Assert.Single(result.Reviews);
        Assert.Equal("Great", result.Reviews![0].Title);
        Assert.Equal(5, result.Reviews[0].Rating);
        Assert.NotNull(result.Reviews[0].Reviewer);
        Assert.Equal("Teddy", result.Reviews[0].Reviewer!.FirstName);

        // Assert - PokemonCategories mapped with nested Category
        Assert.NotNull(result.PokemonCategories);
        Assert.Single(result.PokemonCategories);
        Assert.Equal(1, result.PokemonCategories![0].CategoryId);
        Assert.NotNull(result.PokemonCategories[0].Category);
        Assert.Equal("Electric", result.PokemonCategories[0].Category!.Name);

        // Assert - PokemonOwners mapped with nested Owner
        Assert.NotNull(result.PokemonOwners);
        Assert.Single(result.PokemonOwners);
        Assert.Equal(1, result.PokemonOwners![0].OwnerId);
        Assert.NotNull(result.PokemonOwners[0].Owner);
        Assert.Equal("Jack", result.PokemonOwners[0].Owner!.FirstName);
        Assert.Equal("London", result.PokemonOwners[0].Owner.LastName);
        Assert.Equal("Brocks Gym", result.PokemonOwners[0].Owner.Gym);
    }

    [Fact]
    public void Map_Pokemon_WithNullCollections_ToPokemonResponseDtos()
    {
        // Arrange - Pokemon with no navigation properties
        var pokemon = new Pokemon
        {
            Id = 2,
            Name = "Bulbasaur",
            BirthDate = new DateTime(2023, 6, 1),
            Reviews = null,
            PokemonCategories = null,
            PokemonOwners = null
        };

        // Act
        var result = _mapper.Map<PokemonResponseDtos>(pokemon);

        // Assert - should not throw, collections should be empty (AutoMapper initializes empty lists)
        Assert.NotNull(result);
        Assert.Equal("Bulbasaur", result.Name);
        Assert.NotNull(result.Reviews);
        Assert.Empty(result.Reviews!);
        Assert.NotNull(result.PokemonCategories);
        Assert.Empty(result.PokemonCategories!);
        Assert.NotNull(result.PokemonOwners);
        Assert.Empty(result.PokemonOwners!);
    }

    [Fact]
    public void Map_PokemonCategory_ToPokemonCategoryResponseDtos()
    {
        // Arrange
        var pokemonCategory = new PokemonCategory
        {
            PokemonId = 1,
            CategoryId = 2,
            Category = new api.models.Category
            {
                Id = 2,
                Name = "Water"
            }
        };

        // Act
        var result = _mapper.Map<PokemonCategoryResponseDtos>(pokemonCategory);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.CategoryId);
        Assert.NotNull(result.Category);
        Assert.Equal("Water", result.Category!.Name);
    }

    [Fact]
    public void Map_PokemonOwner_ToPokemonOwnerResponseDtos()
    {
        // Arrange
        var pokemonOwner = new PokemonOwner
        {
            PokemonId = 1,
            OwnerId = 3,
            Owner = new api.models.Owner
            {
                Id = 3,
                FirstName = "Alice",
                LastName = "Wonder",
                Gym = "Saffron Gym"
            }
        };

        // Act
        var result = _mapper.Map<PokemonOwnerResponseDtos>(pokemonOwner);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.OwnerId);
        Assert.NotNull(result.Owner);
        Assert.Equal("Alice", result.Owner!.FirstName);
        Assert.Equal("Saffron Gym", result.Owner.Gym);
    }

    [Fact]
    public void Map_Review_ToReviewResponseDtos()
    {
        // Arrange
        var review = new api.models.Review
        {
            Id = 1,
            Title = "Amazing",
            Text = "Best pokemon ever",
            Rating = 5,
            ReviewerId = 1,
            PokemonId = 1,
            Reviewer = new api.models.Reviewer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe"
            }
        };

        // Act
        var result = _mapper.Map<ReviewResponseDtos>(review);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Amazing", result.Title);
        Assert.Equal(5, result.Rating);
        Assert.NotNull(result.Reviewer);
        Assert.Equal("John", result.Reviewer!.FirstName);
    }

    [Fact]
    public void Map_PokemonCreateDtos_ToPokemon_IgnoresIdAndCreatedAt()
    {
        // Arrange
        var createDto = new PokemonCreateDtos
        {
            Name = "Charizard",
            BirthDate = new DateTime(2023, 3, 15)
        };

        // Act
        var result = _mapper.Map<Pokemon>(createDto);

        // Assert
        Assert.Equal("Charizard", result.Name);
        Assert.Equal(new DateTime(2023, 3, 15), result.BirthDate);
        Assert.Equal(0, result.Id); // Default - ignored by mapping
        Assert.Null(result.CreatedAt); // Default - ignored by mapping
    }
}
