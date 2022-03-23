using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProject1.Models
{
    public class Appointment
    {
        public string Name { get; set; }
        public string Severity { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
