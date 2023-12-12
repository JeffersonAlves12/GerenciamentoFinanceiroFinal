using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceiroRazorTDS.Models
{
   public class CategoriaModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long CategoriaId { get; set; }
    

    public string NomeCategoria { get; set; }
    public string IconeCategoria { get; set; } 
    public string DescricaoCategoria { get; set; }

    [ForeignKey("TransacaoId")]
    public long? TransacaoId { get; set; } 
    public TransacaoModel? Transacoes { get; set; }
 
     public CategoriaModel()
        {
            NomeCategoria = string.Empty;
            IconeCategoria = string.Empty;
            DescricaoCategoria = string.Empty;
            // TransacaoId e Transacoes já são anuláveis, então não precisam ser inicializados aqui
        }
}

    }
