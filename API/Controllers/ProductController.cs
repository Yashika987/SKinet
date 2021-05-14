
using System.Collections.Generic;

using System.Threading.Tasks;

using Core.Entities;

using Microsoft.AspNetCore.Mvc;

using Core.Interfaces;
using Core.Specification;
using API.Dtos;
using AutoMapper;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        IGenericRepository<Products> _productsRepo;
        IGenericRepository<ProductBrand> _productBrandRepo;
        IGenericRepository<ProductType> _productTypeRepo;
        IMapper _mapper;
        public ProductController(IGenericRepository<Products> productsRepo, 
        IGenericRepository<ProductBrand> productBrandRepo, 
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper=mapper;
            _productsRepo=productsRepo;
            _productBrandRepo=productBrandRepo;
            _productTypeRepo=productTypeRepo;

        }

    //   private readonly StoreContext _context;
    //   public ProductController(StoreContext context)
    //   {
    //        _context=context;
    //   }
      [HttpGet]
      public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParam productParams)
        {
            
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            
            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, 
                productParams.PageSize, totalItems, data));
        }
      [HttpGet("{id}")]
      public async Task <ActionResult<ProductToReturnDto>> GetProducts(int id)
      {
          var spec =new ProductsWithTypesAndBrandsSpecification(id);
          var product= await _productsRepo.GetEntityWithSpec(spec);
          return _mapper.Map<Products ,ProductToReturnDto >(product);

      }
      
       [HttpGet("brands")]
       public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
       {
           
           return Ok(await _productBrandRepo.ListAllAsync());
       }
     [HttpGet("types")]       
     public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductType()
       {
           return Ok(await _productTypeRepo.ListAllAsync());
       }
       
    }
}