using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacationRental.Models
{
    public class UnitViewModel
    {
        public UnitViewModel(int unit)
        {
            this.Unit = unit;
        }
        public int Unit { get; set; }
    }
}
