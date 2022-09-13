using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public MapUnitEntity(MapUnitType type = MapUnitType.NONE) {
            this.type = type;
        }
    }
}