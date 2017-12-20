using System;
using Microsoft.AspNetCore.Mvc;
using ChrisEtterDemoApp.Data;
using Microsoft.Extensions.Logging;
using ChrisEtterDemoApp.Data.EF.Entities;
using ChrisEtterDemoApp.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ChrisEtterDemoApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders/[Action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private IDataRepository _repository;
        private ILogger<OrdersController> _logger;
        private IMapper _mapper;
        private UserManager<StoreUser> _userManager;

        public OrdersController(IDataRepository repository, ILogger<OrdersController> logger, IMapper mapper, UserManager<StoreUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetAllOrders(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;

                var results = _repository.GetAllOrdersByUser(username, includeItems);

                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllOrders error {ex}");
                return BadRequest("Failed to get orders");

            }

        }

        [HttpGet("{id:int}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                var order = _repository.GetOrderById(User.Identity.Name, id);

                if (order != null)
                {
                    return Ok(_mapper.Map<Order, OrderViewModel>(order));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrderById error {ex}");
                return BadRequest("Failed to get order");

            }

        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody]OrderViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Order newOrder = _mapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    newOrder.User = currentUser;

                    //_repository.AddEntity(newOrder);
                    _repository.AddOrder(newOrder);

                    if (_repository.SaveAll())
                    {
                        return Created($"/api/orders/getorder/{newOrder.Id}", _mapper.Map<Order, OrderViewModel>(newOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
              
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"PostOrder error {ex}");
            }

            return BadRequest("Failed to save new order");

        }
    }
}