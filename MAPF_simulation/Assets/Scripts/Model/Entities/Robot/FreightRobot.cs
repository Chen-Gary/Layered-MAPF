using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAPF {
    public class FreightRobot : RobotEntity {

        private int priority;
        private Queue<Task> assignedTasks;

        public FreightRobot(int priority_) : base(RobotType.FREIGHT) {
            priority = priority_;
            assignedTasks = new Queue<Task>();
        }

        public void Operate() {
            // fetch job from cloud
            if (assignedTasks.Count == 0) {
                Task newTask;
                if (!GlobalGrid._instance.RequestTask(out newTask)) {
                    Debug.Log(string.Format("[RobotEntity] Robot[{0}] has not more job to perform", priority.ToString()));
                    return;     //return if no more task available
                }
            }

            // plan path
        }
    }
}