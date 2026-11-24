using DistribuidoraApi.Models.Domain;

namespace DistribuidoraApi.Repositories
{
    public interface ICategoriaRepository
    {
        //obtener Toda la lisa
        Task<List<Categoria>> GetAllCategoriasAsync();

        
        //obtener un solo objeto por id
        Task<Categoria> GetCategoriaByCodigoAsync(string codigoCategoria);

        //crear un nuevo objeto
        Task<Categoria> AddCategoriaAsync(Categoria categoria);

        //modificar un ubjeto en concreto
        Task<Categoria> UpdateCategoriaAsync(Categoria categoria);

        //eliminar un objeto en concreto
        Task DeleteCategoriaAsync(Categoria categoria);
    }
}
