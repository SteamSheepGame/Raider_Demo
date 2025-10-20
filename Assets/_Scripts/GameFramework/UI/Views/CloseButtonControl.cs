using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace Demo.Core
{
    public class CloseButtonControl: MonoBehaviour, IPointerDownHandler
    {
        public GameObject TypePanel;
        public Button TypeButton;
        public UIManager Manager;
        
        private void Awake()
        {
            TypeButton.onClick.AddListener(CloseTypePanel);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Button btn = TypeButton.GetComponent<Button>();
            // btn.onClick.AddListener(CloseTypePanel);
        }

        public void CloseTypePanel() 
        {
            Manager.ClosePopup(TypePanel.name);
            EnableAllOtherButtons(TypeButton);
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
