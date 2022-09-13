using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MAPF.UI {
    /// <summary>
    /// Combines View and ViewModel
    /// </summary>
    public class UIInfoManager : MonoBehaviour {

        [SerializeField]
        private TextMeshProUGUI _timeStampText = null;

        private int m_currentTimeStamp = 0;

        public void Render(int deltaTimeStamp) {
            // viewModel part
            m_currentTimeStamp += deltaTimeStamp;

            // view
            _timeStampText.text = _FormatTimeStamp(m_currentTimeStamp);
        }

        private string _FormatTimeStamp(int currentTimeStamp) {
            return string.Format("Time Stamp: {0}", currentTimeStamp.ToString());
        }
    }
}