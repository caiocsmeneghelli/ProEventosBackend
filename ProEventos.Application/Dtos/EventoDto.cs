using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        //MinLength(3, ErrorMessage = "{0} deve possuir no mínimo 3 caracteres."),
        //MaxLength(50, ErrorMessage = "{0} deve possuir no máximo 50 caracteres.")]
        StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve possuir o máximo de 50 caracteres e o mínimo de 3.")]
        public string Tema { get; set; }

        [Display(Name = "Qtd de pessoas")]
        [Required (ErrorMessage ="O campo {0} é obrigatório.")]
        [Range(1, 120000, ErrorMessage = "O campo {0} deve possuir no mínimo 1 e no máximo 120000.")]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Phone(ErrorMessage = "O campo {0} esta inválido.")]
        public string Telefone { get; set; }

        [Display(Name = "e-mail")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo {0} precisar ser válido.")]
        public string Email { get; set; }

        public int UserId { get; set; }
        public UserDto UserDto { get; set; }

        public IEnumerable<LoteDto>? Lotes { get; set; }
        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }
    }
}