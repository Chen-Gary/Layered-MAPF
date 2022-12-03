using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAPF.Utils {
    public class Coord {
        public int x;
        public int y;

        public enum UnitDirection {
            ZERO,
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        public static Coord UnitDirection2DeltaCoord(UnitDirection direction) {
            if (direction == UnitDirection.ZERO)
                return new Coord(0, 0);
            else if (direction == UnitDirection.LEFT)
                return new Coord(-1, 0);
            else if (direction == UnitDirection.RIGHT)
                return new Coord(1, 0);
            else if (direction == UnitDirection.UP)
                return new Coord(0, 1);
            else if (direction == UnitDirection.DOWN)
                return new Coord(0, -1);
            else {
                Debug.LogError("[Coord] invalid direction");
                return new Coord(0, 0);
            }
        }

        public static UnitDirection DeltaCoord2UnitDirection(Coord deltaCoord) {
            if (deltaCoord == new Coord(0, 0))
                return UnitDirection.ZERO;
            else if (deltaCoord == new Coord(-1, 0))
                return UnitDirection.LEFT;
            else if (deltaCoord == new Coord(1, 0))
                return UnitDirection.RIGHT;
            else if (deltaCoord == new Coord(0, 1))
                return UnitDirection.UP;
            else if (deltaCoord == new Coord(0, -1))
                return UnitDirection.DOWN;
            else {
                Debug.LogError("[Coord] invalid deltaCoord");
                return UnitDirection.ZERO;
            }
        }


        public Coord(int x_, int y_) {
            x = x_;
            y = y_;
        }

        public static Coord operator +(Coord l, Coord r) {
            return new Coord(l.x + r.x, l.y + r.y);
        }

        public static Coord operator -(Coord l, Coord r) {
            return new Coord(l.x - r.x, l.y - r.y);
        }

        public static bool operator ==(Coord l, Coord r) {
            return (l.x == r.x) && (l.y == r.y);
        }

        public static bool operator !=(Coord l, Coord r) {
            return !((l.x == r.x) && (l.y == r.y));
        }

        public override bool Equals(object o) {
            if (o == null) return false;

            var right = o as Coord;
            return (this.x == right.x) && (this.y == right.y);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
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