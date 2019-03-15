using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EliziumWeb.Controllers
{
    public class StatController : Controller
    {
        private readonly IOptions<AppSettings> config;

        public StatController(IOptions<AppSettings> config)
        {
            this.config = config;
        }

        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult Details(string id, string name)
        {
            JObject data = null;

            var f = config.Value.StatFolder;

            var file = new FileInfo($"{f}/{id}.json");
            if (!file.Exists)
                return NotFound();

            using (StreamReader jf = file.OpenText())
            {
                using (JsonTextReader reader = new JsonTextReader(jf))
                {
                    data = (JObject)JToken.ReadFrom(reader);
                }
            }

            ViewBag.UserName = name;

            return View(data);
        }
    }
}