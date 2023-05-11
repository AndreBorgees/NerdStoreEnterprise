using System;
using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class CartViewModel
    {
        public decimal TotalPrice { get; set; }
        public bool UsedVoucher { get; set; }
        public decimal Discount { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public List<ItemCartViewModel> Items { get; set; } = new List<ItemCartViewModel>();
    }

    public class ItemCartViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
