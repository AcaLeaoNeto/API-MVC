using System.ComponentModel.DataAnnotations;

namespace User{
    public class Usuario{
        public int id { get; set;}
        [Required]
        public string NomeDoContato { get; set; } = string.Empty;
        [Required]
        public DateTime DataDeNacimento { get; set;}
        [Required]
        public string Sexo { get; set; } = string.Empty;
        public int Idade { get; set; }
        public bool Ativo { get; set;} = true; //Indica Conta Ativa
    }
}