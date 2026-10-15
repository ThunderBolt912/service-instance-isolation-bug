using System;
using System.Collections.Generic;
using System.Text;
using Entities;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// Data Transfer Object(DTO) Class for adding a new Country
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        // You have to convert the country details From the CountryAddRequest into the Country class.

        public Country ToCountry()
        {
            return new Country()
            {
                CountryName = CountryName
            };
        }
    }
}
