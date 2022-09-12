using FluentValidation;
using System;
using System.Text.Json.Serialization;

namespace NSE.Carrinho.API.Model
{
    public class CartItem
    {
        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public Guid CartId { get; set; }
        [JsonIgnore]
        public CartCustomer CartCustomer { get; set; }

        internal void CartAssociate(Guid cartId)
        {
            CartId = cartId;
        }

        internal decimal CalculatePrice()
        {
            return Quantity * Price;
        }

        internal void AddUnits(int unit)
        {
            Quantity += unit;
        }

        internal void UpdateUnits(int unit)
        {
            Quantity = unit;
        }

        internal bool IsValid()
        {
            return new CartItemValidation().Validate(this).IsValid;
        }

        public class CartItemValidation : AbstractValidator<CartItem>
        {
            public CartItemValidation()
            {
                RuleFor(c => c.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");

                RuleFor(c => c.Name)
                   .NotEmpty()
                   .WithMessage("O nome do produto não foi informado");

                RuleFor(c => c.Quantity)
                   .GreaterThan(0)
                   .WithMessage(item => $"A quantidade mínima para o {item.Name} é 1");

                RuleFor(c => c.Quantity)
                   .LessThanOrEqualTo(CartCustomer.MAX_QUANTITY_ITEM)
                   .WithMessage(item => $"A quantidade máxima do {item.Name} é {CartCustomer.MAX_QUANTITY_ITEM}");

                RuleFor(c => c.Price)
                   .GreaterThan(0)
                   .WithMessage(item => $"O valor do {item.Name} precisar ser maior que 0");
            }
        }
    }
}
