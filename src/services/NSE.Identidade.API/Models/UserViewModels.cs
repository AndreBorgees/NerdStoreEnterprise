﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NSE.Identidade.API.Models
{
    public class UserRegistry
    {
        [Required(ErrorMessage = "O camop {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O camop {0} é obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O camop {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string PasswordConfirmation { get; set; }
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "O camop {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O Campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class UserResponseLogin
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
    }
    
    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims{ get; set; }
    }

    public class UserClaim
    {
        public string Value{ get; set; }
        public string Type { get; set; }
    }
}
