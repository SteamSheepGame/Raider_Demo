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
        public bool isEnabled = false;
        
        private GameObject ControlledPanel = null;
        
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
                // TODO: right now used for carddeck control, later consider change to not use popup for carddeck!
                if(ControlledPanel == null) ControlledPanel = UIManager.Instance.GetPanel(viewName);
                
                ControlledPanel.SetActive(true);
                
                isEnabled = true;
            }
            else 
            {
                EnableAllOtherButtons(TypeButton);
                
                if(ControlledPanel == null) ControlledPanel = UIManager.Instance.GetPanel(viewName);
                
                ControlledPanel.SetActive(false);
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
