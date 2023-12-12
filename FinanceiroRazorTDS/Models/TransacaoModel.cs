using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceiroRazorTDS.Models
{
    public enum TipoTransacao
    {
        Entrada, // Representa um crédito na conta, como um depósito ou recebimento
        Saida    // Representa um débito na conta, como um pagamento ou retirada
    }

    public class TransacaoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long TransacaoId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string NomeTransacao { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ValorTransacao { get; set; }

    [Required]
    public TipoTransacao TipoTransacao { get; set; }

    [Required]
    public DateTime DataTransacao { get; set; }
    
    public ICollection<CategoriaModel> Categorias { get; } = new List<CategoriaModel>();

    public string Descricao { get; set; }

    public int? UsuarioId { get; set; }

    public UsuarioModel? Usuario { get; set; } 
}

}
