using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;


namespace MAPF.Convertor {
    [CreateAssetMenu(menuName = "MAPF/Convertor/GridMapConvertor")]
    public class GridMapConvertor : ScriptableObject {
        [SerializeField]
        private string MapCsvFileName = null;
        [SerializeField]
        private string RobotCsvFileName = null;

        public void Convert() {
            ConvertMap();
            ConvertRobotPos();

            Debug.Log($"<color=#00FF00>All nice done!</color>");
        }

        private void ConvertMap() {
            string pathIn = Path.Combine(Application.dataPath, "Convertor", "csv", MapCsvFileName + ".csv");
            string pathOut = Path.Combine(Application.dataPath, "Convertor", "json", MapCsvFileName + ".json");
            string[] lines = File.ReadAllLines(pathIn);

            int dimY = lines.Length;                //row index -> Y
            int dimX = lines[0].Split(',').Length;  //column index -> X
            int[,] gridMap = new int[dimX, dimY];

            for (int j = 0; j < dimY; j++) {
                string[] columns = lines[j].Split(',');
                if (columns.Length != dimX) {
                    Debug.LogWarning(string.Format("[GridMapConvertor] `columns.Length != dimY` in line {0}", j.ToString()));
                }
                for (int i = 0; i < dimX; i++) {
                    if (columns[i] == "1") {
                        gridMap[i, j] = 1;
                    } else if (columns[i] == string.Empty) {
                        gridMap[i, j] = 0;
                    }
                }
            }
            var gridMapJson = JsonConvert.SerializeObject(gridMap);
            File.WriteAllText(pathOut, gridMapJson);
        }

        private void ConvertRobotPos() {
            string pathIn = Path.Combine(Application.dataPath, "Convertor", "csv", RobotCsvFileName + ".csv");
            string pathOut = Path.Combine(Application.dataPath, "Convertor", "json", RobotCsvFileName + ".json");
            string[] lines = File.ReadAllLines(pathIn);

            int maxY = lines.Length;
            int maxX = lines[0].Split(',').Length;

            List<List<int>> listOfPos = new List<List<int>>();
            for (int j = 0; j < maxY; j++) {
                string[] columns = lines[j].Split(',');
                for (int i = 0; i < maxX; i++) {
                    if (columns[i] == "R") {
                        listOfPos.Add(new List<int> { i, j });
                    }
                }
            }
            int[][] arrOfPos = listOfPos.Select(pos => pos.ToArray()).ToArray();
            var arrOfPosJson = JsonConvert.SerializeObject(arrOfPos);
            File.WriteAllText(pathOut, arrOfPosJson);
        }
    }


    [CustomEditor(typeof(GridMapConvertor))]
    public class GridMapConvertorInspector : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GridMapConvertor convertor = (GridMapConvertor)target;
            if (GUILayout.Button("Convert")) {
                convertor.Convert();
            }
        }
    }
}