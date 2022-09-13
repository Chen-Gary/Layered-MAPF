using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAPF {
    public class GlobalGrid {
        public MapUnitEntity[,] gridMap;
        public RobotEntity[,] gridRobot;

        public int dimX = 0;
        public int dimY = 0;

        public GlobalGrid(int dimX_, int dimY_) {
            this.dimX = dimX_;
            this.dimY = dimY_;

            gridMap = null;
            gridRobot = null;
        }

        #region Populate Grid
        private void _FillRectangleInGridMap(int xLowLeft, int yLowLeft, 
                                             int xOffset, int yOffset,
                                             MapUnitEntity.MapUnitType type) {
            if (xOffset <= 0 || yOffset <= 0) {
                Debug.LogError("[GlobalGrid] offset should > 0");
                return;
            }
            // error check skipped, we assume the specified rectangle is contained in the grid map

            for (int xDelta = 0; xDelta < xOffset; xDelta++) {
                for (int yDelta = 0; yDelta < yOffset; yDelta++) {
                    this.gridMap[xLowLeft + xDelta, yLowLeft + yDelta].type = type;
                }
            }
        }

        /// <summary>
        /// Dimension: 20 * 15
        ///     TODO: Dimension 76 * 48
        /// </summary>
        public void Populate_debug_v1() {
            if (dimX != 20 || dimY != 15) {
                Debug.LogError("[GlobalGrid] dimension mismatch when populate the map");
                return;
            }
            gridMap = new MapUnitEntity[dimX, dimY];
            gridRobot = new RobotEntity[dimX, dimY];

            // init
            for (int x = 0; x < dimX; x++) {
                for (int y = 0; y < dimY; y++) {
                    this.gridMap[x, y] = new MapUnitEntity(MapUnitEntity.MapUnitType.PUBLIC_ROAD);
                    this.gridRobot[x, y] = new RobotEntity(RobotEntity.RobotType.NONE);
                }
            }

            // set grid map
            _FillRectangleInGridMap(0, 0, dimX, 1, MapUnitEntity.MapUnitType.BARRIER);
            _FillRectangleInGridMap(0, dimY - 1, dimX, 1, MapUnitEntity.MapUnitType.BARRIER);
            _FillRectangleInGridMap(0, 0, 1, dimY, MapUnitEntity.MapUnitType.BARRIER);
            _FillRectangleInGridMap(dimX - 1, 0, 1, dimY, MapUnitEntity.MapUnitType.BARRIER);

            _FillRectangleInGridMap(3, 3, 4, 9, MapUnitEntity.MapUnitType.GOODS_AREA);
            _FillRectangleInGridMap(3 + 1, 3 + 1, 4 - 2, 9 - 2, MapUnitEntity.MapUnitType.GOODS_SHELF);

            _FillRectangleInGridMap(3 + 4 + 2, 3, 4, 9, MapUnitEntity.MapUnitType.GOODS_AREA);
            _FillRectangleInGridMap(3 + 4 + 2 + 1, 3 + 1, 4 - 2, 9 - 2, MapUnitEntity.MapUnitType.GOODS_SHELF);

            _FillRectangleInGridMap(16, 2, 3, 11, MapUnitEntity.MapUnitType.CONVEYOR);

            // set gird robot
            var xFetchRobotCoord = new int[] { 1, 1, 1, 7, 7, 7, 13, 13, 13 };
            var yFetchRobotCoord = new int[] { 11, 8, 3, 11, 8, 3, 11, 8, 3 };
            for (int i = 0; i < xFetchRobotCoord.Length; i++) {
                gridRobot[xFetchRobotCoord[i], yFetchRobotCoord[i]].type = RobotEntity.RobotType.FETCH;
            }

            var xFreightRobotCoord = new int[] { 2, 2, 2, 8, 8, 8, 15, 15, 15 };
            var yFreightRobotCoord = new int[] { 11, 8, 3, 11, 8, 3, 11, 8, 3 };
            for (int i = 0; i < xFreightRobotCoord.Length; i++) {
                gridRobot[xFreightRobotCoord[i], yFreightRobotCoord[i]].type = RobotEntity.RobotType.FREIGHT;
            }
        }
        #endregion
    }
}