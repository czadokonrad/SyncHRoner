using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SyncHRoner.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        //for ef
        protected Email() { }

        public static Either<Failure, Email> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new Failure("Email should not be empty");

            EmailAddressAttribute emailValidator = new EmailAddressAttribute();

            if (!emailValidator.IsValid(value))
                return new Failure($"Passed email: {value} is incorrect");

            return new Email(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }
    }
}
