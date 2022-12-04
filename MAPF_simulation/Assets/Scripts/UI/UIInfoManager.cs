using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MAPF.UI {
    /// <summary>
    /// Combines View and ViewModel
    /// </summary>
    public class UIInfoManager : MonoBehaviour {

        public static UIInfoManager instance;     //singleton

        [SerializeField]
        private TextMeshProUGUI _timeStampText = null;
        [SerializeField]
        private ScrollRect _logScrollRect = null;
        [SerializeField]
        private TextMeshProUGUI _logText = null;

        public void Render(int updatedTimeStamp) {
            // view
            _timeStampText.text = _FormatTimeStamp(updatedTimeStamp);
        }

        private string _FormatTimeStamp(int currentTimeStamp) {
            return string.Format("Time Stamp: {0}", currentTimeStamp.ToString());
        }

        #region UI Log
        public void UILog(string msg) {
            //Debug.Log(msg);

            if (_logText.text.Length > 1200) _logText.text = _logText.text.Substring(0, 400);
            _logText.text = msg + "\n\n" + _logText.text;
            _logScrollRect.verticalNormalizedPosition = 1f;     //keep on top
        }

        public void UILogSuccess(string msg) {
            msg = "<color=#198754>" + msg + "</color>";
            UILog(msg);
        }
        #endregion

        #region Unity Callbacks
        private void Awake() {
            if (instance != null && instance != this) {
                Debug.LogError("[UIInfoManager] more than one UIInfoManager instance created");
                Destroy(this);
            } else {
                instance = this;
            }
        }

        private void Start() {
            _logText.text = "=== Log Console ===";
        }
        #endregion
    }
}