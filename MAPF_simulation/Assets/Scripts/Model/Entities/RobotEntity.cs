using System.Collections;
using System.Collections.Generic;

namespace MAPF {
    public class RobotEntity {
        public enum RobotType {
            NONE,
            FETCH,
            FREIGHT
        }

        public RobotType type { get; set; }

        public RobotEntity(RobotType type = RobotType.NONE) {
            this.type = type;
        }
    }
}