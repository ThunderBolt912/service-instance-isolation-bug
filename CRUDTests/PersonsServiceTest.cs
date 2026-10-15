using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Services.Helpers;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _personService.AddPerson(request);
            });
        }


        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest? request = new PersonAddRequest
            {
                PersonName = null
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personService.AddPerson(request);
            });
        }


        [Fact]
        public void AddPerson_ValidRequest()
        {
            //Arrange
            PersonAddRequest? request = new PersonAddRequest
            {
                PersonName = "Taylor",
                Email = "Example@Person.com",
                CountryID = Guid.NewGuid(),
                Address = "Sample Address",
                Gender = GenderOptions.Other,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                RecieveNewsLetter = true
            };

            // Act

            PersonResponse response = _personService.AddPerson(request);

            //List<PersonResponse> persons_list = _personService.GetAllPersons();

            // Assert

            Assert.True(response.PersonID != Guid.Empty);

            // Assert.Contains(response, persons_list);

        }

        #endregion

        #region GetPersonByID


        [Fact]
        // If PersonID is == NUll, return Null as the PersonResponse Object
        public void GetPersonByID_NullPersonID()
        {

            // Arrange
            Guid? personID = null;

            // Act
            PersonResponse? personResponse = _personService.GetPersonByID(personID);

            // Assert
            Assert.Null(personResponse);

        }

        [Fact]
        // A valid PersonID is supplied, the Expected return is a matching Person details as PersonResponse object
        public void GetPersonByID_ValidPersonID()
        {
            // As each test case is Indepndant of each other, The list of the Persons start as an empty list, So we add a person to a list then compare its generated ID to the one fetched with the GetPersonByID method

            // Before Adding a new Person to the list we must initialize a new country to be able to associate to the person

            // At the top we utilized dependancy injection to be able to inject the countries service to the PersonsServiceTest class

            //Arrange
            CountryAddRequest country_request = new CountryAddRequest { CountryName = "Denmark" };

            CountryResponse country_response = _countriesService.AddCountry(country_request);

            //Act
            PersonAddRequest? person_AddRequest = new PersonAddRequest()
            {
                PersonName = "Abanoub",
                CountryID = country_response.CountryID,
                Address = "Sample Address",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "company@mail.com",
                Gender = GenderOptions.Other,
                RecieveNewsLetter = true
            };

            PersonResponse person_response_from_addRequest = _personService.AddPerson(person_AddRequest);

            PersonResponse? person_response_from_get = _personService.GetPersonByID(person_response_from_addRequest.PersonID);

            //Assert
            Assert.Equal(person_response_from_addRequest, person_response_from_get);

            // The Equal() Method internal triggers the .equals method we overriden in the past lecture
        }

        #endregion

        #region GetAllPersons

        [Fact]
        // The list of persons should be empty by default(before adding any persons)
        public void GetAllPersons_EmptyList()
        {

            // Act
            List<PersonResponse> person_responses_list = _personService.GetAllPersons();

            // Asssert

            Assert.Empty(person_responses_list);

        }

        //First, we add few persons; and then when we call GetAllPersons(), it should return the same persons that were added.

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            // Arrange

            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Nigeria" };
            //CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Panama" };

            //CountryResponse response1 = _countriesService.AddCountry(country_request_1);
            //CountryResponse response2 = _countriesService.AddCountry(country_request_2);

            //PersonAddRequest? person_AddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Matt",
            //    CountryID = response1.CountryID,
            //    Address = "Sample Address",
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    Email = "company@mail.com",
            //    Gender = GenderOptions.Other,
            //    RecieveNewsLetter = true
            //};

            //PersonAddRequest? person_AddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Hardy",
            //    CountryID = response2.CountryID,
            //    Address = "Sample",
            //    DateOfBirth = DateTime.Parse("2010-09-09"),
            //    Email = "BigMan@mail.com",
            //    Gender = GenderOptions.Male,
            //    RecieveNewsLetter = false
            //};

            //PersonAddRequest? person_AddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Frank",
            //    CountryID = response2.CountryID,
            //    Address = "New Add",
            //    DateOfBirth = DateTime.Parse("2014-01-01"),
            //    Email = "Bargin@mail.com",
            //    Gender = GenderOptions.Male,
            //    RecieveNewsLetter = true
            //};

            //List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_AddRequest1, person_AddRequest2, person_AddRequest3 };

            var person_requests = AddPersonsHelper.CreateListPersonAddRequest(_countriesService);

            // Expected Values from the list
            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();


            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }


            // Printing the person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var personResponseItems in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponseItems.ToString());
            }

            // Actual Values from the list
            // Act
            List<PersonResponse> person_response_list_from_get = _personService.GetAllPersons();

            // Printing the person_response_list_from_add
            _testOutputHelper.WriteLine("Actual: ");
            foreach (var personResponseItems in person_response_list_from_get)
            {
                _testOutputHelper.WriteLine(personResponseItems.ToString());
            }

            // Assert
            foreach (var person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, person_response_list_from_get);
            }
        }

        #endregion

        #region GetFilteredPersons

        [Fact]
        //If the search text is empty and searchBy is "PersonName", it should return all persons
        public void GetFilteredPersons_EmptySearchString()
        {
            // Arrange

            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Bolivia" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Ecuador" };

            CountryResponse response1 = _countriesService.AddCountry(country_request_1);
            CountryResponse response2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest? person_AddRequest1 = new PersonAddRequest()
            {
                PersonName = "Matt",
                CountryID = response1.CountryID,
                Address = "Sample Address",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "company@mail.com",
                Gender = GenderOptions.Other,
                RecieveNewsLetter = true
            };

            PersonAddRequest? person_AddRequest2 = new PersonAddRequest()
            {
                PersonName = "Hardy",
                CountryID = response2.CountryID,
                Address = "Sample",
                DateOfBirth = DateTime.Parse("2010-09-09"),
                Email = "BigMan@mail.com",
                Gender = GenderOptions.Male,
                RecieveNewsLetter = false
            };

            PersonAddRequest? person_AddRequest3 = new PersonAddRequest()
            {
                PersonName = "Frank",
                CountryID = response2.CountryID,
                Address = "New Add",
                DateOfBirth = DateTime.Parse("2014-01-01"),
                Email = "Bargin@mail.com",
                Gender = GenderOptions.Male,
                RecieveNewsLetter = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_AddRequest1, person_AddRequest2, person_AddRequest3 };

            // Expected Values from the list
            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            // Printing the person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (var personResponseItems in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(personResponseItems.ToString());
            }

            // Actual Values from the list
            // Act
            List<PersonResponse> person_response_from_search = _personService.GetFilteredPersons(nameof
                (Person.PersonName), searchString: "");

            // Printing the person_response_list_from_add
            _testOutputHelper.WriteLine("Actual: ");
            foreach (var personResponseItems in person_response_from_search)
            {
                _testOutputHelper.WriteLine(personResponseItems.ToString());
            }

            // Assert
            foreach (var person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, person_response_from_search);
            }




        }



        #endregion

        #region SeparateServicesError

        [Fact]
        public void AddPerson_WithRealCountry_ShouldReturnCountryNameInResponse()
        {
            // Arrange: Add a country via test's CountriesService
            var countryRequest = new CountryAddRequest { CountryName = "Greece" };
            var countryResponse = _countriesService.AddCountry(countryRequest);

            // Create person linked to that country
            var personRequest = new PersonAddRequest
            {
                PersonName = "TestError",
                CountryID = countryResponse.CountryID,  // Real country ID
                Email = "matt@mail.com",
                Address = "Sample Address",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                RecieveNewsLetter = true
            };

            // Act: Add person
            PersonResponse result = _personService.AddPerson(personRequest);

            // Printing Some Variables to Get the real Error pictured
            _testOutputHelper.WriteLine($"CountryName Added: {countryResponse.CountryName}");
            _testOutputHelper.WriteLine($"CountryID Added: {countryResponse.CountryID}");

            // Printing Varaiables for the country associated with the Person
            _testOutputHelper.WriteLine($"PersonResponse CountryName Added: {result.Country}");
            _testOutputHelper.WriteLine($"PersonResponse CountryID Added: {result.CountryID}");


            // Assert: Country name should be "Greece"
            Assert.NotNull(result.Country);
            Assert.Equal("Greece", result.Country);  // This Assert statement is determined to fail - [result.Country] is = null
        }

        #endregion


    }
}
