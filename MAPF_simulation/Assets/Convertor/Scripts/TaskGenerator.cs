using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;


namespace MAPF.Convertor {
    [CreateAssetMenu(menuName = "Convertor/TaskGenerator")]
    public class TaskGenerator : ScriptableObject {
        [SerializeField]
        private string _mapCsvFileName = null;
        [SerializeField]
        private string _outputName = null;
        [SerializeField]
        private int _taskCount = 0;

        public void Generate() {
            string pathIn = Path.Combine(Application.dataPath, "Convertor", "csv", _mapCsvFileName + ".csv");
            string pathOut = Path.Combine(Application.dataPath, "Convertor", "task_set", _outputName + ".json");
            string[] lines = File.ReadAllLines(pathIn);

            int dimY = lines.Length;                //row index -> Y
            int dimX = lines[0].Split(',').Length;  //column index -> X
            int[,] gridMap = new int[dimX, dimY];

            List<List<int>> listOfPos = new List<List<int>>();
            for (int j = 0; j < dimY; j++) {
                string[] columns = lines[j].Split(',');
                for (int i = 0; i < dimX; i++) {
                    if (columns[i] == string.Empty) {
                        listOfPos.Add(new List<int> { i, j });
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
}