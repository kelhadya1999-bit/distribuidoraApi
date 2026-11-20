using DistribuidoraApi.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.DTO
{
    public class UpdateProductoRequest
    {
        //[Required(ErrorMessage = "El código es obligatorio.")]
        //[MaxLength(20, ErrorMessage = "El código no puede superar 20 caracteres.")]
        //public string CodigoProducto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Range(0, 999999.99)]
        public decimal PrecioVenta { get; set; }

        [Range(0, 999999.99)]
        public decimal PrecioCompra { get; set; }

        [Range(0, 999999.999)]
        public decimal StockMinimo { get; set; }

        [MaxLength(20)]
        public string? CodigoCategoria { get; set;}= "GEN";

        [MaxLength(10)]
        public string? CodigoMedida { get; set; } = "UD";
    }
}
