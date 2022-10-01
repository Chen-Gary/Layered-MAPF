using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;


namespace MAPF.Convertor {
    [CreateAssetMenu(menuName = "Convertor/GridMapConvertor")]
    public class GridMapConvertor : ScriptableObject {
        [SerializeField]
        private string CsvFileName = null;

        public void Convert() {
            string pathIn = Path.Combine(Application.dataPath, "Convertor", "csv", CsvFileName + ".csv");
            string pathOut = Path.Combine(Application.dataPath, "Convertor", "json", CsvFileName + ".json");
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

            Debug.Log($"<color=#00FF00>All nice done!</color>");
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