using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        // There'll be only one method in this ICountryService,it receives the CountryAddRequest object and returns the CountryResponse type.

        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="request">Country Object to be Added</param>
        /// <returns>Returns the country object after adding it (Including newly generated CountryID)</returns>
        CountryResponse AddCountry(CountryAddRequest? request);

        /// <summary>
        /// Returns all countries from the list 
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</CountryResponse></returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="countryId">CountryID (guid) to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
        CountryResponse? GetCountryByID(Guid? countryId);

    }
}
