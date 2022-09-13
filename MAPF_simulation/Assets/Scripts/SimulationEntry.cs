using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MAPF.UI;
using MAPF.View;

namespace MAPF { 
    public class SimulationEntry : MonoBehaviour {
        [SerializeField]
        private UIInfoManager _uiInfoManager = null;
        [SerializeField]
        private GlobalGridView _globalGridView = null;

        private GlobalGrid m_globalGrid;

        #region Unity Callbacks
        private void Start() {
            // construct `m_globalGrid`
            m_globalGrid = new GlobalGrid(20, 15);
            m_globalGrid.Populate_debug_v1();

            // render
            _globalGridView.Render(m_globalGrid);
            _uiInfoManager.Render(0);
        }

        private int debugRobotX = 1;
        private int debugRobotY = 11;
        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                _uiInfoManager.Render(1);

                // move robot
                m_globalGrid.gridRobot[debugRobotX, debugRobotY].type = RobotEntity.RobotType.NONE;
                debugRobotX++;
                m_globalGrid.gridRobot[debugRobotX, debugRobotY].type = RobotEntity.RobotType.FETCH;

                // rerender view
                _globalGridView.Render(m_globalGrid);
            }
        }
        #endregion
    }
}