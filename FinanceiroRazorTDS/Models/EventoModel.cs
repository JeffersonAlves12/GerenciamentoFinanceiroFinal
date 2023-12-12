using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinanceiroRazorTDS.Models
{
   public enum TipoEvento
{
    ContaAPagar,
    DebitoAutomatico,
    // Adicione outros tipos conforme necessário
}

public enum StatusEvento
{
    Paga,
    EmAtraso,
    Pendente
}

public class EventoModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "O tipo do evento é obrigatório.")]
    public TipoEvento Tipo { get; set; }

    [Required(ErrorMessage = "A data de consolidação é obrigatória.")]
    public DateTime DataConsolidacao { get; set; }

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O status do evento é obrigatório.")]
    public StatusEvento Status { get; set; }

    // Campo opcional para a foto do evento
    public string? FotoPath { get; set; }

    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public int? UsuarioId { get; set; }

    // Propriedade de navegação para o usuário associado
    public  UsuarioModel? Usuario { get; set; }

          public EventoModel()
        {
            // Inicializar com valores padrão
            Tipo = TipoEvento.ContaAPagar; // ou qualquer valor padrão
            DataConsolidacao = DateTime.Now; // ou qualquer valor padrão
            Valor = 0; // ou qualquer valor padrão
            Status = StatusEvento.Pendente; // ou qualquer valor padrão
            FotoPath = string.Empty; // ou qualquer valor padrão
            // UsuarioId e Usuario são anuláveis, então não precisam ser inicializados
        }
}


}