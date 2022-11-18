using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAPF.Utils;

namespace MAPF {
    public class RobotEntity {
        public enum RobotType {
            NONE,
            FETCH,
            FREIGHT
        }

        public RobotType type { get; set; }
        public Coord position;      //should be consistent with index in `GlobalGrid.gridRobot`

        public RobotEntity(RobotType type_, Coord position_) {
            this.type = type_;
            this.position = position_;
        }

        public virtual void Operate() {
            //Debug.Log("[RobotEntity] Operate called and do nothing");
        }
    }
}