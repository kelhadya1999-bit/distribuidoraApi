using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.Domain
{
    public class Categoria
    {
        public Guid Id { get; set; }

        [MaxLength(20)]
        public string? CodigoCategoria { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        //navegar los productos de la categoria
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
