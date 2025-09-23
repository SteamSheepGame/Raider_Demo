using Newtonsoft.Json.Linq;

namespace Demo.Core
{
    public class LoadedData
    {
        private readonly string _entityTag;
        
        public string Path { get; private set; }
        
        public JProperty EntityContainer { get; private set; }
        
        public string EntityTag => _entityTag.ToLower();

        public LoadedData(string path, JProperty container, string tag)
        {
            Path = path;
            EntityContainer = container;
            _entityTag = tag;
        }
    }
}