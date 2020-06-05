using System;
using System.Collections.Generic;

namespace WebApplication
{
    public partial class City
    {
        public City()
        {
            DirectionCityFromNavigation = new HashSet<Direction>();
            DirectionCityToNavigation = new HashSet<Direction>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }

        public virtual ICollection<Direction> DirectionCityFromNavigation { get; set; }
        public virtual ICollection<Direction> DirectionCityToNavigation { get; set; }
    }
}
