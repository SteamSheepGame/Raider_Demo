using UnityEngine;

namespace Demo.Core
{
    public class CompendiumTest : MonoBehaviour
    {
        void Start()
        {
            ICompendiumService service = new CompendiumService("content");
            ServiceProvider.Instance.RegisterService<ICompendiumService>(service);
        }
        
        
    }
}