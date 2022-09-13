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

        #region GlobalGrid Constructor
        private void _FillRectangleInGridMap(int xLowLeft, int yLowLeft, int xOffset, int yOffset,
                                             ref MapUnitEntity[,] gridMap, MapUnitEntity.MapUnitType type) {
            if (xOffset <= 0 || yOffset <= 0) {
                Debug.LogError("[SimulationEntry] offset should > 0");
                return;
            }
            // error check skipped, we assume the specified rectangle is contained in the grid map

            for (int xDelta = 0; xDelta < xOffset; xDelta++) {
                for (int yDelta = 0; yDelta < yOffset; yDelta++) {
                    gridMap[xLowLeft + xDelta, yLowLeft + yDelta].type = type;
                }
            }
        }

        private GlobalGrid _ConstructGlobalGrid_debug1() {
            //GlobalGrid myGlobalGrid = new GlobalGrid(76, 48);
            GlobalGrid myGlobalGrid = new GlobalGrid(20, 15);

            // init
            for (int x = 0; x < myGlobalGrid.dimX; x++) {
                for (int y = 0; y < myGlobalGrid.dimY; y++) {
                    myGlobalGrid.gridMap[x, y] = new MapUnitEntity(MapUnitEntity.MapUnitType.PUBLIC_ROAD);
                    myGlobalGrid.gridRobot[x, y] = new RobotEntity(RobotEntity.RobotType.NONE);
                }
            }

            // set grid map
            _FillRectangleInGridMap(0, 0, myGlobalGrid.dimX, 1, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.BARRIER);
            _FillRectangleInGridMap(0, myGlobalGrid.dimY-1, myGlobalGrid.dimX, 1, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.BARRIER);
            _FillRectangleInGridMap(0, 0, 1, myGlobalGrid.dimY, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.BARRIER);
            _FillRectangleInGridMap(myGlobalGrid.dimX-1, 0, 1, myGlobalGrid.dimY, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.BARRIER);

            _FillRectangleInGridMap(3, 3, 4, 9, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.GOODS_AREA);
            _FillRectangleInGridMap(3+1, 3+1, 4-2, 9-2, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.GOODS_SHELF);

            _FillRectangleInGridMap(3+4+2, 3, 4, 9, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.GOODS_AREA);
            _FillRectangleInGridMap(3+4+2+1, 3+1, 4-2, 9-2, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.GOODS_SHELF);

            _FillRectangleInGridMap(16, 2, 3, 11, ref myGlobalGrid.gridMap, MapUnitEntity.MapUnitType.CONVEYOR);

            // set gird robot
            var xFetchRobotCoord = new int[] {  1, 1, 1,  7, 7, 7, 13, 13, 13 };
            var yFetchRobotCoord = new int[] { 11, 8, 3, 11, 8, 3, 11,  8,  3 };
            for (int i = 0; i < xFetchRobotCoord.Length; i++) {
                myGlobalGrid.gridRobot[xFetchRobotCoord[i], yFetchRobotCoord[i]].type = RobotEntity.RobotType.FETCH;
            }

            var xFreightRobotCoord = new int[] { 2, 2, 2,  8, 8, 8, 15, 15, 15 };
            var yFreightRobotCoord = new int[] {11, 8, 3, 11, 8, 3, 11,  8,  3 };
            for (int i = 0; i < xFreightRobotCoord.Length; i++) {
                myGlobalGrid.gridRobot[xFreightRobotCoord[i], yFreightRobotCoord[i]].type = RobotEntity.RobotType.FREIGHT;
            }

            return myGlobalGrid;
        }
        #endregion

        #region Unity Callbacks
        private void Start() {
            // construct `m_globalGrid`
            m_globalGrid = _ConstructGlobalGrid_debug1();

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