using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MAPF.UI;
using MAPF.View;

namespace MAPF { 
    public class SimulationEntry : MonoBehaviour {
        [SerializeField]
        private bool _needGraphics = true;

        [SerializeField]
        private UIInfoManager _uiInfoManager = null;
        [SerializeField]
        private GlobalGridView _globalGridView = null;

        [SerializeField]
        private string _mapJsonFileName = null;
        [SerializeField]
        private string _robotJsonFileName = null;
        [SerializeField]
        private string _taskSetJsonFileName = null;
        [SerializeField]
        private float _delayBetweenPasses = 1f;

        private int m_currentTimeStamp = 0;

        private GlobalGrid m_globalGrid;

        private bool m_keepSimulation = true;


        private bool _OneSimulationPass() {
            m_currentTimeStamp++;
            if (_needGraphics) 
                _uiInfoManager.Render(m_currentTimeStamp);

            // for all robot
            bool globalTerminationFlag = true;
            List<FreightRobot> robots = new List<FreightRobot>();
            for (int x = 0; x < m_globalGrid.dimX; x++) {
                for (int y = 0; y < m_globalGrid.dimY; y++) {
                    if (m_globalGrid.gridRobot[x, y].type == RobotEntity.RobotType.FREIGHT)
                        robots.Add((FreightRobot)m_globalGrid.gridRobot[x, y]);
                }
            }
            for (int i = 0; i < robots.Count; i++) {
                bool isCurrentRobotIdle;
                robots[i].Operate(out isCurrentRobotIdle);
                if (!isCurrentRobotIdle) globalTerminationFlag = false;
            }

            // rerender view
            if (_needGraphics)
                _globalGridView.Render(m_globalGrid);

            return globalTerminationFlag;
        }

        private IEnumerator _SimulationLoopCoroutine() {
            while (m_keepSimulation) {
                yield return new WaitForSeconds(_delayBetweenPasses);
                bool globalTerminated = _OneSimulationPass();

                if (globalTerminated) {
                    Debug.Log(string.Format("[SimulationEntry] simulation done, total time stamp: {0}", m_currentTimeStamp.ToString()));
                    break;
                }
            }
        }

        private void _SimulationLoop() {
            while (m_keepSimulation) {
                bool globalTerminated = _OneSimulationPass();

                if (globalTerminated) {
                    Debug.Log(string.Format("[SimulationEntry] simulation done, total time stamp: {0}", m_currentTimeStamp.ToString()));
                    break;
                }
            }
        }

        public void StartSimulation() {
            if (_needGraphics)
                StartCoroutine(_SimulationLoopCoroutine());
            else
                _SimulationLoop();
        }

        #region Unity Callbacks
        private void Start() {
            // construct `m_globalGrid`
            m_globalGrid = new GlobalGrid();
            m_globalGrid.PopulateMapWithJson(_mapJsonFileName);
            m_globalGrid.PopulateRobotWithJson(_robotJsonFileName);
            m_globalGrid.PopulateTaskQueueWithJson(_taskSetJsonFileName);

            // render
            _globalGridView.Render(m_globalGrid);
            _uiInfoManager.Render(m_currentTimeStamp);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                m_keepSimulation = false;
                Debug.Log("[SimulationEntry] manually trigger");
            }
        }
        #endregion
    }
}