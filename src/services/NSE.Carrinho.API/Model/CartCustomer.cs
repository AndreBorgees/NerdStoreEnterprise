using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Carrinho.API.Model
{
    public class CartCustomer
    {
        internal const int MAX_QUANTITY_ITEM = 5;

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public ValidationResult ValidationResult { get; set; }

        public CartCustomer(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public CartCustomer() { }

        internal void CalculateCartPrice()
        {
            TotalPrice = Items.Sum(p => p.CalculatePrice());
        }

        internal bool ExistingCartItem(CartItem item)
        {
            return Items.Any(p => p.ProductId == item.ProductId);
        }

        internal CartItem GetByProductId(Guid productId)
        {
            return Items.FirstOrDefault(p => p.ProductId == productId);
        }

        internal void AddItem(CartItem item)
        {
            item.CartAssociate(Id);

            if (ExistingCartItem(item))
            {
                var existingItem = GetByProductId(item.ProductId);
                existingItem.AddUnits(item.Quantity);

                item = existingItem;
                Items.Remove(existingItem);
            }

            Items.Add(item);
            CalculateCartPrice();
        }

        internal void UpdateItem(CartItem item)
        {
            item.CartAssociate(Id);

            var existingItem = GetByProductId(item.ProductId);
            Items.Remove(existingItem);
            Items.Add(item);
        }

        internal void UpdateUnits(CartItem item, int units)
        {
            item.UpdateUnits(units);
            UpdateItem(item);

            CalculateCartPrice();
        }

        internal void RemoveItem(CartItem item)
        {
            Items.Remove(GetByProductId(item.ProductId));
            CalculateCartPrice();
        }

        internal bool IsValid()
        {
            var errors = Items.SelectMany(i => new CartItem.CartItemValidation().Validate(i).Errors).ToList();
            errors.AddRange(new CartCustomerValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }

        public class CartCustomerValidation : AbstractValidator<CartCustomer>
        {
            public CartCustomerValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.TotalPrice)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precdisa ser maior que 0");
            }
        }
    }
}
