using Newtonsoft.Json;

namespace LokiAspnetCore.Classes {
    public class LokiMeta {
        [JsonProperty("revision")]
        public int Revision { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }
    }

    public class LokiStatistics {
        [JsonProperty("start")]
        public long Start { get; set; }

        public decimal NodeUptime { get; set; }
        public string Platform { get; set; }
        public string Arch { get; set; }
        public string ExecPath { get; set; }

        [JsonProperty("cpuUsage")]
        public string CpuUsage { get; set; }

        [JsonProperty("cpuUsageUser")]
        public long CpuUsageUser { get; set; }

        [JsonProperty("cpuUsageSystem")]
        public long CpuUsageSystem { get; set; }


        [JsonProperty("memoryUsage")]
        public LokiMemoryUsage MemoryUsage { get; set; }

        [JsonProperty("requestStats")]
        public LokiRequestStats RequestStats { get; set; }
        
        [JsonProperty("processVersions")]
        public NodeProcessVersions ProcessVersions { get; set; }

        [JsonProperty("instanceNames")]
        public LokiInstanceName[] InstanceNames { get; set; }

    }

    public class LokiMemoryUsage {
        [JsonProperty("rss")]
       public int Rss { get; set; } 

        [JsonProperty("rssText")]
       public string RssText { get; set; }

        [JsonProperty("heapTotal")]
       public int HeapTotal { get; set; }

        [JsonProperty("heapTotalText")]
       public string HeapTotalText{ get; set; }

        [JsonProperty("heapUsed")]
       public int HeapUsed { get; set; }

        [JsonProperty("heapUsedText")]
       public string HeapUsedText { get; set; }
    }

    public class LokiRequestStats {
        [JsonProperty("totalRequests")]
        public int TotalRequests { get; set; }

        [JsonProperty("totalTime")]
        public decimal TotalTime { get; set; }

        [JsonProperty("getRequests")]
        public int GetRequests { get; set; }

        [JsonProperty("getTime")]
        public decimal GetTime { get; set; }

        [JsonProperty("findRequests")]
        public int FindRequests { get; set; }

        [JsonProperty("findTime")]
        public decimal FindTime { get; set; }

        [JsonProperty("transformRequests")]
        public int TransformRequests { get; set; }

        [JsonProperty("transformTime")]
        public decimal TransformTime { get; set; }

        [JsonProperty("dynamicViewRequests")]
        public int DynamicViewRequests { get; set; }

        [JsonProperty("dynamicViewTime")]
        public decimal DynamicViewTime { get; set; }

        [JsonProperty("insertRequests")]
        public int InsertRequests { get; set; }

        [JsonProperty("insertTime")]
        public decimal InsertTime { get; set; }

        [JsonProperty("updateRequests")]
        public int UpdateRequests { get; set; }

        [JsonProperty("updateTime")]
        public decimal UpdateTime { get; set; }

        [JsonProperty("removeRequests")]
        public int RemoveRequests { get; set; }

        [JsonProperty("removeTime")]
        public decimal RemoveTime { get; set; }

    }

    public class NodeProcessVersions {
        public string http_parser { get; set; }

        public string node { get; set; }

        public string v8 { get; set; }

        public string uv { get; set; }

        public string zlib { get; set; }

        public string ares { get; set; }

        public string icu { get; set; }

        public string modules { get; set; }

        public string openssl { get; set; }
    }

    public class LokiInstanceName {
        public string InitializerName { get; set; }
        public string InstanceName { get; set; }
    }

    public class LokiInstanceStats {
        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("autosave")]
        public bool Autosave { get; set; }

        [JsonProperty("autosaveInterval")]
        public int AutosaveInterval { get; set; }

        [JsonProperty("databaseVersion")]
        public decimal DatabaseVersion { get; set; }

        [JsonProperty("throttledSaves")]
        public bool ThrottledSaves { get; set; }

        [JsonProperty("requestStats")]
        public LokiRequestStats RequestStats { get; set; }

        [JsonProperty("collectionInfo")]
        public LokiCollectionInfo[] CollectionInfo { get; set; }
    }

    public class LokiCollectionInfo {
        public string Name { get; set; }
        public int Count { get; set; }
        public bool Dirty { get; set; }
        public bool Clone { get; set; }

        public bool AdaptiveBinaryIndices { get; set; }
        public string[] BinaryIndices { get; set; }
        public string[] UniqueIndices { get; set; }

        public string[] Transforms { get; set; }
        public string[] DynamicViews { get; set; }

    }
}