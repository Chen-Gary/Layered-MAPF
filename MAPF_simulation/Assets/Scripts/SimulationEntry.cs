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
        [SerializeField]
        private string _mapJsonFileName = null;
        [SerializeField]
        private string _robotJsonFileName = null;
        [SerializeField]
        private string _taskSetJsonFileName = null;

        private GlobalGrid m_globalGrid;

        private bool m_keepSimulation = true;


        private void _OneSimulationPass() {
            _uiInfoManager.Render(1);

            // for all robot
            List<FreightRobot> robots = new List<FreightRobot>();
            for (int x = 0; x < m_globalGrid.dimX; x++) {
                for (int y = 0; y < m_globalGrid.dimY; y++) {
                    if (m_globalGrid.gridRobot[x, y].type == RobotEntity.RobotType.FREIGHT)
                        robots.Add((FreightRobot)m_globalGrid.gridRobot[x, y]);
                }
            }
            for (int i = 0; i < robots.Count; i++) {
                robots[i].Operate();
            }

            // rerender view
            _globalGridView.Render(m_globalGrid);
        }

        private IEnumerator _SimulationLoop() {
            while(m_keepSimulation) {
                yield return new WaitForSeconds(0.5f);
                _OneSimulationPass();
            }
        }

        #region Unity Callbacks
        private void Start() {
            // construct `m_globalGrid`
            m_globalGrid = new GlobalGrid();
            //m_globalGrid.Populate_debug_v1();
            m_globalGrid.PopulateMapWithJson(_mapJsonFileName);
            m_globalGrid.PopulateRobotWithJson(_robotJsonFileName);
            m_globalGrid.PopulateTaskQueueWithJson(_taskSetJsonFileName);

            // render
            _globalGridView.Render(m_globalGrid);
            _uiInfoManager.Render(0);


            // start simulaation
            StartCoroutine(_SimulationLoop());
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