using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SyncHRoner.Automapper;
using SyncHRoner.Common.Functional;
using SyncHRoner.Controllers;
using SyncHRoner.DAL.Repositories;
using SyncHRoner.Domain.Enums;
using SyncHRoner.Domain.ValueObjects;
using SyncHRoner.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SyncHRoner.Tests.Unit
{
    public class ProfileControllerTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly Mock<IMapper> _mockAutoMapper;

        public ProfileControllerTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();
            _mockAutoMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllProfiles_WhenProfilesExistsInDb_ShouldReturnStatusCodeOkWithAllProfiles()
        {
            List<Domain.Entities.Profile> profiles = new List<Domain.Entities.Profile>
            {
                new Domain.Entities.Profile(FullName.Create("Konrad", "Czado").GetSuccessValue(),
                                            BirthDate.Create(new DateTime(1994, 07, 28)).GetSuccessValue(),
                                            GenderEnum.Male,
                                            CountryEnum.Poland,
                                            Phone.Create("48000000000").GetSuccessValue(),
                                            Email.Create("czadokonrad@gmail.com").GetSuccessValue(),
                                            Rating.Create(4.5).GetSuccessValue()),
            };
            var right = FuncHelpers.Right<Option<IEnumerable<Domain.Entities.Profile>>>(profiles);



            _mockAutoMapper.Setup(x => x.Map<ProfileDto>(It.IsAny<Domain.Entities.Profile>()))
                .Returns((Domain.Entities.Profile profile) => new ProfileDto
                {
                    BirthDate = profile.BirthDate.Value,
                    Email = profile.Email.Value,
                    FirstName = profile.FullName.FirstName,
                    Gender = (GenderEnum)profile.GenderId,
                    Id = profile.Id,
                    LastName = profile.FullName.LastName,
                    Nationalities = profile.NationalitiesLink.Select(x => (CountryEnum)x.CountryId).ToList(),
                    Phone = profile.Phone.Value,
                    Rating = profile.Rating.Value
                });

            var mappedProfiles = profiles.Select(x => _mockAutoMapper.Object.Map<ProfileDto>(x));

            ProfileController controller = new ProfileController(_mockProfileRepository.Object, _mockAutoMapper.Object);

            _mockProfileRepository.Setup(x => x
            .GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(right);

            IActionResult result = await controller.GetAllProfiles(default);


            NUnit.Framework.Assert.IsAssignableFrom<OkObjectResult>(result);
            var m = ((OkObjectResult)result).Value;
            var res = (((OkObjectResult)result).Value as List<ProfileDto>);

            NUnit.Framework.Assert.NotNull(res);
            NUnit.Framework.Assert.AreEqual(profiles.Count, res.Count);

        }

        [Fact]
        public async Task GetAllProfiles_WhenNoProfileFound_ShouldReturnEmptySet()
        {
            ProfileController controller = new ProfileController(_mockProfileRepository.Object, _mockAutoMapper.Object);

            var right = FuncHelpers.Right<Option<IEnumerable<Domain.Entities.Profile>>>(null);

            _mockProfileRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(right);


            IActionResult result = await controller.GetAllProfiles(default);


            NUnit.Framework.Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllProfiles_WhenThrowsAnException_ShouldReturnStatusCode500()
        {
            ProfileController controller = new ProfileController(_mockProfileRepository.Object, _mockAutoMapper.Object);

            var left = FuncHelpers.Left<Failure>(new Failure("Test exception"));

            _mockProfileRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(left);


            IActionResult result = await controller.GetAllProfiles(default);


            NUnit.Framework.Assert.IsAssignableFrom<StatusCodeResult>(result);
            NUnit.Framework.Assert.AreEqual(((StatusCodeResult)result).StatusCode, StatusCodes.Status500InternalServerError);

        }
    }
}
