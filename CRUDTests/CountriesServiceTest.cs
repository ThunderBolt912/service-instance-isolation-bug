using ServiceContracts;
using ServiceContracts.DTO;
using Services;
namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region Add-Country-tests

        // When CountryAddRequest is null, ArgumentNullException is Expected
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When CountryName is null, ArgumentException is Expected
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When CountryName is duplicate, ArgumentException is Expected
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "Norway"
            };

            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "Norway"
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        // when a proper CountryName is supplied, is should be added to the existing list of countries
        [Fact]
        public void AddCountry_ValidRequest()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Croatia"
            };

            // Act
           CountryResponse response = _countriesService.AddCountry(request);

            List<CountryResponse> countryResponses = _countriesService.GetAllCountries();

            // Assert

            Assert.True(response.CountryID != Guid.Empty);

            Assert.Contains(response, countryResponses);
        }

        #endregion

        #region GetAllCountries

        [Fact]
        // The list of countries should be empty by default(before adding any countries)
        public void GetAllCountries_EmptyList()
        {
            
            // Act
            List<CountryResponse> countryResponses = _countriesService.GetAllCountries();

            // Asssert

            Assert.Empty(countryResponses);

        }


        [Fact]
        public void GetallCountries_AddCountries()
        {
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest> {
                
                new CountryAddRequest() { CountryName = "USA"},
                new CountryAddRequest() { CountryName = "Canada"}
            };

            List<CountryResponse> countries_from_add_country = new List<CountryResponse>();

            foreach(var country_request in country_request_list)
            {
                countries_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            foreach (var expected_country in countries_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }

        #endregion

        #region GetCountryByID

        [Fact]
        // If CountryID is == NUll, return Null as the CountryResponse Object
        public void GetCountryByID_NullCountryID()
        {

            // Arrange
            Guid? countryID = null;

            // Act
            CountryResponse? countryResponse = _countriesService.GetCountryByID(countryID);

            // Assert
            Assert.Null(countryResponse);

        }

        [Fact]
        // A valid CountryID is supplied, the Expected return is a matching country details as CountryResponse object
        public void GetCountryByID_ValidCountryID()
        {
            // As each test case is Indepndant of each other, The list of the Countries start as an empty list, So we add a country to a list then compare its generated ID to the one fetched with the GetCountryByID method

            //Arrange
            CountryAddRequest? country_addRequest = new CountryAddRequest() { CountryName = "China" };

            CountryResponse country_response_from_addRequest = _countriesService.AddCountry(country_addRequest);

            //Act
            CountryResponse? country_response_from_get = _countriesService.GetCountryByID(country_response_from_addRequest.CountryID);

            //Assert
            Assert.Equal(country_response_from_addRequest, country_response_from_get);

            // The Equal() Method internal triggers the .equals method we overriden in the past lecture
        }



        #endregion

       
    }

}
