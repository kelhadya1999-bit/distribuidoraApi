using DistribuidoraApi.Models.Domain;
namespace DistribuidoraApi.Repositories
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllProductsAsync();

        Task<Producto> GetProductByCodigoAsync(string codigoProducto);

        Task<Producto> AddProductAsync(Producto producto);

        Task<Producto> UpdateProductAsync(Producto producto);

        Task<bool> DeleteProductAsync(string codigoProducto);
        Task<Categoria> GetCategoriaAsync(string codigoCategoria);
        Task<UnidadMedida> GetUnidadMedidaAsync(string codigoMedida);
    }
}
