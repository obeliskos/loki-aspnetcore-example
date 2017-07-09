using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.NodeServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace LokiAspnetCore.Classes {
    public class LokiDatabaseConfiguration {
        public string ServicePath { get; set; }

        public string ServiceInitPath { get; set; }

        public string DatabaseInstancePath { get; set; }

        public LokiDatabaseConfiguration() {
        }

        public LokiDatabaseConfiguration(string servicePath, string serviceInitPath, string databaseInstancePath="") {
            this.ServicePath = servicePath;
            this.ServiceInitPath = serviceInitPath;
            this.DatabaseInstancePath = databaseInstancePath;
        }
    }

    public class LokiDatabase {
        protected INodeServices _nodeServices;
        protected LokiDatabaseConfiguration _config;

        ///<summary>
        /// DotNetCore wrapper class for LokiJS NodeServices database instance.
        ///
        /// LokiService will maintain state across requests and it can have multiple databases loaded. 
        /// Database structure (collections, indexing, autosave intervals, transform and dynamic view definitions)
        ///   are defined within an initializer.
        /// You may have multiple instances of each defined service initializer, each uniquely referenced by 
        ///   the dbInstancePath.
        ///</summary>
        ///<param name="nodeServices">Reference to your controller's dependency injected INodeServices reference</param>
        ///<param name="servicePath">Relative path to the lokiservice.js service</param>
        ///<param name="serviceInitPath">Relative path to your service initializer module</param>
        ///<param name="dbInstancePath">Relative path to where your database instance will be stored.</param>
        ///<remarks>
        ///Intellisense comments generation and consumption are not fully supported within current vscode so this is mostly 
        // for full Visual Studio and documentation support.
        ///</remarks>
        public LokiDatabase(INodeServices nodeServices, string servicePath, string serviceInitPath, string dbInstancePath) {
            this._nodeServices = nodeServices;

            this._config = new LokiDatabaseConfiguration {
                ServicePath = servicePath, 
                ServiceInitPath = serviceInitPath, 
                DatabaseInstancePath = dbInstancePath 
            };
        }

        public LokiDatabase(INodeServices nodeServices, LokiDatabaseConfiguration configuration) {
            this._nodeServices = nodeServices;
            this._config = configuration;
        }

        public async Task<T> Get<T>(string collectionName, int id) {
            var nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "get", _config.ServiceInitPath, 
                _config.DatabaseInstancePath, collectionName, id);

            T result = JsonConvert.DeserializeObject<T>(nodeResponse);

            return result;
        }

        public async Task<string> Get(string collectionName, int id) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "get", _config.ServiceInitPath, 
                _config.DatabaseInstancePath, collectionName, id);

            return nodeResponse;
        }

        // For type conversion, your object must define 'JsonProperty' attributes to account for 
        // differences between json and c# object property names.
        public async Task<List<T>> Find<T>(string collectionName, string queryObject = null) {
            var nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "find", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, queryObject);
            
            List<T> typedObjects = JsonConvert.DeserializeObject<List<T>>(nodeResponse);

            return typedObjects;            
        }

        public async Task<string> Find(string collectionName, string queryObject = null) {
            var nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "find", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, queryObject);
            
            return nodeResponse;            
        }

        public async Task<T> Insert<T>(string collectionName, T document) {
            string documentJson = JsonConvert.SerializeObject(document);

            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "insert", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, documentJson);

            // response should return the inserted document with Id and Meta applied
            T result = JsonConvert.DeserializeObject<T>(nodeResponse);

            return result;
        }

        public async Task<string> Insert(string collectionName, string objectJson) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "insert", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, objectJson);

            return nodeResponse;
        }

        public async Task<T> Update<T>(string collectionName, T document) {
            string documentJson = JsonConvert.SerializeObject(document);
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "update", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, documentJson);

            T result = JsonConvert.DeserializeObject<T>(nodeResponse);

            return result;
        }

        public async Task<string> Update(string collectionName, string objectJson) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "update", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, objectJson);

            return nodeResponse;
        }

        public async Task<string> Remove(string collectionName, int id) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "remove", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, id);

            return nodeResponse;
        }

        // For type conversion, your object must define 'JsonProperty' attributes to account for 
        // differences between json and c# object property names.
        public async Task<List<T>> Transform<T>(string collectionName, string transformName, string transformParams = null, bool dataInvoke = true) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "transform", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, transformName, transformParams, dataInvoke);

            List<T> typedObjects = JsonConvert.DeserializeObject<List<T>>(nodeResponse);

            return typedObjects;
        }

        public async Task<string> Transform(string collectionName, string transformName, string transformParams = null, bool dataInvoke = true) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "transform", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, transformName, transformParams, dataInvoke);

            return nodeResponse;
        }

        public async Task<List<T>> DynamicView<T>(string collectionName, string dynamicViewName, string transformName = null) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "dynamicView", _config.ServiceInitPath,
                _config.DatabaseInstancePath, collectionName, dynamicViewName, transformName);

            List<T> typedObjects = JsonConvert.DeserializeObject<List<T>>(nodeResponse);

            return typedObjects;
        }

        public LokiQueryChain DynamicViewChain(string collectionName, string dynamicViewName, string transformName = null, dynamic transformParams = null) {
            return new LokiQueryChain(_nodeServices, _config, collectionName, dynamicViewName, transformName, transformParams);
        }

        //
        // Server-side chaining is implemented using transforms via the c# 'LokiQueryChain' class
        //
        public LokiQueryChain Chain(string collectionName) {
            return new LokiQueryChain(_nodeServices, _config, collectionName);
        }

        public async Task<LokiStatistics> GetStatistics() {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "stats", _config.ServiceInitPath,
                _config.DatabaseInstancePath);

            LokiStatistics stats = JsonConvert.DeserializeObject<LokiStatistics>(nodeResponse);

            return stats;
        }
        
        public async Task<string> GetStatisticsJson() {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "stats", _config.ServiceInitPath,
                _config.DatabaseInstancePath);

            return nodeResponse;
        }

        public async Task<LokiInstanceStats> GetInstanceStatistics(string initializer, string instancePath) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "instanceStats", initializer, instancePath);

            LokiInstanceStats stats = JsonConvert.DeserializeObject<LokiInstanceStats>(nodeResponse);

            return stats;
        }
        
        public async Task<string>  GetInstanceStatisticsJson(string initializer, string instancePath) {
            string nodeResponse = await _nodeServices.InvokeExportAsync<string>(_config.ServicePath, "instanceStats", initializer, instancePath);

            return nodeResponse;
        }

    }
}