using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VeryHard2.Models;

namespace VeryHard2.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly NorthwindContext _northwindContext;

		public HomeController(ILogger<HomeController> logger, NorthwindContext northwindContext)
		{
			_logger = logger;
			_northwindContext = northwindContext;
		}

		public IActionResult Index([FromQuery] string? numinput)
		{
			ViewBag.Data = GetCustomer(numinput);
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		private IEnumerable<string> GetCustomer(string? numinput)
		{
			IQueryable<string> result;
			if (numinput == null)
			{
				result = _northwindContext.Customers
				.Select(x => x.Address);
			}
			else
			{
				result = _northwindContext.Customers
				   .Where(x => x.Address.Contains(numinput))
				   .Select(x => x.Address);
			}


			List<string> final = new List<string>();

			foreach (var item in result)
			{
				final.Add(findAndReverseNumber(item));
			}

			return final;
		}

		private string findAndReverseNumber(string input)
		{
			List<char> numberChar = new List<char>();
			List<int> indexList = new List<int>();

			for (int i = 0; i < input.Length; i++)
			{
				if (Char.IsDigit(input[i]))
				{
					numberChar.Add(input[i]);
					indexList.Add(i);
				}
			}

			StringBuilder sb = new StringBuilder(input);
			for (int i = 0; i < indexList.Count; i++)
			{
				sb[indexList[i]] = numberChar[indexList.Count - 1 - i];
			}
			return sb.ToString();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
