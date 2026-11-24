using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.DTO
{
    public class CategoriaResponseDTO
    {
        [MaxLength(20)]
        public string? CodigoCategoria { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}
