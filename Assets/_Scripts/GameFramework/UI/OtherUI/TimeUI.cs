using System;
using UnityEngine;
using TMPro;

namespace Demo.Core
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        private void Update()
        {
            var time = TimeSystem.Instance.GetGameTime();
            timeText.text = time.ToString(); 
        }
    }
}