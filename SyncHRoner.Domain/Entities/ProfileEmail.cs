using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.ValueObjects;

namespace SyncHRoner.Domain.Entities
{
    public class ProfileEmail : Entity
    {
        public Email Email { get; }
        //for ef
        protected ProfileEmail() { }

        public ProfileEmail(Email email)
        {
            Email = email;
        }
    }
}
