using System.Collections.Generic;

namespace NSE.Bff.Compras.Models
{
    public class CartDTO
    {
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public List<CartItenDTO> Items { get; set; } = new List<CartItenDTO>();
    }
}
