using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SyncHRoner.Domain.ValueObjects
{
    public sealed class FullName : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }
        private FullName(string firstName, string lastName) 
        {
            FirstName = firstName;
            LastName = lastName;
        }

        //for ef
        protected FullName() { }

        public static Either<Failure, FullName> Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return new Failure($"FirstName cannot be null or empty, provided: {firstName}");
            if (string.IsNullOrWhiteSpace(lastName))
                return new Failure($"LastName cannot be null or empty, provided: {lastName}");

            if (firstName.Length < 2 || firstName.Length > 50)
                return new Failure($"FirstName cannot be shorter than 2 characters and cannot be longer than 50 characters");

            if (lastName.Length < 2 || lastName.Length > 50)
                return new Failure($"LastName cannot be shorter than 2 characters and cannot be longer than 50 characters");

            if (!Regex.IsMatch(firstName, @"^[a-zA-Z]{2,50}$"))
            {
                return new Failure($"FirstName is invalid: {firstName}");
            }

            if (!Regex.IsMatch(lastName, @"^[a-zA-Z]{2,50}$"))
            {
                return new Failure($"LastName is invalid, provided: {lastName}");
            }

            return new FullName(firstName, lastName);
        }

        public static implicit operator string(FullName fullName)
        {
            return $"{fullName.FirstName} {fullName.LastName}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
