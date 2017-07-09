using Newtonsoft.Json;
using System.Collections.Generic;

namespace LokiAspnetCore.Classes {
    public enum UserGender { Male=0, Female=1 }
    public enum AttributeType { String=0, Integer=1, Decimal=2, DateTime=3 }

    //public class UserAttribute {
    //    [JsonProperty("name")]
    //    public string Name { get; set; }
    //    public AttributeType AttrType { get; set; }
    //    public string StringVal { get; set; }
    //}

    public class User : LokiDocument {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("gender")]
        public UserGender Gender;

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}