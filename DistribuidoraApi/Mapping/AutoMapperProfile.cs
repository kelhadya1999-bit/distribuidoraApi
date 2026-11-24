using AutoMapper;
using DistribuidoraApi.Models.Domain;
using DistribuidoraApi.Models.DTO;

namespace DistribuidoraApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Producto
            CreateMap<ProductoResponseDTO, Producto>().ReverseMap();
            CreateMap<AddProductoRequestDTO, Producto>().ReverseMap();
            CreateMap<UpdateProductoRequest, Producto>().ReverseMap();

            //Categoria
            CreateMap<CategoriaResponseDTO, Categoria>().ReverseMap();
            CreateMap<CategoriaRequestDTO, Categoria>().ReverseMap();

            //Unidad de medida

        }
    }
}
