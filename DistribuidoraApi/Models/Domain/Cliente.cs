using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.Domain
{
    public class Cliente
    {
        public Guid Id { get; set; }

        [MaxLength(20)]
        public string CodigoCliente { get; set; } = string.Empty;

        [MaxLength(15)]
        public string NifCif { get; set; } = string.Empty;

        [MaxLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [MaxLength(250)]
        public string? Direccion { get; set; }

        public bool Activo { get; set; } = true;

        //public ICollection<PedidoVenta> Pedidos { get; set; } = new List<PedidoVenta>();
    }
}
