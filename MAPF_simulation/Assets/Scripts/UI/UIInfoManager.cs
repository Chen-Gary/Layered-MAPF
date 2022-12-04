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
        [SerializeField]
        private TextMeshProUGUI _robotCount = null;
        [SerializeField]
        private TextMeshProUGUI _mapSize = null;
        [SerializeField]
        private TextMeshProUGUI _totalTaskCount = null;
        [SerializeField]
        private TextMeshProUGUI _notAssignedTaskCount = null;
        [SerializeField]
        private TextMeshProUGUI _runningTaskCount = null;
        [SerializeField]
        private TextMeshProUGUI _finishedTaskCount = null;

        private int bufTotalTaskCount = 0;

        #region Render Repeatedly
        public void RenderTimeStamp(int updatedTimeStamp) {
            _timeStampText.text = string.Format("Time Stamp: {0}", updatedTimeStamp.ToString());
        }
        public void RenderTaskInfo(int notAssignedTaskCount, int finishedTaskCount) {
            int runningTaskCount = bufTotalTaskCount - notAssignedTaskCount - finishedTaskCount;

            _notAssignedTaskCount.text = string.Format("Tasks to be Assigned: {0}", notAssignedTaskCount.ToString());
            _runningTaskCount.text = string.Format("Tasks Under Execution: {0}", runningTaskCount.ToString());
            _finishedTaskCount.text = string.Format("Tasks Finished: {0}", finishedTaskCount.ToString());
        }
        #endregion

        #region Render Only Once
        public void RenderMapSize(int dimX, int dimY) {
            _mapSize.text = string.Format("Map Size: {0} x {1}", dimX.ToString(), dimY.ToString());
        }
        public void RenderRobotCount(int count) {
            _robotCount.text = string.Format("Robot Number: {0}", count.ToString());
        }
        public void RenderTotalTaskCount(int count) {
            this.bufTotalTaskCount = count;
            _totalTaskCount.text = string.Format("Total Task Number: {0}", count.ToString());
        }
        #endregion

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