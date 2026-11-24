using AutoMapper;
using DistribuidoraApi.Models.Domain;
using DistribuidoraApi.Models.DTO;
using DistribuidoraApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        // Inyección del repositorio de categorías y de AutoMapper por constructor
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        // Código de la categoría por defecto: no se puede borrar
        private const string CategoriaPorDefecto = "GEN";

        public CategoriaController(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        // GET: api/categoria
        [HttpGet]
        public async Task<IActionResult> GetAllCategorias()
        {
            var categorias = await _categoriaRepository.GetAllCategoriasAsync();

            // Mapear la lista de entidades a DTOs de salida
            var categoriasDTO = _mapper.Map<List<CategoriaResponseDTO>>(categorias);
            return Ok(categoriasDTO);
        }

        // GET: api/categoria/codigo/{codigoCategoria}
        [HttpGet("codigo/{codigoCategoria}")]
        public async Task<IActionResult> GetCategoriaByCodigo(string codigoCategoria)
        {
            var codigo = codigoCategoria.Trim().ToUpperInvariant();

            var categoria = await _categoriaRepository.GetCategoriaByCodigoAsync(codigo);
            if (categoria == null)
            {
                return NotFound($"No existe ninguna categoría con el código '{codigo}'.");
            }

            var categoriaDTO = _mapper.Map<CategoriaResponseDTO>(categoria);
            return Ok(categoriaDTO);
        }

        // POST: api/categoria
        [HttpPost]
        public async Task<IActionResult> AddCategoria([FromBody] AddCategoriaRequestDTO addCategoriaRequest)
        {
            // Normalizar el código
            var codigo = addCategoriaRequest.CodigoCategoria.Trim().ToUpperInvariant();

            // El código no puede estar repetido
            if (await _categoriaRepository.GetCategoriaByCodigoAsync(codigo) != null)
            {
                return Conflict($"Ya existe una categoría con el código '{codigo}'.");
            }

            // DTO de entrada → entidad + valores normalizados
            var categoria = _mapper.Map<Categoria>(addCategoriaRequest);
            categoria.Id = Guid.NewGuid();
            categoria.CodigoCategoria = codigo;
            categoria.Nombre = addCategoriaRequest.Nombre.Trim();

            await _categoriaRepository.AddCategoriaAsync(categoria);

            // Entidad → DTO de salida
            var categoriaDTO = _mapper.Map<CategoriaResponseDTO>(categoria);
            return CreatedAtAction(nameof(GetCategoriaByCodigo), new { codigoCategoria = categoria.CodigoCategoria }, categoriaDTO);
        }

        // PUT: api/categoria/{codigoCategoria}
        [HttpPut("{codigoCategoria}")]
        public async Task<IActionResult> UpdateCategoria(string codigoCategoria, [FromBody] UpdateCategoriaRequestDTO updateCategoriaRequest)
        {
            var codigo = codigoCategoria.Trim().ToUpperInvariant();

            var categoria = await _categoriaRepository.GetCategoriaByCodigoAsync(codigo);
            if (categoria == null)
            {
                return NotFound($"No existe ninguna categoría con el código '{codigo}'.");
            }

            // Solo se actualiza el nombre: el código es clave alternativa y no se puede modificar
            categoria.Nombre = updateCategoriaRequest.Nombre.Trim();

            await _categoriaRepository.UpdateCategoriaAsync(categoria);

            var categoriaDTO = _mapper.Map<CategoriaResponseDTO>(categoria);
            return Ok(categoriaDTO);
        }

        // DELETE: api/categoria/{codigoCategoria}
        [HttpDelete("{codigoCategoria}")]
        public async Task<IActionResult> DeleteCategoria(string codigoCategoria)
        {
            var codigo = codigoCategoria.Trim().ToUpperInvariant();

            // La categoría por defecto no se puede borrar
            if (codigo == CategoriaPorDefecto)
            {
                return BadRequest($"La categoría '{CategoriaPorDefecto}' es la categoría por defecto y no se puede eliminar.");
            }

            var categoria = await _categoriaRepository.GetCategoriaByCodigoAsync(codigo);
            if (categoria == null)
            {
                return NotFound($"No existe ninguna categoría con el código '{codigo}'.");
            }

            // No se puede borrar si tiene productos asociados
            //if (await _categoriaRepository.TieneProductosAsync(codigo))
            //{
            //    return Conflict($"La categoría '{codigo}' tiene productos asociados y no se puede eliminar.");
            //}

            await _categoriaRepository.DeleteCategoriaAsync(categoria);

            return NoContent();   // 204: borrado correctamente, sin contenido que devolver
        }
    }
}