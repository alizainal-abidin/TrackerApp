namespace Tracker.App.Api.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RegisterViewModel : LoginViewModel
    {
        public string FullName { get; internal set; }

        public string PhoneNumber { get; internal set; }
    }
}