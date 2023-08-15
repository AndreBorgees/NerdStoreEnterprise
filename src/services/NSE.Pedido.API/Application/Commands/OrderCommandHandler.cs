using FluentValidation.Results;
using MediatR;
using NSE.Core.Messages;
using NSE.Pedido.API.Application.DTO;
using NSE.Pedido.API.Application.Events;
using NSE.Pedidos.Domain.Orders;
using NSE.Pedidos.Domain.Vouchers;
using NSE.Pedidos.Domain.Vouchers.Specs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Pedido.API.Application.Commands
{
    public class OrderCommandHandler: CommandHandler, IRequestHandler<AddOrderCommand, ValidationResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVoucherRepository _voucherRepository;

        public OrderCommandHandler(IOrderRepository orderRepository, 
                                    IVoucherRepository voucherRepository)
        {
            _orderRepository = orderRepository;
            _voucherRepository = voucherRepository;
        }

        public async Task<ValidationResult> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            // Validação do comando
            if (!request.IsValid()) return request.ValidationResult;

            // Mapear pedido
            var order = OrderMap(request);

            // Aplicar voucher se houver
            if (!await ApplyVoucher(request, order)) return ValidationResult;

            //Validar pedido
            if(!OrderValidate(order)) return ValidationResult;

            // Processar pagamento
            if(!ProcessPayment(order)) return ValidationResult;

            // Se pagamento tudo okay
            order.AuthorizeOrder();

            // Adicionar evento
            order.AddEvent(new OrderRealizedEvent(order.Id, order.ClientId));

            // Adicionar pedido repositorio
            _orderRepository.Add(order);

            //Persiste dados do pedido e voucher
            return await PersistData(_orderRepository.UnitOfWork);
        }

        private Order OrderMap(AddOrderCommand request)
        {
            var address = new Address
            {
                Street = request.Address.Street,
                Number = request.Address.Number,
                Complement = request.Address.Complement,
                District = request.Address.District,
                PostalCode = request.Address.Cep,    
                City = request.Address.City,
                UF = request.Address.State
            };
        
            var order = new Order(request.ClientId, request.TotalValue, request.OrderItems.Select(OrderItemDTO.ForOrderItem).ToList(),
                request.VoucherUsed, request.Discount);

            order.AddAddress(address);

            return order;
        }

        private async Task<bool> ApplyVoucher(AddOrderCommand request, Order order)
        {
            if (!request.VoucherUsed) return true;

            var voucher = await _voucherRepository.GetVoucherByCode(request.VoucherCode);
            if(voucher == null)
            {
                AddError("O voucher informado não existe!");
                return false;
            }

            var voucherValidation = new VoucherValidation().Validate(voucher);
            if(!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AddError(m.ErrorMessage));
                return false;
            }

            order.AddVoucher(voucher);
            voucher.DebitAmount();

            _voucherRepository.Update(voucher);

            return true;
        }

        private bool OrderValidate(Order order)
        {
            var orderOriginalValue = order.TotalValue;
            var orderDiscount = order.Discount;

            order.CalculateOrderValue();

            if(order.TotalValue != orderOriginalValue)
            {
                AddError("O valor total do pedido não confere com o cálculo do pedido");
                return false;
            }

            if(order.Discount != orderDiscount)
            {
                AddError("O valor total não confere com o cálculo do pedido");
                return false;
            }

            return true;
        }

        private bool ProcessPayment(Order order)
        {
            return true;
        }
    }
}
