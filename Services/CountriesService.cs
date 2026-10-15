using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // Private List of Countrie to hold the values of countries
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? request)
        {

            // Validation : CounrtyAddRequest parameter can't be null.
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Validation : CountryName can't be null.
            if (request.CountryName == null)
            {
                throw new ArgumentException(nameof(request.CountryName));
            }

            // Validation : CountryName shouldn't be a duplicate.

            if (_countries.Where(tempCountry => tempCountry.CountryName == request.CountryName).Count() > 0)
            {
                throw new ArgumentException("The Entered Country Name already exists !");
            }

            // Convert object from CounrtyAddRequest to a Country Object
            Country country = request.ToCountry();

            // Generate a GUID for the newly created country
            country.CountryID = Guid.NewGuid();

            // Add country object to the list
            _countries.Add(country);

            // Return CountryResponse Object after Validating and adding the country to the list
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {

            return _countries.Select(country => country.ToCountryResponse()).ToList();

        }


        public CountryResponse? GetCountryByID(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            // The temp variable in the lambda expression is referencing each element found in the _countries list.

            // The FirstOrDefault Method gets the first matching element in the list.
            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryId);

            if (country_response_from_list == null)
            {
                return null;
            }

            return country_response_from_list.ToCountryResponse();
        }
    }
}
