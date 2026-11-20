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
        // Constructor del controlador que recibe el contexto de base de datos como parámetro
        public ProductoController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            // Obtiene todos los productos de la base de datos utilizando el contexto de base de datos
            var productos = await _productoRepository.GetAllProductsAsync();
            //mapear el un DTO de producto para no exponer la entidad directamente
            var productoDTOs = productos.Select(p => new Models.DTO.ProductoResponseDTO
            {
                Id = p.Id,
                CodigoProducto = p.CodigoProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioVenta = p.PrecioVenta,
                StockActual = p.StockActual,
                StockMinimo = p.StockMinimo,
                Activo = p.Activo
            }).ToList();
            return Ok(productoDTOs);
        }

        [HttpGet("{CodigoProducto}")]
        public async Task<IActionResult> GetProductById([Required] string CodigoProducto)
        {
            // Validación del parámetro de entrada
            if (CodigoProducto == string.Empty || CodigoProducto == null)
            {
                return BadRequest("El código del producto no puede estar vacío.");
            }
            // Busca el producto en la base de datos por su código
            var producto = await _productoRepository.GetProductByCodigoAsync(CodigoProducto);

            // Si no se encuentra el producto, devuelve un 404 Not Found
            if (producto == null)
            {
                return NotFound();
            }
            //mapear el producto a un DTO de salida para no exponer la entidad directamente
            var productoDTO = new Models.DTO.ProductoResponseDTO
            {
                CodigoProducto = producto.CodigoProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioVenta = producto.PrecioVenta,
                StockActual = producto.StockActual,
                StockMinimo = producto.StockMinimo,
                Activo = producto.Activo
            };
            return Ok(productoDTO);
        }

        // POST: api/producto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProductoRequestDTO request)
        {
            // Normalizar códigos (mayúsculas, sin espacios) y aplicar valores por defecto
            var codigo = request.CodigoProducto.Trim().ToUpperInvariant();

            var codigoCategoria = string.IsNullOrWhiteSpace(request.CodigoCategoria)
                ? "GEN"
                : request.CodigoCategoria.Trim().ToUpperInvariant();

            var codigoMedida = string.IsNullOrWhiteSpace(request.CodigoMedida)
                ? "UD"
                : request.CodigoMedida.Trim().ToUpperInvariant();

            // Reglas de negocio
            if (await _productoRepository.GetProductByCodigoAsync(codigo) != null)
                return Conflict($"Ya existe un producto con el código '{codigo}'.");

            if (await _productoRepository.GetCategoriaAsync(codigoCategoria) == null)
                return BadRequest($"La categoría '{codigoCategoria}' no existe.");

            if (await _productoRepository.GetUnidadMedidaAsync(codigoMedida) == null)
                return BadRequest($"La unidad de medida '{codigoMedida}' no existe.");

            // DTO de entrada → entidad
            var producto = new Producto
            {
                Id = Guid.NewGuid(),
                CodigoProducto = codigo,
                Nombre = request.Nombre.Trim(),
                Descripcion = request.Descripcion,
                PrecioVenta = request.PrecioVenta,
                PrecioCompra = request.PrecioCompra,
                StockMinimo = request.StockMinimo,
                StockActual = 0,
                Activo = true,
                CodigoCategoria = codigoCategoria,
                CodigoMedida = codigoMedida
            };

            await _productoRepository.AddProductAsync(producto);
            
            var productoDTO = ToDto(producto);
            return CreatedAtAction(nameof(GetProductById),
                new { codigoProducto = producto.CodigoProducto },
                new { mensaje = "Se ha creado el producto", producto = productoDTO });
        }

        [HttpPut("{codigoProducto}")]
        public async Task<IActionResult> Update([Required] string codigoProducto, UpdateProductoRequest request)
        {
            //obtener el producto de la base de datos
            var productDomainModel = await _productoRepository.GetProductByCodigoAsync(codigoProducto);
            //comprobar si existe, si no existe devolver un 404
            if(productDomainModel == null)
            {
                return NotFound();
            }
            //comprobar si la categoria existe, si no existe devolver un 400
            if(request.CodigoCategoria != null)
            {
                var categoria = await _productoRepository.GetCategoriaAsync(request.CodigoCategoria);
                if (categoria == null)
                {
                    return BadRequest($"La categoría '{request.CodigoCategoria}' no existe.");
                }
            }
            //comprobar si la unidad de medida existe, si no existe devolver un 400
            if (request.CodigoMedida != null)
            { 
                var unidadMedida = await _productoRepository.GetUnidadMedidaAsync(request.CodigoMedida);
                if (unidadMedida == null)
                {
                    return BadRequest($"La unidad de medida '{request.CodigoMedida}' no existe.");
                }
            }

            //mapear dto a domain model
            productDomainModel.Nombre = request.Nombre;
            productDomainModel.Descripcion = request.Descripcion;
            productDomainModel.PrecioVenta = request.PrecioVenta;
            productDomainModel.PrecioCompra = request.PrecioCompra;
            productDomainModel.StockMinimo = request.StockMinimo;
            productDomainModel.CodigoCategoria = request.CodigoCategoria ?? productDomainModel.CodigoCategoria;
            productDomainModel.CodigoMedida = request.CodigoMedida ?? productDomainModel.CodigoMedida;


            //mapear domain model a dto de salida
            var productoDTO = ToDto(productDomainModel);

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

        // Entidad → DTO de salida (provisional, hasta AutoMapper)
        private static ProductoResponseDTO ToDto(Producto p) => new()
        {
            CodigoProducto = p.CodigoProducto,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            PrecioVenta = p.PrecioVenta,
            StockActual = p.StockActual,
            StockMinimo = p.StockMinimo,
            Activo = p.Activo,
            CodigoCategoria = p.CodigoCategoria,
            CodigoMedida = p.CodigoMedida
        };

    }
}
