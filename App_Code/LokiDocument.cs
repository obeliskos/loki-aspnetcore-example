using Newtonsoft.Json;

namespace LokiAspnetCore.Classes {
    public class LokiDocument {
        [JsonProperty("$loki")]
        public int Id { get; set; }

        [JsonProperty("meta")]
        public LokiMeta Meta { get; set; }
        
    }
}