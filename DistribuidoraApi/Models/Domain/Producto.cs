using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.Domain
{
    public class Producto
    {
        public Guid Id { get; set; }

        [MaxLength(20)]
        public string CodigoProducto { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public decimal PrecioVenta { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }

        public bool Activo { get; set; } = true; // para "dar de baja" sin borrar

       
        // Claves foráneas: TEXTO (se guardan en la tabla)
        [MaxLength(20)]
        public string CodigoCategoria { get; set; } = string.Empty;

        [MaxLength(10)]
        public string CodigoMedida { get; set; } = string.Empty;

        // Navegación: OBJETOS (no son columnas), con OTRO nombre
        public Categoria Categoria { get; set; } = null!;
        public UnidadMedida UnidadMedida { get; set; } = null!;
    }
}
