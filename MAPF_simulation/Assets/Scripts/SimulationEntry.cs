using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MAPF.UI;
using MAPF.View;

namespace MAPF { 
    public class SimulationEntry : MonoBehaviour {

        public SimulationConfig _config;

        [Header("Static Reference")]
        [SerializeField]
        private UIInfoManager _uiInfoManager = null;
        [SerializeField]
        private GlobalGridView _globalGridView = null;


        public static SimulationEntry instance;     //singleton

        private int m_currentTimeStamp = 0;

        private GlobalGrid m_globalGrid;

        private bool m_keepSimulation = true;


        private bool _OneSimulationPass() {
            m_currentTimeStamp++;
            if (_config._needGraphics) 
                _uiInfoManager.RenderTimeStamp(m_currentTimeStamp);

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
            if (_config._needGraphics)
                _globalGridView.Render(m_globalGrid);

            return globalTerminationFlag;
        }

        private IEnumerator _SimulationLoopCoroutine() {
            while (m_keepSimulation) {
                yield return new WaitForSeconds(_config._delayBetweenPasses);
                bool globalTerminated = _OneSimulationPass();

                if (globalTerminated) {
                    UIInfoManager.instance.UILogSuccess/*Debug.Log*/(string.Format("[SimulationEntry] simulation done, total time stamp: {0}", m_currentTimeStamp.ToString()));
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
            if (_config._needGraphics)
                StartCoroutine(_SimulationLoopCoroutine());
            else
                _SimulationLoop();
        }

        #region Unity Callbacks
        private void Awake() {
            if (instance != null && instance != this) {
                Debug.LogError("[SimulationEntry] more than one SimulationEntry instance created");
                Destroy(this);
            } else {
                instance = this;
            }
        }

        private void Start() {
            // construct `m_globalGrid`
            m_globalGrid = new GlobalGrid();
            m_globalGrid.PopulateMapWithJson(_config._mapJsonFileName);
            m_globalGrid.PopulateRobotWithJson(_config._robotJsonFileName);
            m_globalGrid.PopulateTaskQueueWithJson(_config._taskSetJsonFileName);

            // render
            _globalGridView.Render(m_globalGrid);
            _uiInfoManager.RenderTimeStamp(m_currentTimeStamp);
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