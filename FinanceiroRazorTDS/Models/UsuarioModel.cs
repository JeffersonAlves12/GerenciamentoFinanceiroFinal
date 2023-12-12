using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinanceiroRazorTDS.Models
{
    public enum Moeda
    {
        BRL,
        EUR,
        USD,
    }

    public class UsuarioModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "O primeiro nome é obrigatório.")]
        public string PrimeiroNome { get; set; }

        public string? UltimoNome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        public string? Telefone { get; set; }
        public Moeda? MoedaUsuario { get; set; }

        // Propriedade de navegação para as transações do usuário
        public virtual ICollection<TransacaoModel> Transacoes { get; set; } = new List<TransacaoModel>();
        public virtual ICollection<InvestimentoModel> Investimentos { get; set; } = new List<InvestimentoModel>();
        public virtual ICollection<EventoModel> Eventos { get; set; } = new List<EventoModel>();

    }
}
