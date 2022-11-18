using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAPF.Utils {
    public class Coord {
        public int x;
        public int y;

        public Coord(int x_, int y_) {
            x = x_;
            y = y_;
        }

        public static Coord operator +(Coord l, Coord r) {
            return new Coord(l.x + r.x, l.y + r.y);
        }

        public static bool operator ==(Coord l, Coord r) {
            return (l.x == r.x) && (l.y == r.y);
        }

        public static bool operator !=(Coord l, Coord r) {
            return !((l.x == r.x) && (l.y == r.y));
        }

        public static int ManhattanDistance(Coord l, Coord r) {
            int manhattanDistance = Mathf.Abs(l.x - r.x) + Mathf.Abs(l.y - r.y);
            return manhattanDistance;
        }

        public override string ToString() {
            return string.Format("({0}, {1})", x.ToString(), y.ToString());
        }
    }
}