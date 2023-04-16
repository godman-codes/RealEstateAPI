using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Model
{
    public class Listings
    {
        //public Listings()
        //{
        //    this.DateCreated = DateTime.UtcNow;
        //    this.LastDateModified= DateTime.UtcNow;


        //}
        [Key]
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal StartingPrice { get; set; }
        public bool IsListed { get; set; }
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateModified { get; set; }
        public ICollection<Offers> Offers {get; set;}
        public ICollection<Images> Images { get; set;}
        public UsersOrRealtors Owner { get; set; }

    }
}
