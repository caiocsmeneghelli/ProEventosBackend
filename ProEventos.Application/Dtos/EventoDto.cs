using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio."),
        //MinLength(3, ErrorMessage = "{0} deve possuir no m�nimo 3 caracteres."),
        //MaxLength(50, ErrorMessage = "{0} deve possuir no m�ximo 50 caracteres.")]
        StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve possuir o m�ximo de 50 caracteres e o m�nimo de 3.")]
        public string Tema { get; set; }

        [Display(Name = "Qtd de pessoas")]
        [Required (ErrorMessage ="O campo {0} � obrigat�rio.")]
        [Range(1, 120000, ErrorMessage = "O campo {0} deve possuir no m�nimo 1 e no m�ximo 120000.")]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio.")]
        [Phone(ErrorMessage = "O campo {0} esta inv�lido.")]
        public string Telefone { get; set; }

        [Display(Name = "e-mail")]
        [Required(ErrorMessage = "O campo {0} � obrigat�rio.")]
        [EmailAddress(ErrorMessage = "O campo {0} precisar ser v�lido.")]
        public string Email { get; set; }

        public int UserId { get; set; }
        public UserDto UserDto { get; set; }

        public IEnumerable<LoteDto>? Lotes { get; set; }
        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }
    }
}