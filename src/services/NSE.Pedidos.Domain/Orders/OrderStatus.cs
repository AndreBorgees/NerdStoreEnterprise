namespace NSE.Pedidos.Domain.Orders
{
    public enum OrderStatus
    {
        Authorize = 1,
        Paid = 2,
        Refused = 3,
        Delivered = 4,
        Canceled = 5,
    }
}
