using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncHRoner.Domain.Entities
{
    public class Gender : Entity, IEnumeration
    {
        public static Gender[] Genders => Enum.GetValues(typeof(GenderEnum))
                                            .Cast<GenderEnum>()
                                            .ToList()
                                            .Select((x, index) => new Gender(index + 1, x.ToString()))
                                            .ToArray();
        public string Name { get; }

        private Gender(long id, string name) : base(id)
        {
            Name = name;
        }

        //for ef core
        protected Gender() { }

        public static implicit operator Gender(GenderEnum genderEnum)
        {
            return new Gender((long)genderEnum, genderEnum.ToString());
        }

        public static implicit operator GenderEnum(Gender gender)
        {
            return (GenderEnum)Enum.Parse(typeof(GenderEnum), gender.Name);
        }
    }
}
