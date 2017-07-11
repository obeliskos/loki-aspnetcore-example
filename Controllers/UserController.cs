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
using LokiAspnetCoreExample.Classes;
using System.Text;

namespace LokiAspnetCore.Controllers
{
    ///<summary>
    /// This Class represents an MVC controller with routing mapped to /Views/User/ for 
    ///   actions returning a View or PartialView.
    ///
    /// We will use this controller for demoing concretely defined C# class persistence.
    /// Our concrete classes will reside in the App_Code folder along with the core
    ///   LokiService classes (LokiDatabase, LokiQueryChain, LokiTypes, etc.)
    ///</summary>
    public class UserController : Controller
    {
        //private LokiDatabaseConfiguration _demoServiceConfiguration = new LokiDatabaseConfiguration(
        //    "./node_modules/loki-nodeservice/lokiservice.js", 
        //    "/nodesvcs/demo1-service.init.js", 
        //    "./dbinstances/demo-1.db"
        //);
        private LokiDatabaseConfiguration _demoServiceConfiguration;

        private IHostingEnvironment _env;

        public UserController(IHostingEnvironment env)

        {
            _env = env;

            _demoServiceConfiguration = new LokiDatabaseConfiguration(
                "./node_modules/loki-nodeservice/lokiservice.js", 
                env.ContentRootPath.Replace("\\", "/") + "/nodesvcs/demo1-service.init.js", 
                "./dbinstances/demo-1.db"
            );            
        }
        
        ///<summary>
        /// MVC View Action for the Index.cshtml razor page in /Views/User folder.
        /// The model for that page will be a generic list of User class instances.
        /// All controll actions using lokiservice should accept the INodeServices 
        ///   from MVC dependency injection and pass that to LokiDatabase constructor.
        ///</summary>
        public async Task<IActionResult> Index([FromServices] INodeServices nodeServices)
        {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);

            List<User> result = await db.Find<User>("users");

            return View(result);
        }

        ///<summary>
        /// Example MVC Controller action which runs an example chained query and returns its results as json.
        ///</summary>
        [HttpGet]
        public async Task<IActionResult> RunCustomChainedQuery([FromServices] INodeServices nodeServices) {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);

            List<User> users = await db.Chain("users")
                .Find("{ age: { $gte: 50 }}")
                .SimpleSort("age", true)
                .Limit(3)
                .Data<User>();

            return Json(users);
        }

        ///<summary>
        /// MVC http post action for doing a loki get and receiving a User object rendered back to json
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> Get([FromServices] INodeServices nodeServices, int userId)
        {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);

            User usr = await db.Get<User>("users", userId);

            return new JsonResult(usr);
        }

        ///<summary>
        /// MVC action for doing a loki update of a User and receiving back the same loki object with $loki/Id set.
        ///</summary>
        public async Task<IActionResult> Update([FromServices] INodeServices nodeServices, User user) {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);

            User result = null;

            if (user.Id == 0) {
                result = await db.Insert<User>("users", user);
            }
            else {
                result = await db.Update<User>("users", user);
            }

            return new JsonResult(result);
        }

        ///<summary>
        /// MVC action for doing a loki remove of a User.
        ///</summary>
        public async Task<IActionResult> Remove([FromServices] INodeServices nodeServices, int userId) {
            LokiDatabase db = new LokiDatabase(nodeServices, _demoServiceConfiguration);
            
            string result = await db.Remove("users", userId);

            return new JsonResult(result);
        }
        
    }

}