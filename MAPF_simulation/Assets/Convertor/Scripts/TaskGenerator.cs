using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Newtonsoft.Json;


namespace MAPF.Convertor {
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "MAPF/Convertor/TaskGenerator")]
    public class TaskGenerator : ScriptableObject {
        [SerializeField]
        private string _mapJsonFileName = null;
        [SerializeField]
        private string _outputName = null;
        [SerializeField]
        private int _taskCount = 0;

        public void Generate() {
            string pathIn = Path.Combine(Application.dataPath, "Convertor", "json", _mapJsonFileName + ".json");
            string pathOut = Path.Combine(Application.dataPath, "Convertor", "task_set", _outputName + ".json");
            string gridMapJson = File.ReadAllText(pathIn);
            int[,] gridMapIntArr = JsonConvert.DeserializeObject<int[,]>(gridMapJson);

            int dimX = gridMapIntArr.GetLength(0);
            int dimY = gridMapIntArr.GetLength(1);
            int[,] gridMap = new int[dimX, dimY];

            List<List<int>> listOfPos = new List<List<int>>();
            for (int x = 0; x < dimX; x++) {
                for (int y = 0; y < dimY; y++) {
                    if (gridMapIntArr[x, y] == 0) {     // `MapUnitEntity.canEnter`
                        listOfPos.Add(new List<int> { x, y });
                    }
                }
            }

            List<List<int>> listOfTask = new List<List<int>>();
            for (int i = 0; i < _taskCount; i++) {
                int randIndex = Random.Range(0, listOfPos.Count);
                listOfTask.Add(listOfPos[randIndex]);
            }
            int[][] arrOfTask = listOfTask.Select(pos => pos.ToArray()).ToArray();
            var arrOfTaskJson = JsonConvert.SerializeObject(arrOfTask);
            File.WriteAllText(pathOut, arrOfTaskJson);

            Debug.Log($"<color=#00FF00>All nice done!</color>");
        }
    }

    [CustomEditor(typeof(TaskGenerator))]
    public class TaskGeneratorInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            TaskGenerator generator = (TaskGenerator)target;
            if (GUILayout.Button("Generate")) {
                generator.Generate();
            }
        }
    }
#endif
}