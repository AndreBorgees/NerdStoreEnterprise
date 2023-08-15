using NSE.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NSE.Bff.Compras.Models
{
    public class OrderDTO
    {
        public int Code { get; set; }   
        public int Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Discount { get; set; }
        public string VocuherCode { get; set; }
        public bool UsedVoucher { get; set; }
        public List<CartItenDTO> OrderItems { get; set; }
        public AddressDTO Address { get; set; }

        [Required(ErrorMessage = "Informe o número do cartão")]
        [DisplayName("Número do Cartão")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string CardName { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]
        [CardExpiration(ErrorMessage = "Cartão Expirado")]
        [Required(ErrorMessage = "Informe o vencimento")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string CardExpiration { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [DisplayName("Código de Segurança")]
        public string CardSecurity { get; set; }
    }
}
