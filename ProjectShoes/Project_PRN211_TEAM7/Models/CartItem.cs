namespace Project_PRN211_TEAM7.Models
{
    
        public class CartItem
        {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }
        public double ThanhTien => Quantity * Price;
    }
    
}
