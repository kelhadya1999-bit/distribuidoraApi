using AutoMapper;
using DistribuidoraApi.Data;
using DistribuidoraApi.Models.Domain;
using DistribuidoraApi.Models.DTO;
using DistribuidoraApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DistribuidoraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        // Inyección de dependencias del contexto de base de datos DistribuidoraDbContext

        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;
        // Constructor del controlador que recibe el contexto de base de datos como parámetro
        //implementar el automapper para mapear entre el DTO y la entidad
        public ProductoController(IProductoRepository productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            // Obtiene todos los productos de la base de datos utilizando el contexto de base de datos
            var productos = await _productoRepository.GetAllProductsAsync();

            // Mapea la lista de productos de dominio a una lista de DTOs de salida utilizando AutoMapper
            var productoDTOs = _mapper.Map<List<ProductoResponseDTO>>(productos);

            return Ok(productoDTOs);
        }

        [HttpGet("codigo/{codigoProducto}")]
        public async Task<IActionResult> GetProductByCode(string codigoProducto)
        {
            // Validación del parámetro de entrada
            if (string.IsNullOrWhiteSpace(codigoProducto))
            {
                return BadRequest("El código del producto no puede estar vacío.");
            }

            // Normaliza el código igual que al guardarlo
            var codigo = codigoProducto.Trim().ToUpperInvariant();

            // Busca el producto en la base de datos por su código
            var producto = await _productoRepository.GetProductByCodigoAsync(codigo);

            // Si no se encuentra el producto, devuelve un 404 Not Found
            if (producto == null)
            {
                return NotFound($"No existe ningún producto con el código '{codigo}'.");
            }

            // Mapear el producto a un DTO de salida para no exponer la entidad directamente
            var productoDTO = _mapper.Map<ProductoResponseDTO>(producto);
            return Ok(productoDTO);
        }

        // POST: api/producto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProductoRequestDTO addProductoRequestDTO)
        {
            // Normalizar códigos y aplicar valores por defecto
            var codigo = addProductoRequestDTO.CodigoProducto.Trim().ToUpperInvariant();

            var codigoCategoria = string.IsNullOrWhiteSpace(addProductoRequestDTO.CodigoCategoria)
                ? "GEN"
                : addProductoRequestDTO.CodigoCategoria.Trim().ToUpperInvariant();

            var codigoMedida = string.IsNullOrWhiteSpace(addProductoRequestDTO.CodigoMedida)
                ? "UD"
                : addProductoRequestDTO.CodigoMedida.Trim().ToUpperInvariant();

            // Reglas de negocio
            if (await _productoRepository.GetProductByCodigoAsync(codigo) != null)
                return Conflict($"Ya existe un producto con el código '{codigo}'.");

            if (await _productoRepository.GetCategoriaAsync(codigoCategoria) == null)
                return BadRequest($"La categoría '{codigoCategoria}' no existe.");

            if (await _productoRepository.GetUnidadMedidaAsync(codigoMedida) == null)
                return BadRequest($"La unidad de medida '{codigoMedida}' no existe.");

            // DTO de entrada → entidad (AutoMapper) + valores normalizados
            var productoDomainModel = _mapper.Map<Producto>(addProductoRequestDTO);
            productoDomainModel.Id = Guid.NewGuid();
            productoDomainModel.CodigoProducto = codigo;
            productoDomainModel.Nombre = addProductoRequestDTO.Nombre.Trim();
            productoDomainModel.CodigoCategoria = codigoCategoria;
            productoDomainModel.CodigoMedida = codigoMedida;

            await _productoRepository.AddProductAsync(productoDomainModel);

            // Entidad → DTO de salida
            var productoDTO = _mapper.Map<ProductoResponseDTO>(productoDomainModel);
            return CreatedAtAction(nameof(GetProductByCode), new { codigoProducto = productoDomainModel.CodigoProducto }, productoDTO);
        }

        [HttpPut("{codigoProducto}")]
        public async Task<IActionResult> Update([Required] string codigoProducto, UpdateProductoRequest updateProductoRequest) 
        {
            //obtener el producto de la base de datos
            var productDomainModel = await _productoRepository.GetProductByCodigoAsync(codigoProducto);
            //comprobar si existe, si no existe devolver un 404
            if(productDomainModel == null)
            {
                return NotFound();
            }
            //comprobar si la categoria existe, si no existe devolver un 400
            if(updateProductoRequest.CodigoCategoria != null)
            {
                var categoria = await _productoRepository.GetCategoriaAsync(updateProductoRequest.CodigoCategoria);
                if (categoria == null)
                {
                    return BadRequest($"La categoría '{updateProductoRequest.CodigoCategoria}' no existe.");
                }
            }
            //comprobar si la unidad de medida existe, si no existe devolver un 400
            if (updateProductoRequest.CodigoMedida != null)
            { 
                var unidadMedida = await _productoRepository.GetUnidadMedidaAsync(updateProductoRequest.CodigoMedida);
                if (unidadMedida == null)
                {
                    return BadRequest($"La unidad de medida '{updateProductoRequest.CodigoMedida}' no existe.");
                }
            }

            //mapear dto a domain model
            productDomainModel = _mapper.Map(updateProductoRequest, productDomainModel);


            //mapear domain model a dto de salida
            var productoDTO = _mapper.Map<ProductoResponseDTO>(productDomainModel);

            //devolver un 200 OK con el producto actualizado
            return Ok(productoDTO);
        }

        [HttpDelete("{codigoProducto}")]
        public async Task<IActionResult> Delete([Required] string codigoProducto)
        {
            //obtener el producto de la base de datos
            var productDomainModel = await _productoRepository.GetProductByCodigoAsync(codigoProducto);
            //comprobar si existe, si no existe devolver un 404
            if (productDomainModel == null)
            {
                return NotFound();
            }
            //eliminar el producto de la base de datos
            await _productoRepository.DeleteProductAsync(codigoProducto);

            return Ok("Se ha eliminado el producto "+ codigoProducto + "correctamente");
        }
    }
}
