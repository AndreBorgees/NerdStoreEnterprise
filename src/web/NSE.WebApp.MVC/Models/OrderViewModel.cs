using System;
using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class OrderViewModel
    {
        public int Code { get; set; }
        public int Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalValue { get; set; }
        public bool UsedVoucher { get; set; }
        public List<OrderItemViewModel> OrderItems { get;  set; }  = new List<OrderItemViewModel>();    

        public class OrderItemViewModel
        {
            public Guid ProductId { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
        }

        public AddressViewModel Address { get; set; }
    }
}

