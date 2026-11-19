using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Models.Domain
{
    public class UnidadMedida
    {
        public Guid Id { get; set; }

        [MaxLength(10)]
        public string CodigoMedida { get; set; } = string.Empty; // "ud", "kg", "caja"
        public string Nombre { get; internal set; }
    }
}
