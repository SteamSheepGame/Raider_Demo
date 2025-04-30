namespace Demo.Core
{
    public interface IJsonService : IService
    {
        public void SaveJson<T>(string fileName, T data) where T : new();
        
        public T LoadJson<T>(string fileName) where T : new();
    }
}
