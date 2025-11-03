using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
namespace Demo.Core
{
    public class SettingPopupManager : MonoBehaviour
    {
        public TMP_Dropdown graphicDropdown;


        public void ChangeGraphicQuality()
        {
            QualitySettings.SetQualityLevel(graphicDropdown.value);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
