using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LokiAspnetCore.Classes;
using System.Text;

namespace LokiAspnetCore.Controllers
{
    public class TransformViewModel {
        public List<User> Goddesses { get; set; }
        public List<User> Knowlege { get; set; }
    }

    /**
     * Generic Controller meant to (optionally) expose all loki service function to client-side. 
     *
     */
    public class TransformExampleController : Controller
    {
        // We will define reusable configuration parameters here since this controller uses single database
        private IHostingEnvironment _env;

        private LokiDatabaseConfiguration _demoServiceConfiguration;
        
        public TransformExampleController(IHostingEnvironment env)

        {
            _env = env;

            _demoServiceConfiguration  = new LokiDatabaseConfiguration(
                "./node_modules/loki-nodeservice/lokiservice.js", 
                env.ContentRootPath.Replace("\\", "/") + "/nodesvcs/demo1-service.init.js", 
                "./dbinstances/demo-1.db"
            );
        }
        
        public async Task<IActionResult> Index([FromServices] INodeServices nodeServices)
        {
            TransformViewModel model = new TransformViewModel();

            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);

            List<User> result;
            
            result = await db.Transform<User>("users", "goddesses", "{ \"AgeFilter\": 100 }");
            model.Goddesses = result;

            result = await db.Transform<User>("users", "knowlege");
            model.Knowlege = result;

            return View(model);
        }
    }
}
