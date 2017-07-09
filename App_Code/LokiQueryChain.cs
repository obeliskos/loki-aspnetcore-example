using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace LokiAspnetCore.Classes {
    public class LokiQueryChain {
        private INodeServices _nodeServices;
        private LokiDatabaseConfiguration _databaseConfiguration;
        private string _collection;

        private JArray _transformSteps = new JArray();

        private string _dynamicViewName = null;

        private string _transformName = null;

        private dynamic _transformParams = null;

        public LokiQueryChain(INodeServices nodeServices, LokiDatabaseConfiguration configuration, string collectionName) {
            _nodeServices = nodeServices;
            _databaseConfiguration = configuration;
            _collection = collectionName;
        }

        public LokiQueryChain(INodeServices nodeServices, LokiDatabaseConfiguration configuration, string collectionName,
            string dynamicViewName, string transformName = null, dynamic transformParams = null) {
            _nodeServices = nodeServices;
            _databaseConfiguration = configuration;
            _collection = collectionName;
            _dynamicViewName = dynamicViewName;
            _transformName = transformName;
            _transformParams = transformParams;
        }

        public LokiQueryChain Find(string queryObject) {
            JObject findStep = new JObject();
            findStep["type"] = "find";
            findStep["value"] = JObject.Parse(queryObject);
            _transformSteps.Add(findStep);

            return this;
        }

        public LokiQueryChain Offset(int offset) {
            dynamic step = new JObject();
            step.type = "offset";
            step.value = offset;

            _transformSteps.Add(step);

            return this;
        }

        public LokiQueryChain Limit(int limit) {
            dynamic step = new JObject();
            step.type = "limit";
            step.value = limit;

            _transformSteps.Add(step);

            return this;
        }

        public LokiQueryChain SimpleSort(string propertyName, bool descending = false) {
            dynamic step = new JObject();
            step.type = "simplesort";
            step.property = propertyName;
            step.desc = descending;

            _transformSteps.Add(step);

            return this;
        }

        public async Task<List<T>> Data<T>() {
            string tx = JsonConvert.SerializeObject(_transformSteps);

            string nodeResponse;

            // Collection Chaining
            if (_dynamicViewName == null) {
                nodeResponse = await _nodeServices.InvokeExportAsync<string>(_databaseConfiguration.ServicePath, "transformRaw", 
                    _databaseConfiguration.ServiceInitPath, _databaseConfiguration.DatabaseInstancePath, 
                    _collection, tx, null, true);
            }
            // Dynamic View (branchResultset) chaining
            else {
                //var txParamString = "";

                nodeResponse = await _nodeServices.InvokeExportAsync<string>(_databaseConfiguration.ServicePath, "dynamicViewTransform", 
                    _databaseConfiguration.ServiceInitPath, _databaseConfiguration.DatabaseInstancePath, 
                    _collection, _dynamicViewName, _transformName, _transformParams, tx);
            }

            List<T> typedObjects = JsonConvert.DeserializeObject<List<T>>(nodeResponse);

            return typedObjects;
        }
    }
}