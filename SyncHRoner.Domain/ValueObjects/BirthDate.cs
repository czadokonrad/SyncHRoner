using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Domain.ValueObjects
{
    public sealed class BirthDate : ValueObject
    {
        public DateTime Value { get; }

        private BirthDate(DateTime value)
        {
            Value = value;
        }

        //for ef
        protected BirthDate() { }


        public static Either<Failure, BirthDate> Create(DateTime value)
        {
            if (value == default || value == DateTime.MinValue || value == DateTime.MaxValue)
                return new Failure($"BirthDate must be provided");

            if (value > DateTime.Now)
                return new Failure($"BirthDate cannot be greater than todays date");

            return new BirthDate(value);


        }

        public static implicit operator DateTime(BirthDate birthDate)
        {
            return birthDate.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
