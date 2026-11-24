using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.DTO
{
    public class UpdateCategoriaRequestDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}
