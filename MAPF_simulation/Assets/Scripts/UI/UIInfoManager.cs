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

        public void Render(int updatedTimeStamp) {
            // view
            _timeStampText.text = _FormatTimeStamp(updatedTimeStamp);
        }

        private string _FormatTimeStamp(int currentTimeStamp) {
            return string.Format("Time Stamp: {0}", currentTimeStamp.ToString());
        }
    }
}