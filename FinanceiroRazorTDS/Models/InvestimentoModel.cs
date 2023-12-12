using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceiroRazorTDS.Models
{

    public enum TipoInvestimento
{
    Acoes,
    Titulos,
    FundosImobiliarios,
    CDB,
    LCI,
    LCA,
    ETF,
    Poupanca,
    Outro 
}

    public class InvestimentoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do investimento é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O tipo do investimento é obrigatório.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "O valor investido é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorInvestido { get; set; }

        [Required(ErrorMessage = "A data de compra é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataDeCompra { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorAtual { get; set; }

       [DataType(DataType.Date)]
       public DateTime? DataUltimaAtualizacao { get; set; }

        public int? UsuarioId { get; set; }

  
        public UsuarioModel? Usuario { get; set; }
    }
}
