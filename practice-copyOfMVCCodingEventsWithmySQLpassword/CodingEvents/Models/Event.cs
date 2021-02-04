using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingEvents.Models
{
    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int NoOfAttendees { get; set; }
        public EventCategory Category { get; set; }
        public int CategoryId { get; set; }
        public string ContactEmail { get; set; }
        public int Id { get; set; }
   
        public Event (string name, string location, string description, int noOfAttendees, string contactEmail)
        {
            Name = name;
            Location = location;
            Description = description;
            NoOfAttendees = noOfAttendees;
            ContactEmail = contactEmail;            
        }

        public Event()
        {
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Event @event &&
                   Id == @event.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
