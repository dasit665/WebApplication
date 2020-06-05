using System;
using System.Collections.Generic;

namespace WebApplication
{
    public partial class Direction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CityFrom { get; set; }
        public int CityTo { get; set; }
        public int Plan { get; set; }
        public int Fact { get; set; }

        public virtual City CityFromNavigation { get; set; }
        public virtual City CityToNavigation { get; set; }
    }
}
