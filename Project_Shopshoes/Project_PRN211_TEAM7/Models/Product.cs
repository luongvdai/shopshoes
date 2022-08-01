using System;
using System.Collections.Generic;

#nullable disable

namespace Project_PRN211_TEAM7.Models
{
    public partial class Product
    {
        public Product()
        {
            OderDetails = new HashSet<OderDetail>();
            Sizes = new HashSet<Size>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public double? Discount { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int? BrandId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<OderDetail> OderDetails { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}
