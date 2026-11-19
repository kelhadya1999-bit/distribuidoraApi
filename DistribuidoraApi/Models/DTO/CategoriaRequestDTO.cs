using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.DTO
{
    public class CategoriaRequestDTO
    {
        [MaxLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}
