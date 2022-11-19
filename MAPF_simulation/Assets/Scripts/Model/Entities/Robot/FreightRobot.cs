using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAPF.Utils;


namespace MAPF {
    public class FreightRobot : RobotEntity {

        private MapUnitEntity[,] gridMap;
        public int priority;
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
                    //Debug.Log(string.Format("[RobotEntity] Robot[{0}] has not more job to perform", priority.ToString()));
                    isIdle = true;
                    return;     //return if no more task available
                }
                assignedTasks.Enqueue(newTask);
            }
            isIdle = false;

            // decide current job
            Task currentTask = assignedTasks.Peek();

            // plan path
            /*------------- avoid other robots -------------*/
            // very dirty implementation!!!
            MapUnitEntity[,] gridMapWithRobot = this.gridMap.Clone() as MapUnitEntity[,];
            RobotEntity[,] gridRobotBuf = GlobalGrid._instance.gridRobot;
            for (int x = 0; x < gridMapWithRobot.GetLength(0); x++) {
                for (int y = 0; y < gridMapWithRobot.GetLength(1); y++) {
                    if (gridRobotBuf[x, y].type == RobotType.FREIGHT) {
                        gridMapWithRobot[x, y] = new MapUnitEntity(MapUnitEntity.MapUnitType.BARRIER);
                    }
                }
            }
            AStar algorithm = new AStar(gridMapWithRobot);
            /*----------------------------------------------*/
            //AStar algorithm = new AStar(this.gridMap);
            List<Coord> path = algorithm.FindPath(this.position, currentTask.targetPos);
            if (path == null || path.Count <= 0) {
                Debug.LogError(string.Format("[FreightRobot] A star path planning failed, start{0} to target{1}",
                    this.position.ToString(), currentTask.targetPos.ToString()));
                return;
            }
            Coord nextStep = path[0];

            // actual move
            MoveToAdjacent(nextStep);
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
    }
}