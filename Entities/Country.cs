using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Domain Model for Country Entity
    /// </summary>
    public class Country
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }
}
