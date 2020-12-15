using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Ntech.CitusData.Models;

namespace Ntech.CitusData.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly Tenant _currentTenant;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, Tenant tenant)
        {
            _logger = logger;
            _context = context;
            _currentTenant = tenant;
        }
        
        public async Task<IActionResult> Index()
        {
            var topQuestions = await _context
                                .Questions
                                .Where(q => q.Tenant.Id == _currentTenant.Id)
                                .OrderByDescending(q => q.UpdatedAt)
                                .Take(5)
                                .ToArrayAsync();

            var viewModel = new QuestionListViewModel
            {
                Questions = topQuestions
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
