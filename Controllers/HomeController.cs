using Microsoft.AspNetCore.Mvc;
using ProyectoChart.Models;
using ProyectoChart.Models.ViewModels;
using System.Diagnostics;

namespace ProyectoChart.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBPRUEBASContext _dbcontext;

        public HomeController(DBPRUEBASContext context)
        {
            _dbcontext = context;
        }


        public IActionResult resumenVenta()
        {

            DateTime FechaInicio = DateTime.Now;
            FechaInicio = FechaInicio.AddDays(-5);

            List<VMVenta> Lista = (from tbventa in _dbcontext.Venta
                                   where tbventa.FechaRegistro.Value.Date >= FechaInicio.Date
                                   group tbventa by tbventa.FechaRegistro.Value.Date into grupo
                                   select new VMVenta {
                                    fecha = grupo.Key.ToString("dd/MM/yyyy"),
                                    cantidad = grupo.Count(),
                                   }).ToList();



            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult resumenProducto()
        {


            List<VMProducto> Lista = (from tbdetalleventa in _dbcontext.DetalleVenta
                                   group tbdetalleventa by tbdetalleventa.NombreProducto into grupo
                                   orderby grupo.Count() descending
                                   select new VMProducto
                                   {
                                       producto = grupo.Key,
                                       cantidad = grupo.Count(),
                                   }).Take(4).ToList();



            return StatusCode(StatusCodes.Status200OK, Lista);
        }



        public IActionResult Index()
        {
            return View();
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