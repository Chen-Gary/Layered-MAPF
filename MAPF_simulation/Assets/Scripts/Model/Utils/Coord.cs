using System;
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

        public static Coord operator *(Coord coord, int scalar) {
            return new Coord(coord.x * scalar, coord.y * scalar);
        }

        public static Coord operator *(int scalar, Coord coord) {
            return new Coord(coord.x * scalar, coord.y * scalar);
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
            int manhattanDistance = Math.Abs(l.x - r.x) + Math.Abs(l.y - r.y);
            return manhattanDistance;
        }

        public static double EuclideanDistance(Coord l, Coord r) {
            double differenceX = l.x - r.x;
            double differenceY = l.y - r.y;
            double distance = Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceY, 2));
            return distance;
        }

        public static double GaussianDistribution(double x, double mean = 0, double standard_deviation = 1) {
            // ref: https://www.probabilitycourse.com/chapter4/4_2_3_normal.php
            //      https://en.wikipedia.org/wiki/Normal_distribution
            double power = -0.5 * Math.Pow( (x-mean)/standard_deviation, 2);
            double pdf = 1 / (standard_deviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(power);
            return pdf;
        }

        public static bool IsOnAxisAndPerpendicular(Coord l, Coord r) {
            bool case1 = l.x == 0 && l.y != 0 && r.x != 0 && r.y == 0;
            bool case2 = l.x != 0 && l.y == 0 && r.x == 0 && r.y != 0;
            return case1 || case2;
        }

        public static bool IsOnAxisAndPerpendicular(UnitDirection d1, UnitDirection d2) {
            Coord l = UnitDirection2DeltaCoord(d1);
            Coord r = UnitDirection2DeltaCoord(d2);
            return IsOnAxisAndPerpendicular(l, r);
        }

        public override string ToString() {
            return string.Format("({0}, {1})", x.ToString(), y.ToString());
        }
    }
}