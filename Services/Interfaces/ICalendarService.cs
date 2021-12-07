using DataContext.Models;
using Services.Models;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendar(int rentalId, IEnumerable<Booking> bookings, int preparationTimeInDays, DateTime startDate, int nights);
    }
}