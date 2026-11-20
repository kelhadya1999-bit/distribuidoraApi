using DistribuidoraApi.Data;
using DistribuidoraApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraApi.Repositories
{
    public class SQLProductoRepository : IProductoRepository
    {
        //inyectar la clase con DistribuidoraDbContext para poder acceder a la base de datos
        private readonly DistribuidoraDbContext _dbContext;
        public SQLProductoRepository(DistribuidoraDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Producto>> GetAllProductsAsync()
        {
            return await _dbContext.Productos.ToListAsync();
        }

        public async Task<Producto> GetProductByCodigoAsync(string codigoProducto)
        {
            return await _dbContext.Productos.FirstOrDefaultAsync(p => p.CodigoProducto == codigoProducto);
        }

        public async Task<Producto> AddProductAsync(Producto producto)
        {
            _dbContext.Productos.Add(producto);
            await _dbContext.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateProductAsync(Producto producto)
        {
            _dbContext.Productos.Update(producto);
            await _dbContext.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteProductAsync(string codigoProducto)
        {
            var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.CodigoProducto == codigoProducto);
            if (producto == null)
            {
                return false;
            }
            _dbContext.Productos.Remove(producto);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        //categorias
        public async Task<Categoria> GetCategoriaAsync(string codigoCategoria)
        {
            return await _dbContext.Categorias.FirstOrDefaultAsync(c => c.CodigoCategoria == codigoCategoria);
        }
        //medidas
        public async Task<UnidadMedida> GetUnidadMedidaAsync(string codigoMedida)
        {
            return await _dbContext.UnidadesMedida.FirstOrDefaultAsync(u => u.CodigoMedida == codigoMedida);
        }
    }
}
