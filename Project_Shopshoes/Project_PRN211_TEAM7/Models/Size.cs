using System;
using System.Collections.Generic;

#nullable disable

namespace Project_PRN211_TEAM7.Models
{
    public partial class Size
    {
        public int ProductId { get; set; }
        public int Size1 { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
