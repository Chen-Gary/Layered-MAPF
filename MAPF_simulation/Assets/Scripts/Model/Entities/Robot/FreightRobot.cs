using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAPF.Utils;


namespace MAPF {
    public class FreightRobot : RobotEntity {

        public int priority;
        public Coord.UnitDirection prevMove = Coord.UnitDirection.ZERO;

        private MapUnitEntity[,] gridMap;
        private SimulationConfig config;
        private Queue<Task> assignedTasks;

        public FreightRobot(Coord position_, int priority_) : base(RobotType.FREIGHT, position_) {
            if (GlobalGrid._instance == null)
                Debug.LogError("[FreightRobot] GlobalGrid._instance is null, when trying to instantiate FreightRobot");

            gridMap = GlobalGrid._instance.gridMap;
            config = SimulationEntry.instance._config;
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
            float[,] localHeatmap = GlobalGrid._instance.globalHeatmap.Clone() as float[,];
            switch (config._localHeatmapAlgorithm) {
                case SimulationConfig.LocalHM.Naive:
                    _LoopThroughEachSlot(localHeatmap, _DerivateLocalHeatmap_Naive);
                    break;
                case SimulationConfig.LocalHM.InverseProportion:
                    _LoopThroughEachSlot(localHeatmap, _DerivateLocalHeatmap_InverseProportion);
                    break;
                case SimulationConfig.LocalHM.Piecewise:
                    _LoopThroughEachSlot(localHeatmap, _DerivateLocalHeatmap_Piecewise);
                    break;
                default:
                    Debug.LogError("[FreightRobot] invalid _localHeatmapAlgorithm");
                    break;
            }
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
            //this.position = nextPos;  //let the cloud server do this

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
        private void _LoopThroughEachSlot(float[,] localHeatmap, Func<Coord, Coord, float, float> func) {
            int dimX = localHeatmap.GetLength(0);
            int dimY = localHeatmap.GetLength(1);

            for (int x = 0; x < dimX; x++) {
                for (int y = 0; y < dimY; y++) {
                    Coord thisCoord = new Coord(x, y);
                    localHeatmap[x, y] = func(this.position, thisCoord, localHeatmap[x, y]);
                }
            }
        }

        private float _DerivateLocalHeatmap_Naive(Coord robotPos, Coord slotPos, float originalHeat) {
            return originalHeat;
        }

        private float _DerivateLocalHeatmap_InverseProportion(Coord robotPos, Coord slotPos, float originalHeat) {
            float SCALE = 3f;
            int dist = Coord.ManhattanDistance(robotPos, slotPos);
            float newHeat = originalHeat * (SCALE / (float)dist);
            return newHeat;
        }

        private float _DerivateLocalHeatmap_Piecewise(Coord robotPos, Coord slotPos, float originalHeat) {
            int dist = Coord.ManhattanDistance(robotPos, slotPos);
            if (dist < 6)
                return originalHeat * 1f;
            else if (dist < 12)
                return originalHeat * 0.8f;
            else if (dist < 24)
                return originalHeat * 0.5f;
            else if (dist < 50)
                return originalHeat * 0.2f;
            else
                return originalHeat * 0f;
        }
        #endregion
    }
}