using StockPulse.Models;
using System.ComponentModel.DataAnnotations;

namespace StockPulse.Dtos
{
    public class CustomerDto
    {
        [Required]
        public string Email { get; set; }

        public string? Street { get; set; }

        [Display(Name = "Zipcode")]
        public string? ZipCode { get; set; }

        public string? City { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        public string? Particle { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Display(Name = "Phone-number")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Pronouns { get; set; }


        public Person ExtractPerson()
        {
            return new Person
            {
                Email = this.Email,
                FirstName = this.FirstName,
                Particle = this.Particle,
                Lastname = this.Lastname,
                PhoneNumber = this.PhoneNumber,
                Pronouns = this.Pronouns
            };
        }

        public Customer ExtractCustomer()
        {
            return new Customer
            {
                PersonEmail = this.Email,
                City = this.City,
                Street = this.Street,
                ZipCode = this.ZipCode
            };
        }

        public static CustomerDto Combine(Customer customer, Person person)
        {
            return new CustomerDto
            {
                Email = person.Email,
                FirstName = person.FirstName,
                Particle = person.Particle,
                Lastname = person.Lastname,
                PhoneNumber = person.PhoneNumber,
                Pronouns = person.Pronouns,
                City = customer.City,
                Street = customer.Street,
                ZipCode = customer.ZipCode
            };
        }
    }
}
