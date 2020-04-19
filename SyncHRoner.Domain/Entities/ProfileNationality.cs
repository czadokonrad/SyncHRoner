using SyncHRoner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Domain.Entities
{
    public class ProfileNationality
    {
        public long ProfileId { get; }

        public long CountryId { get; }

        //for ef
        protected ProfileNationality() { }

        public ProfileNationality(long profileId, long countryId)
        {
            ProfileId = profileId;
            CountryId = countryId;
        }
    }
}
