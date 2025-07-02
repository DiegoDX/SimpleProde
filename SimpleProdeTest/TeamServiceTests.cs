using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using SimpleProde;
using SimpleProde.Controllers;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using SimpleProde.Services;
using SimpleProde.Utilities;
using System;

namespace SimpleProdeTest
{
    public class TeamServiceTests
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ApplicationDbContext context;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IStoreFiles storeFiles;
        private readonly PaginationDTO pagination;
        private readonly ITeamService service;

        public TeamServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(_dbContextOptions);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>(); // tu perfil de AutoMapper
            });
            
            _mapper = config.CreateMapper();

            var mockOutputCacheStore = new Mock<IOutputCacheStore>();
            mockOutputCacheStore
            .Setup(x => x.EvictByTagAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
            var mockStoreFiles = new Mock<IStoreFiles>();

            mockStoreFiles.Setup(x => x.Store(It.IsAny<string>(), It.IsAny<IFormFile>()))
    .ReturnsAsync("https://fakeurl.com/fakeimage.jpg");

            // Mock del método Delete
            mockStoreFiles.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);


            pagination = new PaginationDTO { Page = 1, RecordsPerPage = 10 };

            // Crear el servicio real con dependencias
            service = new TeamService(context, _mapper, mockOutputCacheStore.Object, mockStoreFiles.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTeams()
        {
            // Arrange
           // using var context = new ApplicationDbContext(_dbContextOptions);
            context.Teams.Add(new Team { Id = 1, Name = "Team A" });
            context.Teams.Add(new Team { Id = 2, Name = "Team B" });
            await context.SaveChangesAsync();

             //new TeamService(context, _mapper, outputCacheStore, storeFiles); // asumimos que tu servicio usa DbContext y AutoMapper

            // Act
            var result = await service.Get(pagination);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Name == "Team A");
        }

        [Fact]
        public async Task Create_ShouldAddTeamToDb()
        {
            var teamCreateDTO = new TeamCreateDTO { Name = "New Team" };
            //Act
            await service.Create(teamCreateDTO);

            // Assert
            var teamInDb = await context.Teams.FirstOrDefaultAsync();
            Assert.NotNull(teamInDb);
            Assert.Equal("New Team", teamInDb.Name);
        }

        [Fact]
        public async Task Update_ShouldModifyTeam()
        {
            var team = new Team { Id = 3, Name = "Old Name" };
            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var teamCreateDTO = new TeamCreateDTO { Name = "Updated Name" };

            // Act
            await service.Update(3, teamCreateDTO);

            // Assert
            var teamInDb = await context.Teams.FindAsync(3);
            Assert.NotNull(teamInDb);
            Assert.Equal("Updated Name", teamInDb.Name);
        }

        [Fact]
        public async Task Delete_ShouldRemoveTeamFromDb()
        {
            context.Teams.Add(new Team { Id = 10, Name = "ToDelete" });
            await context.SaveChangesAsync();

            var test = context.Teams.AsEnumerable();
            // Act
            await service.Delete(10);

            // Assert
            var teamInDb = await context.Teams.FindAsync(10);
            Assert.Null(teamInDb);
        }

        [Fact]
        public async Task Update_NonExistingTeam_ReturnsFalse()
        {
            //var context = new Mock<ApplicationDbContext>();
            //var service = new TeamService(context.Object, context);

            var dto = new TeamCreateDTO { Name = "Desconocido" };
            
            //Act
            var result = await service.Update(999, dto);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Get_ShouldReturnListOfTeams()
        {
            // Arrange
            var fakeService = new Mock<ITeamService>();
            fakeService.Setup(s => s.Get(It.IsAny<PaginationDTO>()))
                .ReturnsAsync(new List<TeamDTO> {
            new TeamDTO { Id = 1, Name = "The Strongest" },
            new TeamDTO { Id = 2, Name = "Always Ready" }
                });

            var controller = new TeamController(fakeService.Object);
            // Simular HttpContext para que no sea null
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await controller.Get(new PaginationDTO());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var teams = Assert.IsType<List<TeamDTO>>(okResult.Value);
            Assert.Equal(2, teams.Count);
        }
    }
}