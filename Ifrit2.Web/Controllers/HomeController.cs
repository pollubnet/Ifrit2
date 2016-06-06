using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ifrit2.Models;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Devices;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ifrit2.Controllers
{
    public class HomeController : Controller
    {
        // automatyczna konfiguracja dzięki DI wbudowanemu w ASP.NET Core
        private readonly IoTConfig _settings;

        public HomeController(IOptions<IoTConfig> settings)
        {
            _settings = settings.Value;
        }

        // akcja index ma dwa parametry - dla kogo wysłać wiadomość i jaką
        public async Task<IActionResult> Index(string color, string device)
        {
            ViewData["Title"] = "Home Page";

            if (color != null)
            {
                // łączy się z Azure i wysyła wiadomość
                ServiceClient sc = ServiceClient.CreateFromConnectionString(_settings.AzureIoTHubConnectionString);
                await sc.OpenAsync();
                await sc.SendAsync(device, new Message(Encoding.UTF8.GetBytes(color)));
                await sc.CloseAsync();

                ViewData["Message"] = string.Format("Sent {0} to device {1}", color, device);
            }

            return View();
        }

        // akcja odbierająca informacje z formularza
        [HttpPost]
        public async Task<IActionResult> SetColor()
        {
            var form = Request.Form;

            ViewData["Title"] = "Home Page";

            // działa identycznie jak w Index, tylko opiera się na danych z formularza
            if (form["color"].First() != null)
            {
                ServiceClient sc = ServiceClient.CreateFromConnectionString(_settings.AzureIoTHubConnectionString);
                await sc.OpenAsync();
                await sc.SendAsync(form["device"].First(), new Message(Encoding.UTF8.GetBytes(form["color"].First())));
                await sc.CloseAsync();

                ViewData["Message"] = string.Format("Sent {0} to device {1}", form["color"].First(), form["device"].First());
            }

            return View("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
