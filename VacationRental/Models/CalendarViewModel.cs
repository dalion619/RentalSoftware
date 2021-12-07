using DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }

        public CalendarViewModel() { }

        // Our customers requested a new feature related to the coronavirus restriction. They have to clean each rental unit after their guest occupies it. 
        // To achieve this we should block additional **X days** after each booking.Block means to not allow to have any booking for the same unit during this time.
        // The number of days will be defined by the host.

        public CalendarViewModel(int rentalId, IEnumerable<Booking> bookings, int preparationTimeInDays, DateTime startDate, int nights)
        {
            this.RentalId = rentalId;
            this.Dates = new List<CalendarDateViewModel>();
            var endDate = startDate.AddDays(nights - 1);
            var dateList = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days).Select(offset => startDate.AddDays(offset));
            this.Dates = dateList.Select(x => new CalendarDateViewModel(x)).ToList();

            var unit = 1;

            foreach (var b in bookings) //for each booking
            {
                //I think the first thing this is trying to ascertain is if there are any more bookings after the current booking
                for (int i = 0; i < b.Nights; i++) //for each day in that booking
                {
                    var date = this.Dates.FirstOrDefault(x => x.Date == b.Start.AddDays(i));
                    if (date is null) continue; //if there is no 

                    date.Bookings.Add(new CalendarBookingViewModel(b.Id, unit));
                }

                for (int i = b.Nights + 1; i < b.Nights + 1 + preparationTimeInDays; i++)
                {
                    var date = this.Dates.FirstOrDefault(x => x.Date == b.Start.AddDays(i) || x.Date.AddDays(i) == b.Start.AddDays(i));
                    if (date is null) continue;

                    this.Dates.FirstOrDefault(x => x.Date == date.Date.AddDays(i))?.PreparationTimes.Add(new UnitViewModel(unit));
                }
                unit++;
            }

            this.Dates = this.Dates.OrderBy(x => x.Date).ToList();
        }
    }
}
