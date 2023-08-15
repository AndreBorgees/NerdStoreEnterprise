using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace NSE.WebApp.MVC.Extensions
{
    public static class RazorHelpers
    {
        public static string HashEmailForGravatar(this RazorPage page, string email)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string FormatCoin(this RazorPage page, decimal value)
        {
            return value > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", value) : "Gratuito";
        }

        private static string FormatCoin(decimal value)
        {
            return string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", value);
        }

        public static string StockMessage(this RazorPage page, int quantity)
        {
            return quantity > 0 ? $"Apenas {quantity} em estoque!" : "Produto esgotado!";
        }

        public static string UnitsPerProduct(this RazorPage page, int units)
        {
            return units > 1 ? $"{units} unidades" : $"{units} unidade";
        }

        public static string UnitsPerProductTotalValue(this RazorPage page, int units, decimal value)
        {
            return $"{units}x {FormatCoin(value)} = Total: {FormatCoin(value * units)}";
        }

        public static string SelectOptionsPerQuantiry(this RazorPage page, int quantity, int selectedValue = 0)
        {
            var sb = new StringBuilder();
            for (var i = 1; i <= quantity; i++)
            {
                var selected = "";
                if (i == selectedValue) selected = "selected";
                sb.Append($"<option {selected} value='{i}'>{i}</option>");
            }

            return sb.ToString();
        }

        public static string ShowStatus(this RazorPage page, int status) 
        { 
            var statusMessage = "";
            var statusClass = "";

            switch (status) 
            {
                case 1:
                    statusClass = "info";
                    statusMessage = "Em aprovação";
                    break;
                case 2:
                    statusClass = "primary";
                    statusMessage = "Aprovado";
                    break;
                case 3:
                    statusClass = "danger";
                    statusMessage = "Recusado";
                    break;
                case 4:
                    statusClass = "success";
                    statusMessage = "Entregue";
                    break;
                case 5:
                    statusClass = "warning";
                    statusMessage = "Cancelado";
                    break;
            }

            return $"<span class='badge badge-{statusClass}'>{statusMessage}</span>";
        }
    }
}
