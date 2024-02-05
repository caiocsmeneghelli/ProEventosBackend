using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class PalestranteUpdateDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Minicurriculo { get; set; }
    }
}
