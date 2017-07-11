using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class LokiServiceController : Controller
    {
        // We will define reusable configuration parameters here since this controller uses single database
        private LokiDatabaseConfiguration _demoServiceConfiguration = new LokiDatabaseConfiguration(
            "nodesvcs/lokiservice.js", 
            "./demo1-service.init.js", 
            "./dbinstances/demo-1.db"
        );
        
        public IActionResult Index([FromServices] INodeServices nodeServices)
        {
            return View();
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Find([FromServices] INodeServices nodeServices, string collection, string queryObject) 
        {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);

            string nodeResponse = await db.Find(collection, queryObject);

            return Content(nodeResponse, "text/json"); 
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Insert([FromServices] INodeServices nodeServices, string collection, string obj) 
        {
            //var nodeResponse = await nodeServices.InvokeExportAsync<string>("nodesvcs/lokiservice.js", "insert", "./demo-service", "./dbinstances/demo.db", collection, obj);
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            string nodeResponse = await db.Insert(collection, obj);

            return Content(nodeResponse, "text/json"); 
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Update([FromServices] INodeServices nodeServices, string collection, string obj) 
        {
            //var nodeResponse = await nodeServices.InvokeExportAsync<string>("nodesvcs/lokiservice.js", "update", "./demo-service", "./dbinstances/demo.db", collection, obj);
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            string nodeResponse = await db.Update(collection, obj);

            return Content(nodeResponse, "text/json"); 
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Remove([FromServices] INodeServices nodeServices, string collection, int id) 
        {
            //var nodeResponse = await nodeServices.InvokeExportAsync<string>("nodesvcs/lokiservice.js", "remove", "./demo-service", "./dbinstances/demo.db", collection, id);
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            string nodeResponse = await db.Remove(collection, id);

            return Content(nodeResponse, "text/json"); 
        }

    }
}
