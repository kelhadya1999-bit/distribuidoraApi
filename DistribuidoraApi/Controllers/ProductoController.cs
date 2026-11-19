using DistribuidoraApi.Data;
using DistribuidoraApi.Models.Domain;
using DistribuidoraApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistribuidoraApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        // Inyección de dependencias del contexto de base de datos DistribuidoraDbContext
        private readonly DistribuidoraDbContext _dbContext;
        // Constructor del controlador que recibe el contexto de base de datos como parámetro
        public ProductoController(DistribuidoraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            // Obtiene todos los productos de la base de datos utilizando el contexto de base de datos
            var productos = _dbContext.Productos.ToList();
            //mapear el un DTO de producto para no exponer la entidad directamente
            var productoDTOs = productos.Select(p => new Models.DTO.ProductoResponseDTO
            {
                Id = p.Id,
                Codigo = p.CodigoProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioVenta = p.PrecioVenta,
                StockActual = p.StockActual,
                StockMinimo = p.StockMinimo,
                Activo = p.Activo
            }).ToList();
            return Ok(productoDTOs);
        }

        [HttpGet("{id:Guid}")]
        public IActionResult GetProductById(Guid id)
        {
            var producto = _dbContext.Productos.FirstOrDefault(p => p.Id == id);
            var productoDTO = new Models.DTO.ProductoResponseDTO
            {
                Id = producto.Id,
                Codigo = producto.CodigoProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioVenta = producto.PrecioVenta,
                StockActual = producto.StockActual,
                StockMinimo = producto.StockMinimo,
                Activo = producto.Activo
            };
            if (productoDTO == null)
            {
                return NotFound();
            }
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
            if (await _dbContext.Productos.AnyAsync(p => p.CodigoProducto == codigo))
                return Conflict($"Ya existe un producto con el código '{codigo}'.");

            if (!await _dbContext.Categorias.AnyAsync(c => c.CodigoCategoria == codigoCategoria))
                return BadRequest($"La categoría '{codigoCategoria}' no existe.");

            if (!await _dbContext.UnidadesMedida.AnyAsync(u => u.CodigoMedida == codigoMedida))
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

            _dbContext.Productos.Add(producto);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = producto.Id }, ToDto(producto));
        }

        // Entidad → DTO de salida (provisional, hasta AutoMapper)
        private static ProductoResponseDTO ToDto(Producto p) => new()
        {
            Id = p.Id,
            Codigo = p.CodigoProducto,
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
