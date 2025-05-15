using System.Reflection.PortableExecutable;

namespace Domain.Entities.Order
{
    public class Address
    {
        public Address()
        {
            
        }
        public Address(string firstName, string lastName, string street, string country, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            Country = country;
            City = city;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}