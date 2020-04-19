using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.Enums;
using SyncHRoner.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncHRoner.Domain.Entities
{
    //aggregate root
    public class Profile : Entity
    {
        //for ef core
        protected Profile() { }

        private List<ProfileEmail> _profileEmails = new List<ProfileEmail>();
        private List<ProfilePhone> _profilePhones = new List<ProfilePhone>();
        private List<ProfileNationality> _nationalitiesLink = new List<ProfileNationality>();

        public FullName FullName { get; private set; }
        public BirthDate BirthDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? LastUpdate { get; private set; }
        public long GenderId { get; private set; }
        private Phone _phone;
        public Phone Phone
        {
            get => _phone;
            private set
            {
                if (!_profilePhones.Any(pp => pp.Phone == value))
                    AddProfilePhone(new ProfilePhone(value));
                _phone = value;
            }
        }
        private Email _email;
        public Email Email
        {
            get => _email;
            private set
            {
                if (!_profileEmails.Any(pe => pe.Email == value))
                    AddProfileEmail(new ProfileEmail(value));
                _email = value;
            }
        }
        public Rating Rating { get; private set; }
        public bool IsActive { get; private set; }
        public IReadOnlyList<ProfileEmail> ProfileEmails => _profileEmails.ToList();
        public IReadOnlyList<ProfilePhone> ProfilePhones => _profilePhones.ToList();
        public IReadOnlyList<ProfileNationality> NationalitiesLink => _nationalitiesLink.ToList();

        public Profile(FullName fullName, BirthDate birthDate, Gender gender, CountryEnum country, Phone phone, Email email, Rating rating)
        {
            FullName = fullName;
            BirthDate = birthDate;
            GenderId = gender.Id;
            Phone = phone;
            Email = email;
            Rating = rating;
            AddNationality((long)country);

            CreationDate = DateTime.UtcNow;
            IsActive = true;
        }

        public void ChangeLastUpdate(DateTime lastUpdate)
        {
            LastUpdate = lastUpdate;
        }

        public void ChangeFullName(FullName fullName)
        {
            FullName = fullName;
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }

        public void ChangePhone(Phone phone)
        {
            Phone = phone;
        }

        public void ChangeBirthDate(BirthDate birthDate)
        {
            BirthDate = birthDate;
        }

        public void ChangeRating(Rating rating)
        {
            Rating = rating;
        }

        public void ChangeGender(GenderEnum genderEnum)
        {
            GenderId = (long)genderEnum;
        }

        public void AddProfileEmail(ProfileEmail profileEmail)
        {
            if (!_profileEmails.Any(pe => pe.Email == profileEmail.Email))
                _profileEmails.Add(profileEmail);
        }

        public void RemoveProfileEmail(ProfileEmail profileEmail)
        {
            ProfileEmail toRemove = _profileEmails.FirstOrDefault(x => x.Id == profileEmail.Id);

            if (toRemove != null)
                _profileEmails.Remove(toRemove);
        }


        public void AddProfilePhone(ProfilePhone profilePhone)
        {
            if (!_profilePhones.Any(pp => pp.Phone == profilePhone.Phone))
                _profilePhones.Add(profilePhone);
        }

        public void RemoveProfilePhone(ProfilePhone profilePhone)
        {
            ProfilePhone toRemove = _profilePhones.FirstOrDefault(x => x.Id == profilePhone.Id);

            if (toRemove != null)
                _profilePhones.Remove(toRemove);
        }

        public void AddNationality(long countryId)
        {
            if (!_nationalitiesLink.Any(c => c.CountryId == countryId))
                _nationalitiesLink.Add(new ProfileNationality(this.Id, countryId));
        }

        public void RemoveNationality(long countryId)
        {
            ProfileNationality toRemove = _nationalitiesLink.FirstOrDefault(c => c.CountryId == countryId);

            if (toRemove != null)
                _nationalitiesLink.Remove(toRemove);
        }

        public void ClearNationalities()
        {
            _nationalitiesLink.Clear();
        }
    }
}
