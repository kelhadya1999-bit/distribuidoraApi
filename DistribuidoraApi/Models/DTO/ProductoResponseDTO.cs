using DistribuidoraApi.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.DTO
{
    public class ProductoResponseDTO
    {
        public Guid Id { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal StockActual { get; set; }
        public decimal StockMinimo { get; set; }
        public bool Activo { get; set; }

        public string CodigoCategoria { get; set; } = string.Empty;
        public string CodigoMedida { get; set; } = string.Empty;

    }
}
