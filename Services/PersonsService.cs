using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse response = person.ToPersonResponse();
            response.Country = _countriesService.GetCountryByID(response.CountryID)?.CountryName;

            return response;
        }


        #region AddPerson Implementation
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
            var countriesService = new CountriesService(); // fresh per test
            var list = AddPersonsHelper.CreateListPersonAddRequest(countriesService);
            // Model Validation
            ValidationHelper.ModelValidation(personAddRequest);


            //if (string.IsNullOrEmpty(personAddRequest.PersonName))
            //{
            //    throw new ArgumentException(nameof(personAddRequest.PersonName));
            //}



            Person person = personAddRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }
        #endregion

        #region GetPersonByID
        public PersonResponse? GetPersonByID(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            // The temp variable in the lambda expression is referencing each element found in the _countries list.

            // The FirstOrDefault Method gets the first matching element in the list.
            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personId);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }
        #endregion


        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }

        public List<PersonResponse> GetFilteredPersons(string? serachBy, string? searchString)
        {
            throw new NotImplementedException();
        }
    }
}
