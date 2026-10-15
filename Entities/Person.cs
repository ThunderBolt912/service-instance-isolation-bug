using System;
using System.Collections.Generic;

namespace Entities
{
    /// <summary>
    /// Person Domain Model
    /// </summary>
    public class Person
    {
        public Guid PersonID { get; set; }
        public Guid? CountryID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public bool? RecieveNewsLetter { get; set; }

    }
}
