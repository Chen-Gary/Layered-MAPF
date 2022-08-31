using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationEntry : MonoBehaviour {

    [SerializeField]
    private GlobalGridView _globalGridView = null;

    private GlobalGrid m_globalGrid;

    private void Start() {
        //m_globalGrid = new GlobalGrid(76, 48);
        m_globalGrid = new GlobalGrid(20, 15);
        _globalGridView.Render(m_globalGrid);
    }
}
