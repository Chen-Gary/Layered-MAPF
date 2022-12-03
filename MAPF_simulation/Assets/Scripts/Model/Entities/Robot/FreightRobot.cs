﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAPF.Utils;


namespace MAPF {
    public class FreightRobot : RobotEntity {

        public int priority;
        public Coord.UnitDirection prevMove = Coord.UnitDirection.ZERO;

        private MapUnitEntity[,] gridMap;
        private Queue<Task> assignedTasks;

        public FreightRobot(Coord position_, int priority_) : base(RobotType.FREIGHT, position_) {
            if (GlobalGrid._instance == null)
                Debug.LogError("[FreightRobot] GlobalGrid._instance is null, when trying to instantiate FreightRobot");

            gridMap = GlobalGrid._instance.gridMap;
            priority = priority_;
            assignedTasks = new Queue<Task>();
        }

        public override void Operate(out bool isIdle) {
            // fetch job from cloud
            if (assignedTasks.Count == 0) {
                Task newTask;
                if (!GlobalGrid._instance.RequestTask(out newTask)) {
                    isIdle = true;
                    GlobalGrid._instance.RemoveRobot(this);     //idle robot will be removed
                    Debug.Log(string.Format("[RobotEntity] Robot[{0}] has not more job to perform, so it is removed", priority.ToString()));
                    return;     //return if no more task available
                }
                assignedTasks.Enqueue(newTask);
            }
            isIdle = false;

            // decide current job
            Task currentTask = assignedTasks.Peek();

            // plan path
            /*------------- avoid other robots -------------*/
            // check neighbors
            MapUnitEntity[,] gridMapWithRobot = this.gridMap.Clone() as MapUnitEntity[,];
            List<FreightRobot> neighbors = GetNeighborRobots();
            if (neighbors.Count >= 4) {
                prevMove = Coord.UnitDirection.ZERO;
                MoveToAdjacent(this.position);
                return; //just return and stay at current position
            } else {
                foreach(FreightRobot neighbor in neighbors) {
                    if (neighbor.position == currentTask.targetPos) {
                        prevMove = Coord.UnitDirection.ZERO;
                        MoveToAdjacent(this.position);
                        return; //just return and stay at current position
                    }
                    gridMapWithRobot[neighbor.position.x, neighbor.position.y] = new MapUnitEntity(MapUnitEntity.MapUnitType.BARRIER);
                }
            }
            /*----------------------------------------------*/

            /*------------- get local heatmap -------------*/
            float[,] localHeatmap = _DerivateLocalHeatmap_Naive(GlobalGrid._instance.globalHeatmap);
            /*----------------------------------------------*/


            AStar algorithm = new AStar(gridMapWithRobot, localHeatmap);
            List<Coord> path = algorithm.FindPath(this.position, currentTask.targetPos);
            if (path == null) {
                Debug.LogWarning(string.Format("[FreightRobot] A star path planning failed, start{0} to target{1}",
                    this.position.ToString(), currentTask.targetPos.ToString()));
                prevMove = Coord.UnitDirection.ZERO;
                MoveToAdjacent(this.position);
                return; //stay at current position
            }
            Coord nextStep = (path.Count == 0) ? this.position : path[0];
            prevMove = Coord.DeltaCoord2UnitDirection(nextStep - this.position);
            MoveToAdjacent(nextStep);
            //Debug.Log($"<color=#00FF00>" + "[FreightRobot] prevMove = </color>" + prevMove.ToString());
        }

        private List<FreightRobot> GetNeighborRobots() {
            List<FreightRobot> neighbors = new List<FreightRobot>();

            RobotEntity[,] gridRobotBuf = GlobalGrid._instance.gridRobot;
            RobotEntity robotRight = gridRobotBuf[position.x + 1, position.y];
            RobotEntity robotLeft = gridRobotBuf[position.x - 1, position.y];
            RobotEntity robotUp = gridRobotBuf[position.x, position.y + 1];
            RobotEntity robotDown = gridRobotBuf[position.x, position.y - 1];

            if (robotRight.type == RobotType.FREIGHT) neighbors.Add((FreightRobot)robotRight);
            if (robotLeft.type == RobotType.FREIGHT) neighbors.Add((FreightRobot)robotLeft);
            if (robotUp.type == RobotType.FREIGHT) neighbors.Add((FreightRobot)robotUp);
            if (robotDown.type == RobotType.FREIGHT) neighbors.Add((FreightRobot)robotDown);

            return neighbors;
        }

        private void MoveToAdjacent(Coord nextPos) {
            int manhattanDistance = Coord.ManhattanDistance(this.position, nextPos);
            if (manhattanDistance > 1) {
                Debug.LogWarning("[FreightRobot] invalid move");
                return;
            }

            // inform cloud (global gridRobot)
            bool updateAllowed = GlobalGrid._instance.UpdateRobotPos(this, nextPos);
            if (!updateAllowed) {
                Debug.LogWarning("[FreightRobot] update request rejected");
                return;
            }

            // update local position
            this.position = nextPos;

            // check if current task finished
            if (assignedTasks.Count > 0) {
                Task currentTask = assignedTasks.Peek();
                if (currentTask.targetPos == this.position) {
                    assignedTasks.Dequeue();
                    Debug.Log(string.Format("[FreightRobot] local task finished, targetPos={0}", this.position.ToString()));
                }
            }
            
        }

        #region Local Heatmap
        private float[,] _DerivateLocalHeatmap_Naive(float[,] globalHeatmap) {
            return globalHeatmap.Clone() as float[,];
        }
        #endregion
    }
}