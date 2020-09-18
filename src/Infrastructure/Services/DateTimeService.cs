namespace Infrastructure.Services
{
    using System;    
    using Application.Common.Interfaces;

    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}