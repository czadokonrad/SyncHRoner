using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SyncHRoner.Domain.Enums
{
    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Unknown = 3
    }
}
