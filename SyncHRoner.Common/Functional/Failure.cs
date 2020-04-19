using System;
using System.Collections.Generic;
using System.Text;

namespace SyncHRoner.Common.Functional
{
    public class Failure
    {
        public string Error { get; }

        public Failure(string error)
        {
            Error = error;
        }
    }
}
