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

            gridMap = new MapUnitEntity[dimX, dimY];
            gridRobot = new RobotEntity[dimX, dimY];
        }
    }
}