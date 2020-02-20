using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class WebApiTestController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebApiTestController(IValuesService ValuesService)
        {
            _ValuesService = ValuesService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _ValuesService.GetAsync();

            return View(values);
        }
    }
}