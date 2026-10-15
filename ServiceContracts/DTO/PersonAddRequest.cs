using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServiceContracts.Enums;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for Inserting a new Person
    /// </summary>
    public class PersonAddRequest
    {
        public Guid? CountryID { get; set; }

        [Required(ErrorMessage ="Person Name is Required")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage ="Please Provide a Valid Email")]
        public string? Email { get; set; }
        public GenderOptions? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public bool? RecieveNewsLetter { get; set; }


        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetter = RecieveNewsLetter
            };
        }
    }

    

}
