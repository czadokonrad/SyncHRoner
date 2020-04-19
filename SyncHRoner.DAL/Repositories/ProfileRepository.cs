using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Context;
using SyncHRoner.Domain.Entities;
using SyncHRoner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncHRoner.DAL.Repositories
{
    public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        private readonly ILogger<ProfileRepository> _logger;
        public ProfileRepository(SyncHRonerContext context, ILogger<ProfileRepository> logger)
            : base(context, logger)
        {
            _logger = logger;
        }

        public async override Task<Either<Failure, bool>> UpdateAsync(Profile entity, CancellationToken cancellationToken)
        {
            try
            {
                Profile profile = await GetByIdTrackedAsync(entity.Id, cancellationToken);

                profile.ChangeBirthDate(entity.BirthDate);
                profile.ChangeEmail(entity.Email);
                profile.ChangeGender((GenderEnum)entity.GenderId);
                profile.ChangePhone(entity.Phone);
                profile.ChangeRating(entity.Rating);
                profile.ChangeFullName(entity.FullName);
                profile.ChangeLastUpdate(entity.LastUpdate.Value);

                var removedNationalities = profile.NationalitiesLink
                    .Where(x => !entity.NationalitiesLink.Any(y => y.CountryId == x.CountryId));

                foreach (var item in removedNationalities)
                {
                    profile.RemoveNationality(item.CountryId);
                }

                foreach (var item in entity.NationalitiesLink)
                {
                    profile.AddNationality(item.CountryId);             
                }


                await Context.SaveChangesAsync(cancellationToken);
                return true;

            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException)
            {
                if (((SqlException)ex.InnerException).Number == 2601)
                    return new Failure($"Cannot insert duplicated data");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync for Profile");
                return new Failure("Unexpected error occured");
            }
        }
    }
}
