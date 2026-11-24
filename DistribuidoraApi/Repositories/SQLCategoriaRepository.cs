using DistribuidoraApi.Data;
using DistribuidoraApi.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraApi.Repositories
{
    public class SQLCategoriaRepository : ICategoriaRepository
    {
        private readonly DistribuidoraDbContext _dbContext;
        public SQLCategoriaRepository(DistribuidoraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //obtener toda la lista de categorias
        public async Task<List<Categoria>> GetAllCategoriasAsync()
        {
            return await _dbContext.Categorias.ToListAsync();
        }

        //obtener un solo objeto por codigo
        public async Task<Categoria> GetCategoriaByCodigoAsync(string codigoCategoria)
        {
            return await _dbContext.Categorias.FirstOrDefaultAsync(c => c.CodigoCategoria == codigoCategoria);
        }
        
        //crear un nuevo objeto
        public async Task<Categoria> AddCategoriaAsync(Categoria categoria)
        {
            _dbContext.Categorias.Add(categoria);
            await _dbContext.SaveChangesAsync();
            return categoria;
        }

        //modificar un objeto en concreto
        public async Task<Categoria> UpdateCategoriaAsync(Categoria categoria)
        {
            _dbContext.Categorias.Update(categoria);
            await _dbContext.SaveChangesAsync();
            return categoria;
        }

        //eliminar un objeto en concreto
        public async Task<bool> DeleteCategoriaAsync(Categoria categoria)
        {
            _dbContext.Categorias.Remove(categoria);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        Task ICategoriaRepository.DeleteCategoriaAsync(Categoria categoria)
        {
            return DeleteCategoriaAsync(categoria);
        }
    }
}
