using System.ComponentModel.DataAnnotations;

namespace UserAplication.Models
{
    public class Usuario{
        public int Id { get; set; }
        [StringLength(50 ,MinimumLength = 5)]
        [Display(Name = "Nome")]
        public string NomeDoContato { get; set; } = string.Empty;
        [Display(Name = "Data de Nacimento")]
        public DateTime DataDeNacimento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public int Idade { get; set; }
    }
}