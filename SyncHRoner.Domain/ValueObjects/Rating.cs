using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Domain.ValueObjects
{
    public class Rating : ValueObject
    {
        private static double _maxRating = 5;
        public double Value { get; }

        private Rating(double value)
        {
            Value = value;
        }

        public static Either<Failure, Rating> Create(double value)
        {
            if (value < 0)
                return new Failure($"Rating cannot be negative");

            if (value > _maxRating)
                return new Failure($"Rating cannot be greater than max available rating: {_maxRating}");

            return new Rating(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
