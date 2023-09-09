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
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }

        [EmailAddress(ErrorMessage = "O campo {0} precisar ser v�lido.")]
        public string Email { get; set; }
        public IEnumerable<LoteDto>? Lotes { get; set; }
        public IEnumerable<RedeSocialDto>? RedesSociais { get; set; }
        public IEnumerable<PalestranteDto>? Palestrantes { get; set; }
    }
}