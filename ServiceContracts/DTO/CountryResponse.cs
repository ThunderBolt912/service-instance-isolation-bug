using Entities;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO Class That's used as a return type for CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        //
        public override bool Equals(object? obj)
        {
            // Checking if the second object we want to compare exists or not.
            if ( obj == null)
            {
                return false;
            }

            // Checking if the comparison is between 2 objects of the  same CountryResponse types
            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            // Typecasting the Object to a valid CountryResponse Object to access the CountryResponse properties
            CountryResponse country_to_compare = (CountryResponse)obj;

            return (this.CountryID == country_to_compare.CountryID) &&
                (this.CountryName == country_to_compare.CountryName);

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class CountryExtensions
    {
        // Converts from Country Object to COuntryResponse Object
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
