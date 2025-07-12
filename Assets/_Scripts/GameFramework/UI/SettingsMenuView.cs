using UnityEngine;

using System.Collections.Generic;

using System.Collections;

using UnityEngine.UI;

namespace Demo.Core
{
    ///Test
    public class SettingsMenuView : View
    {
        [SerializeField] private Button _backButton;
        
        public override void Initialize()
        {
            _backButton.onClick.AddListener(() => UIManager.ShowLast());
        }
    }
}
