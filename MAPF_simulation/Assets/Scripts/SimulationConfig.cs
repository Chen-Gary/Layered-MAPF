using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MAPF {
    [CreateAssetMenu(menuName = "MAPF/SimulationConfig")]
    public class SimulationConfig : ScriptableObject {

        #region Enum
        public enum GlobalHM {
            NoHeatmap, Naive, TShape
        }

        public enum LocalHM {
            Naive, InverseProportion, Piecewise
        }
        #endregion

        [Header("Graphic Config")]
        public bool _needGraphics = true;
        public float _delayBetweenPasses = 1f;

        [Header("Files to Load")]
        public string _mapJsonFileName = null;
        public string _robotJsonFileName = null;
        public string _taskSetJsonFileName = null;

        [Header("Global Heatmap Config")]
        public GlobalHM _globalHeatmapAlgorithm = GlobalHM.NoHeatmap;
        public float _Naive_weight = 6f;

        [Header("Local Heatmap Config")]
        public LocalHM _localHeatmapAlgorithm = LocalHM.Naive;
    }
}