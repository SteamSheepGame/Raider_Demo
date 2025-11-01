using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Demo.Core
{
    public class StartGameButton : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public Button TypeButton;
        public UIManager Manager;


        private void Awake()
        {
            TypeButton.onClick.AddListener(OpenScenel);
        }

        public void OpenScenel()
        {
            SceneManager.LoadScene("AlexTestScene");

        }
    }
}
