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
using API.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

      private readonly StoreContext _context;
      public ProductController(StoreContext context)
      {
           _context=context;
      }
      [HttpGet]
      public async Task <ActionResult<List<Products>>> GetProducts()
      {
           var products = await _context.Products.ToListAsync();
          return Ok(products);
      }
      [HttpGet("{id}")]
      public async Task <ActionResult<Products>> GetProducts(int id)
      {
           

          return await _context.Products.FindAsync(id);
      }

       
    }
}