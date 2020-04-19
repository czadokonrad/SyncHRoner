using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SyncHRoner.DAL.Repositories;
using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.Context;
using SyncHRoner.Domain.Entities;
using SyncHRoner.Domain.Enums;
using SyncHRoner.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SyncHRoner.DAL.Utils
{
    public static class DbContextExtensions
    {

        public static void SeedData(this SyncHRonerContext dbContext)
        {
            if (!dbContext.Country.Any())
            {
                dbContext.Country.AddRange(Country.Countries);
            }

            if (!dbContext.Gender.Any())
            {
                dbContext.Gender.AddRange(Gender.Genders);
            }

            dbContext.SaveChanges();


            if (!dbContext.Profile.Any())
            {
                var repo = new ProfileRepository(dbContext, default);

                List<Profile> profiles = new List<Profile>
            {
                new Profile(FullName.Create("Konrad", "Czado").GetSuccessValue(),
                            BirthDate.Create(new DateTime(1994, 7, 28)).GetSuccessValue(),
                            GenderEnum.Male,
                            CountryEnum.Poland,
                            Phone.Create("000000000").GetSuccessValue(),
                            Email.Create("czadokonrad@gmail.com").GetSuccessValue(),
                            Rating.Create(4).GetSuccessValue()),
                new Profile(FullName.Create("Miron", "Buszta").GetSuccessValue(),
                            BirthDate.Create(new DateTime(2000, 2, 18)).GetSuccessValue(),
                            GenderEnum.Male,
                            CountryEnum.Poland,
                            Phone.Create("000000012").GetSuccessValue(),
                            Email.Create("miron@gmail.com").GetSuccessValue(),
                            Rating.Create(1).GetSuccessValue()),
                new Profile(FullName.Create("Agnieszka", "Czado").GetSuccessValue(),
                            BirthDate.Create(new DateTime(1930, 12, 28)).GetSuccessValue(),
                            GenderEnum.Female,
                            CountryEnum.Germany,
                            Phone.Create("034000000").GetSuccessValue(),
                            Email.Create("bagnieszka@gmail.com").GetSuccessValue(),
                            Rating.Create(0).GetSuccessValue()),
                new Profile(FullName.Create("Marcin", "Bojda").GetSuccessValue(),
                            BirthDate.Create(new DateTime(1995, 7, 28)).GetSuccessValue(),
                            GenderEnum.Male,
                            CountryEnum.Poland,
                            Phone.Create("999999999").GetSuccessValue(),
                            Email.Create("bojda@gmail.com").GetSuccessValue(),
                            Rating.Create(2).GetSuccessValue()),
                new Profile(FullName.Create("Arkadiusz", "Milik").GetSuccessValue(),
                            BirthDate.Create(new DateTime(1994, 7, 28)).GetSuccessValue(),
                            GenderEnum.Male,
                            CountryEnum.Poland,
                            Phone.Create("005450000").GetSuccessValue(),
                            Email.Create("milik@gmail.com").GetSuccessValue(),
                            Rating.Create(1.5).GetSuccessValue())


            };
                

                foreach(var profile in profiles)
                {
                    repo.SaveAsync(profile, default).Wait();
                }
            }

        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }
        /// <summary>
        /// Idea for this code I took from stackoverflow => 
        /// https://stackoverflow.com/questions/49593482/entity-framework-core-2-0-1-eager-loading-on-all-nested-related-entities
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetIncludePaths(this DbContext context, Type clrEntityType)
        {
            var entityType = context.Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();

            while (true)
            {
                var entityNavigations = new List<INavigation>();
                foreach (var navigation in entityType.GetNavigations().
                        Where(x =>
                        !clrEntityType.GetType().GetProperties().Where(x => x.GetType().BaseType != typeof(ValueObject)).Select(x => x.Name).Any(y =>
                            y == x.Name)))
                {
                    if (includedNavigations.Add(navigation))
                        entityNavigations.Add(navigation);
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.FindInverse();
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();
                if (stack.Count == 0) break;
                entityType = stack.Peek().Current.GetTargetType();
            }
        }

        private static DbContext GetDbContextFromDbSet<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
        {

            FieldInfo contextField =
                dbSet.GetType().GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance);

            return (DbContext)contextField.GetValue(dbSet);
        }
    }
}
