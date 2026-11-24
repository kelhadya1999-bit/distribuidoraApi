using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.DTO
{
    public class AddCategoriaRequestDTO
    {
         [Required(ErrorMessage = "El código es obligatorio.")]
        [MaxLength(20)]
        public string CodigoCategoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}
