using UnityEngine;

namespace Demo.Core
{
    public class HUDView : View
    {
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("[HUDView] Initialized");
        }
    }
}
