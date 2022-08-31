using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnitEntity {
    public enum MapUnitType {
        NONE,
        GOODS_SHELF, 
        GOODS_AREA, 
        CONVEYOR, 
        PUBLIC_ROAD
    }

    public MapUnitType type;

    public MapUnitEntity(MapUnitType type = MapUnitType.NONE) {
        this.type = type;
    }
}
