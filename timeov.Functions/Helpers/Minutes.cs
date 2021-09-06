using System;
using System.Collections.Generic;
using System.Text;

namespace timeov.Functions.Helpers
{
    public class Minutes
    {
        public int GetMinutesBetweenDates(DateTime initialDate, DateTime finalDate)
        {
            return (int)(finalDate - initialDate).TotalMinutes;
        }
    }
}
