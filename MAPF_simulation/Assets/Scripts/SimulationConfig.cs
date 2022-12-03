using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MAPF {
    [CreateAssetMenu(menuName = "MAPF/SimulationConfig")]
    public class SimulationConfig : ScriptableObject {

        [Header("Graphic Config")]
        public bool _needGraphics = true;
        public float _delayBetweenPasses = 1f;

        [Header("Files to Load")]
        public string _mapJsonFileName = null;
        public string _robotJsonFileName = null;
        public string _taskSetJsonFileName = null;
    }
}