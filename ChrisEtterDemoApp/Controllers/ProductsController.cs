using System;
using Microsoft.AspNetCore.Mvc;
using ChrisEtterDemoApp.Data;
using Microsoft.Extensions.Logging;

namespace ChrisEtterDemoApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Products/[Action]")]
    public class ProductsController : Controller
    {
        private IDataRepository _repository;
        private ILogger _logger;

        public ProductsController(IDataRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetAllProducts error {ex}");
                return BadRequest("Failed to get products");

            }
            
        }
    }
}