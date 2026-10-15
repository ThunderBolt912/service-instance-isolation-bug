using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    public class AddPersonsHelper
    {
        private readonly ICountriesService _countriesService;
        public AddPersonsHelper()
        {
            _countriesService = new CountriesService();
        }

        public static List<PersonAddRequest> CreateListPersonAddRequest(ICountriesService countriesService)
        {
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "Greece" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "Ecuador" };
            CountryAddRequest country_request_3 = new CountryAddRequest() { CountryName = "Estonia" };

            CountryResponse response1 = countriesService.AddCountry(country_request_1);
            CountryResponse response2 = countriesService.AddCountry(country_request_2);
            CountryResponse response3 = countriesService.AddCountry(country_request_3);

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
                PersonName = "Johan",
                CountryID = response3.CountryID,
                Address = "New Add",
                DateOfBirth = DateTime.Parse("2014-01-01"),
                Email = "Bargin@mail.com",
                Gender = GenderOptions.Male,
                RecieveNewsLetter = true
            };

            PersonAddRequest? person_AddRequest4 = new PersonAddRequest()
            {
                PersonName = "Kaya",
                CountryID = response2.CountryID,
                Address = "New Add",
                DateOfBirth = DateTime.Parse("2014-01-01"),
                Email = "Dero@mail.com",
                Gender = GenderOptions.Male,
                RecieveNewsLetter = true
            };

            PersonAddRequest? person_AddRequest5 = new PersonAddRequest()
            {
                PersonName = "Lucien",
                CountryID = response3.CountryID,
                Address = "New Add",
                DateOfBirth = DateTime.Parse("2014-01-01"),
                Email = "Totti@mail.com",
                Gender = GenderOptions.Male,
                RecieveNewsLetter = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_AddRequest1, person_AddRequest2, person_AddRequest3 };

            return person_requests;
        }
    }
}
