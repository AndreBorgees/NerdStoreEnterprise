using System;

namespace NSE.Bff.Compras.Models
{
    public class ProductItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
    }
}
