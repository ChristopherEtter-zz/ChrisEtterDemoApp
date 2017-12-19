using Microsoft.AspNetCore.Mvc;
using ChrisEtterDemoApp.ViewModels;
using ChrisEtterDemoApp.Services;
using ChrisEtterDemoApp.Data.EF;
using System.Linq;
using ChrisEtterDemoApp.Data;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChrisEtterDempApp.Controllers
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IDataRepository _repository;

        public AppController(IMailService mailService, IDataRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {

            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {

            if (ModelState.IsValid)
            {
                //Send Email
                _mailService.SendMessage("christopheretter@outlook.com", model.Name, model.Email, model.Subject, model.Message);
                ViewBag.UserMessage = "Mail Sent";

                ModelState.Clear();
            }

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            return View();
        }
    }
}
