using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that's used as return type of most methods of Persons Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool? RecieveNewsLetter { get; set; }
        public double? Age { get; set; }


        /// <summary>
        /// Compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The PersonResponse Object to compare</param>
        /// <returns>True or false, indicating whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            // Checking if the second object we want to compare exists or not.
            if (obj == null)
            {
                return false;
            }

            // Checking if the comparison is between 2 objects of the  same PersonResponse types
            if (obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }

            // Typecasting the Object to a valid PersonResponse Object to access the PersonResponse properties
            PersonResponse person_to_compare = (PersonResponse)obj;

            return (PersonID == person_to_compare.PersonID) &&
                (PersonName == person_to_compare.PersonName) && (Email == person_to_compare.Email) && (DateOfBirth == person_to_compare.DateOfBirth) && (Gender == person_to_compare.Gender) && (CountryID == person_to_compare.CountryID) && (Address == person_to_compare.Address) && (RecieveNewsLetter == person_to_compare.RecieveNewsLetter);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person Name : {PersonName} , PersonID : {PersonID}, CountryID : {CountryID}";
        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// An extension method to convert an object of Person class into PersonResponse class
        /// </summary>
        /// <param name="person">The Person object to convert</param>
        /// /// <returns>Returns the converted PersonResponse object</retur
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                RecieveNewsLetter = person.RecieveNewsLetter,
                Address = person.Address,
                CountryID = person.CountryID,
                Gender = person.Gender,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            }; ; 
        }
    }
}
