using Microsoft.Extensions.Logging;
using Moq;
using SyncHRoner.DAL.Repositories;
using SyncHRoner.DAL.Utils;
using SyncHRoner.Domain.Context;
using SyncHRoner.Domain.Enums;
using SyncHRoner.Domain.ValueObjects;
using SyncHRoner.Dtos.UpdateDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SyncHRoner.DAL.Tests.Integration
{
    public class ProfileRepositoryTests : IDisposable
    {
        private readonly SyncHRonerContext _context;
        private readonly ProfileRepository _repository;

        public ProfileRepositoryTests()
        {
            var mockRepositoryLogger = new Mock<ILogger<ProfileRepository>>();
            _context = new SyncHRonerContext();
            _context.SeedData();
            _repository = new ProfileRepository(_context, mockRepositoryLogger.Object);
        }

        [Fact]
        public async Task SaveProfile_WhenAllFieldsAreValid_ShouldReturnTrue()
        {
            _context.Profile.RemoveRange(_context.Profile);
            await _context.SaveChangesAsync();

            SyncHRoner.Domain.Entities.Profile newProfile =
                new Domain.Entities.Profile(FullName.Create("Konrad", "Czado").GetSuccessValue(),
                                            BirthDate.Create(new DateTime(1994, 7, 28)).GetSuccessValue(),
                                            GenderEnum.Male,
                                            CountryEnum.Poland,
                                            Phone.Create("000000000").GetSuccessValue(),
                                            Email.Create("czadokonrad@gmail.com").GetSuccessValue(),
                                            Rating.Create(4.5).GetSuccessValue());

            var result = await _repository.SaveAsync(newProfile, It.IsAny<CancellationToken>());

            bool isSuccesssfullySaved = result.Match(Left: (failure) => false,
                                               Right: (result) => result);

            Assert.True(isSuccesssfullySaved);
        }


        [Fact]
        public async Task UpdateProfile_WhenProfileExists_AndConstainsValidData_ShouldReturnTrue()
        {
            var profilesResult = await _repository.GetAllAsync(default);

            var profiles = profilesResult.GetSuccessValue()
                .Match(result => result, () => new List<SyncHRoner.Domain.Entities.Profile>()).ToList();

            var toUpdate = profiles.FirstOrDefault();

            Assert.NotNull(toUpdate);

            ProfileUpdateDto profileUpdateDto = new ProfileUpdateDto
            {
                Id = toUpdate.Id,
                BirthDate = new DateTime(1992, 2, 3),
                Phone = "010203045",
                Email = "konrad521@vp.pl",
                Rating = 3.5,
                Gender = GenderEnum.Unknown,
                FirstName = "Dante",
                LastName = "Inferno",
            };


            toUpdate.ChangeBirthDate(BirthDate.Create(profileUpdateDto.BirthDate).GetSuccessValue());
            toUpdate.ChangePhone(Phone.Create(profileUpdateDto.Phone).GetSuccessValue());
            toUpdate.ChangeEmail(Email.Create(profileUpdateDto.Email).GetSuccessValue());
            toUpdate.ChangeRating(Rating.Create(profileUpdateDto.Rating).GetSuccessValue());
            toUpdate.ChangeGender(profileUpdateDto.Gender);
            toUpdate.ChangeLastUpdate(profileUpdateDto.LastUpdate);
            var result = await _repository.UpdateAsync(toUpdate, It.IsAny<CancellationToken>());

            bool isSuccesssfullyUpdated = result.Match(Left: (failure) => false,
                                             Right: (result) => result);

            Assert.True(isSuccesssfullyUpdated);
        }

        [Fact]
        public async Task RemoveProfile_WhenProfileExistsAndSuccessfulyRemoved_ShouldReturnTrue()
        {
            var profilesResult = await _repository.GetAllAsync(default);

            var profiles = profilesResult.GetSuccessValue()
                .Match(result => result, () => new List<SyncHRoner.Domain.Entities.Profile>()).ToList();

            var toRemove = profiles.FirstOrDefault();

            Assert.NotNull(toRemove);

            var result = await _repository.DeleteAsync(toRemove.Id, It.IsAny<CancellationToken>());

            bool isSuccesssfullyRemoved = result.Match(Left: (failure) => false,
                                          Right: (result) => result);

            Assert.True(isSuccesssfullyRemoved);
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
