using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SyncHRoner.Domain.ValueObjects
{
    public sealed class Phone : ValueObject
    {
        public string Value { get; }
        private Phone(string value)
        {
            Value = value;
        }

        //for ef
        protected Phone() { }

        public static Either<Failure, Phone> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new Failure($"Phone number must be provided");

            if(value.Length < 8 || value.Length > 11)
                return new Failure($"Phone number is invalid");

            value = value.Length == 9 ? "48" + value : value;

            if (!Regex.IsMatch(value, @"^(48)?\d{9}$"))
                return new Failure($"Phone number is invalid");


            return new Phone(value);
        }

        public static implicit operator string(Phone phone)
        {
            return phone.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
