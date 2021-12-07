using DataContext.Models;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class CalendarService : ICalendarService
    {
        public CalendarViewModel GetCalendar(int rentalId, IEnumerable<Booking> bookings, int preparationTimeInDays, DateTime startDate, int nights)
        {
            CalendarViewModel model = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var endDate = startDate.AddDays(nights - 1);
            var dateList = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days).Select(offset => startDate.AddDays(offset));

            model.Dates = dateList.Select(x => new CalendarDateViewModel(x)).ToList();

            var unit = 1;

            foreach (var b in bookings)
            {
                for (int i = 0; i < b.Nights; i++)
                {
                    var date = model.Dates.FirstOrDefault(x => x.Date == b.Start.AddDays(i));
                    if (date is null) continue;

                    date.Bookings.Add(new CalendarBookingViewModel(b.Id, unit));
                }

                for (int i = b.Nights + 1; i < b.Nights + 1 + preparationTimeInDays; i++)
                {
                    var date = model.Dates.FirstOrDefault(x => x.Date == b.Start.AddDays(i) || x.Date.AddDays(i) == b.Start.AddDays(i));
                    if (date is null) continue;

                    model.Dates.FirstOrDefault(x => x.Date == date.Date.AddDays(i))?.PreparationTimes.Add(new UnitViewModel(unit));
                }

                unit++;
            }

            model.Dates = model.Dates.OrderBy(x => x.Date).ToList();

            return model;
        }
    }
}
