using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncHRoner.Domain.Entities
{
    public class Country : Entity, IEnumeration
    {
        public static Country[] Countries => Enum.GetValues(typeof(CountryEnum))
                                            .Cast<CountryEnum>()
                                            .ToList()
                                            .Select((x, index) => new Country(index + 1, x.ToString()))
                                            .ToArray();
        private Country(long id, string name) : base(id)
        {
            Name = name;
        }

        //for ef
        protected Country() { }

        private List<ProfileNationality> _profilesLink = new List<ProfileNationality>();
        public string Name { get; }
        public IReadOnlyList<ProfileNationality> ProfilesLink => _profilesLink.ToList();

        public static implicit operator Country(CountryEnum countryEnum)
        {
            return new Country((long)countryEnum, countryEnum.ToString());
        }

        public static implicit operator CountryEnum(Country country)
        {
            return (CountryEnum)Enum.Parse(typeof(CountryEnum), country.Name);
        }
    }
}
