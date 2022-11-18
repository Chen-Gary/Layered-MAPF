using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAPF.Utils;


namespace MAPF {
    public class Task {
        Coord targetPos;

        public Task(int x_, int y_) {
            targetPos = new Coord(x_, y_);
        }
    }
}