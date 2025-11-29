using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace Demo.Core
{
    public class GUIButtonControl: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private string viewName;
        // public GameObject TypePanel;
        public Button TypeButton;
        public UIManager Manager;
        public bool isEnabled = false;
        
        private void Awake()
        {
            TypeButton.onClick.AddListener(OpenTypePanel);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Button btn = TypeButton.GetComponent<Button>();
            // btn.onClick.AddListener(OpenTypePanel);
        }

        public void OpenTypePanel() 
        {
            if(isEnabled == false)
            {
                // DisableAllOtherButtons(TypeButton);
                Manager.ShowPopup(viewName);
                isEnabled = true;
            }
            else 
            {
                EnableAllOtherButtons(TypeButton);
                Manager.ClosePopup(viewName);
                isEnabled = false;
            }

        }
        
        // Todo:: Cache all button in UIManager, use a function to get cached buttons
        public void DisableAllOtherButtons(Button clickedButton)
        {
            Button[] allButtons = FindObjectsOfType<Button>(); 

            foreach (Button x in allButtons)
            {
                if (x != clickedButton) 
                {
                    x.interactable = false;
                }
            }
        }
        
        // Todo:: Cache all button in UIManager, use a function to get cached buttons
        public void EnableAllOtherButtons(Button clickedButton)
        {
            Button[] allButtons = FindObjectsOfType<Button>(); 

            foreach (Button x in allButtons)
            {
                x.interactable = true;
            }
        }
    }
}
