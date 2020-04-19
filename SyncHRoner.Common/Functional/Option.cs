using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Common.Functional
{

    using static FuncHelpers;
    public partial class FuncHelpers
    {
        public static Option.None None => Option.None.Default;
        public static Option<T> Some<T>(T value) => new Option.Some<T>(value);
    }

    public struct Option<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        internal Option(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
                throw new InvalidOperationException($"Tried to instantiate Option with default value of type: {typeof(T).Name}");
            _value = value;
            _hasValue = true;
        }

        public static implicit operator Option<T>(Option.None _) => new Option<T>();
        public static implicit operator Option<T>(Option.Some<T> value) => new Option<T>(value.Value);
        public static implicit operator Option<T>(T value) =>
            EqualityComparer<T>.Default.Equals(value, default) ? None : Some(value);

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        {
            if (_hasValue)
                return some(_value);
            return none();
        }

    }

    public static class OptionExtensions
    {
        public static Option<TResult> Map<T, TResult>(this Option.None _, Func<T, TResult> f)
            => None;
        public static Option<TResult> Map<T, TResult>(this Option.Some<T> some, Func<T, TResult> f)
            => Some(f(some.Value));
        public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> f)
           => option.Match(
                    some: (result) => Some(f(result)),
                    none: () => None);

        public static Option<TResult> Bind<T, TResult>(this Option<T> option, Func<T, Option<TResult>> f)
           => option.Match(
                    some: (result) => f(result),
                    none: () => None);

    }

    namespace Option
    {
        public struct None
        {
            internal static readonly None Default = new None();
        }

        public struct Some<T>
        {
            internal T Value { get; }
            internal Some(T value)
            {
                if (EqualityComparer<T>.Default.Equals(value, default))
                    throw new InvalidOperationException($"Tried to instantiate some with default value of type: {typeof(T).Name}");
                Value = value;
            }

        }
    }


}
