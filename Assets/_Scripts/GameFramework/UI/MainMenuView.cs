using UnityEngine;

using System.Collections.Generic;

using System.Collections;

using UnityEngine.UI;

namespace Demo.Core
{

    ///Test
    public class MainMenuView : View
    {
        [SerializeField] private Button _settingButton;

        public override void Initialize()
        {
            _settingButton.onClick.AddListener(() => UIManager.Show<SettingsMenuView>());
        }
    }
}
