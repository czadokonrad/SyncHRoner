using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Domain.Entities
{
    public class ProfilePhone : Entity
    {
        public Phone Phone { get; }

        //for ef 
        protected ProfilePhone() { }

        public ProfilePhone(Phone phone)
        {
            Phone = phone;
        }
    }
}
