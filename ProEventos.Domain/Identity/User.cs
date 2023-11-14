﻿using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enums;
namespace ProEventos.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public Titulo Titulo { get; set; }
        public string? Descricao { get; set; }
        public Funcao Funcao { get; set; }
        public string? ImagemPerfil { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }

        public string NomeCompleto
        {
            get
            {
                return string.Format("{0} {1}", PrimeiroNome, UltimoNome);
            }
        }
    }
}
