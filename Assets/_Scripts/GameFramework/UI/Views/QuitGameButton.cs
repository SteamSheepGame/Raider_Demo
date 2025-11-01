using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Demo.Core
{
    public class QuitGameButton : MonoBehaviour
    {
        public Button TypeButton;
        public UIManager Manager;


        private void Awake()
        {
            TypeButton.onClick.AddListener(doExitGame);
        }

        private void doExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }
    }
}
