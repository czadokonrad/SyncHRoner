using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Domain.Base
{
    public abstract class Entity
    {
        public long Id { get; private set; }


        protected Entity() { }

        protected Entity(long id) : this()
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity other))
                return false;

            if (this.GetRealType() != other.GetType())
                return false;

            if (Id == 0 || other.Id == 0)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.Id == other.Id;
        }


        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        /// <summary>
        /// Get real type, because EF Core is wrapping entity into proxy object
        /// </summary>
        /// <returns></returns>
        private Type GetRealType()
        {
            Type t = GetType();

            if (t.ToString().Contains("Castle.Proxies."))
                return t.BaseType;

            return t;
        }
    }
}
