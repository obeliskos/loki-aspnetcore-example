using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LokiAspnetCore;
using System.Text;

namespace LokiAspnetCore.Controllers
{
    /**
     * Generic Controller meant to (optionally) expose all loki service function to client-side. 
     *
     */
    public class StatisticsController : Controller
    {
        // We will define reusable configuration parameters here since this controller uses single database
        private IHostingEnvironment _env;

        private LokiDatabaseConfiguration _demoServiceConfiguration;
        
        public StatisticsController(IHostingEnvironment env)

        {
            _env = env;

            _demoServiceConfiguration = new LokiDatabaseConfiguration(
                "./node_modules/loki-nodeservice/lokiservice.js", 
                env.ContentRootPath.Replace("\\", "/") + "/nodesvcs/demo1-service.init.js"
            );            
        }
        
        public async Task<IActionResult> Index([FromServices] INodeServices nodeServices)
        {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            LokiStatistics stats = await db.GetStatistics();
            
            return View(stats);
        }

        public async Task<IActionResult> InstanceStatistics([FromServices] INodeServices nodeServices, string initializer, string databasePath) 
        {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            LokiInstanceStats stats = await db.GetInstanceStatistics(initializer, databasePath);
            
            return View(stats);
        }

        public async Task<IActionResult> InstanceStatisticsJson([FromServices] INodeServices nodeServices, string initializer, string databasePath) 
        {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            string stats = await db.GetInstanceStatisticsJson(initializer, databasePath);
            
            return Json(stats);
        }

        [HttpPost]
        public async Task<IActionResult> InstanceStatsPartial([FromServices] INodeServices nodeServices, string initializer, string databasePath) {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            LokiInstanceStats lis = await db.GetInstanceStatistics(initializer, databasePath);
            
            return PartialView(lis);
        }
    }
}
