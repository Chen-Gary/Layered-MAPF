using System.Collections;
using System.Collections.Generic;

namespace MAPF {
    public class MapUnitEntity {
        public enum MapUnitType {
            NONE,
            GOODS_SHELF, 
            GOODS_AREA, 
            CONVEYOR, 
            PUBLIC_ROAD,
            BARRIER
        }

        public MapUnitType type { get; set; }
        public bool canEnter {
            get {
                return this.type == MapUnitType.PUBLIC_ROAD;
            }
        }

        public MapUnitEntity(MapUnitType type = MapUnitType.NONE) {
            this.type = type;
        }
    }
}