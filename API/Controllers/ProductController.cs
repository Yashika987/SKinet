using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Core.Interfaces;
using Core.Specification;
using API.Dtos;
using AutoMapper;

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
      public async Task <ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
      {
          var spec =new ProductsWithTypesAndBrandsSpecification();
           var products = await _productsRepo.ListAsync(spec);
          return Ok(_mapper.Map<IReadOnlyList<Products>, IReadOnlyList<ProductToReturnDto>>
          (products));
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