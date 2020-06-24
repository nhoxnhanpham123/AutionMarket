using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class User
    {
        [Key]
        public int UId { set; get; }

        [Required]
        [StringLength(30), MinLength(3, ErrorMessage = "Username must be more than 3 characters")]
        public string UName { set; get; }

        [Required]
        [StringLength(20), MinLength(3)]
        public string Password { set; get; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(20), MinLength(3)]
        public string Phone { set; get; }

        [StringLength(50)]
        public string Address { set; get; }

        [StringLength(300)]
        public string Image { set; get; }

        public virtual ICollection<Product> Products { set; get; }
        public virtual ICollection<HistoryAuction> HistoryAuctions { set; get; }

        public User()
        {
            this.Products = new HashSet<Product>();
            this.HistoryAuctions = new HashSet<HistoryAuction>();
        }
    }
    public class Product
    {
        [Key]
        public int PId { set; get; }

        [Required]
        [StringLength(50), MinLength(2)]
        [Display(Name = "Name")]
        public string PName { set; get; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Start")]
        public DateTime DateStart { set; get; }

        [DataType(DataType.Date)]
        [Display(Name = "Date End")]
        public DateTime DateEnd { set; get; }

        [Required]
        [Range(0, 100000000)]
        [Display(Name = "Start Price")]
        public int MinimumPrice { set; get; }

        [Range(0, 1000000000000)]
        [Display(Name = "Biggest Price")]
        public Nullable<int> MaximumPrice { set; get; }

        [Required]
        [StringLength(300), MinLength(2)]
        [DataType(DataType.MultilineText)]
        public string Description { set; get; }

        [Required]
        public string Status { set; get; }

        [Required]
        public int Incremenent { set; get; }

        public string Image { set; get; }
        
        [NotMapped]
        public HttpPostedFileBase ImageFile { set; get; }

        public int UId { set; get; }
        [Display(Name = "Category")]
        public int CId { set; get; }

        public virtual User User { set; get; }
        public virtual Category Category { set; get; }
    }
    public class Category
    {
        [Key]
        public int CId { set; get; }
        [Required]
        [StringLength(50), MinLength(3)]
        [Display(Name = "Category Name")]
        public string CName { set; get; }
        public ICollection<Product> Products { set; get; }
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
    }
    public class HistoryAuction
    {
        [Key]
        public int AHId { set; get; }
        public int PId { set; get; }
        public int UId { set; get; }
        public int PriceAuction { set; get; }
        public string Status { set; get; }
        public DateTime DateBid { set; get; }

        public virtual User User { set; get; }
    }
    public class Notification
    {
        [Key]
        public int NId { set; get; }
        public int UId { set; get; }
        public int PId { set; get; }
        public string Status { set; get; }
    }
    public class Admin
    {
        [Key]
        public int AId { set; get; }

        [Required]
        [StringLength(20), MinLength(3)]
        public string AName { set; get; }

        [Required]
        [StringLength(20), MinLength(3)]
        public string Password { set; get; }
    }
}