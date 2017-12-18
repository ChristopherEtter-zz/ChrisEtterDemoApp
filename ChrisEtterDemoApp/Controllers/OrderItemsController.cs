using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChrisEtterDemoApp.Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ChrisEtterDemoApp.Data.EF.Entities;
using ChrisEtterDemoApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChrisEtterDemoApp.Controllers
{
    [Produces("application/json")]
    [Route("api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
    {
        private IDataRepository _repository;
        private ILogger _logger;
        private IMapper _mapper;

        public OrderItemsController(IDataRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, orderId);
                if (order != null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Get order items error {ex}");
                return BadRequest("Failed to get order items");
            }
           
        }
        [HttpGet("{orderItemId}")]
        public IActionResult Get(int orderId, int orderItemId)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, orderId);
                if (order != null)
                {
                    var item = order.Items.Where(x => x.Id == orderItemId).FirstOrDefault();
                    return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get order items error {ex}");
                return BadRequest("Failed to get order items");
            }

        }
    }
}