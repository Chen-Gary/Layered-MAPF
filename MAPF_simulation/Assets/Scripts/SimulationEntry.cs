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
            m_globalGrid = new GlobalGrid();
            //m_globalGrid.Populate_debug_v1();
            m_globalGrid.PopulateMapWithJson("Map1");
            m_globalGrid.PopulateRobotWithJson("Bot1");

            // render
            _globalGridView.Render(m_globalGrid);
            _uiInfoManager.Render(0);

            //test A*
            //RobotEntity robot = new RobotEntity(RobotEntity.RobotType.FREIGHT);
            //Utils.AStar algorithm = new Utils.AStar(GlobalGrid._instance.gridMap);
            //algorithm.FindPath(new Utils.AStar.Coord(1, 5), new Utils.AStar.Coord(7, 8));
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                _uiInfoManager.Render(1);
                // rerender view
                _globalGridView.Render(m_globalGrid);
            }
        }
        #endregion
    }
}