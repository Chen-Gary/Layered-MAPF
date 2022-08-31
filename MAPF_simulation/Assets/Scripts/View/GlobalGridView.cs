using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGridView : MonoBehaviour {
    [SerializeField]
    private GridEntityView _prefab = null;
    [SerializeField]
    private Transform _container = null;

    private bool m_hasInit = false;
    private GlobalGrid m_globalGrid;
    private GridEntityView[,] m_gridEntityViews;

    public void Render(GlobalGrid globalGrid_) {
        if (globalGrid_ == null) {
            Debug.LogError("[GlobalGridView] globalGrid_ == null");
            return; 
        }
        this.m_globalGrid = globalGrid_;

        _InitIfNot();
    }

    private void _InitIfNot() {
        if (m_hasInit) { return; }
        m_hasInit = true;

        if (m_globalGrid == null) {
            Debug.LogError("[GlobalGridView] m_globalGrid == null, when init");
            return;
        }

        // instantiate all `GridEntityView`
        m_gridEntityViews = new GridEntityView[m_globalGrid.dimX, m_globalGrid.dimY];
        for (int x = 0; x < m_globalGrid.dimX; x++) {
            for (int y = 0; y < m_globalGrid.dimY; y++) {
                m_gridEntityViews[x, y] = Instantiate(_prefab, _container);
                m_gridEntityViews[x, y].transform.position = new Vector3(x * GridEntityView.CELL_SIZE, 0f, y * GridEntityView.CELL_SIZE);
                m_gridEntityViews[x, y]._coordText.text = GridEntityView.FormatCoordText(x, y);
            }
        }
    }
}
