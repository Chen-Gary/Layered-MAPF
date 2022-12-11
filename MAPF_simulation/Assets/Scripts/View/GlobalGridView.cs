using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAPF.View { 
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
            _RefreshView();
        }

        private void _InitIfNot() {
            if (m_hasInit) { return; }
            m_hasInit = true;

            // error check
            if (m_globalGrid == null) {
                Debug.LogError("[GlobalGridView] m_globalGrid == null, when init");
                return;
            }
            if (m_gridEntityViews != null) {
                Debug.LogError("[GlobalGridView] m_gridEntityViews != null, when init");
                return;
            }

            // init
            m_gridEntityViews = new GridEntityView[m_globalGrid.dimX, m_globalGrid.dimY];
            for (int x = 0; x < m_globalGrid.dimX; x++) {
                for (int y = 0; y < m_globalGrid.dimY; y++) {
                    // 1. instantiate each `GridEntityView`
                    m_gridEntityViews[x, y] = Instantiate(_prefab, _container);
                    m_gridEntityViews[x, y].transform.position = new Vector3(x * GridEntityView.CELL_SIZE, 0f, y * GridEntityView.CELL_SIZE);

                    // 2. Render "static" graphic for each `GridEntityView`
                    m_gridEntityViews[x, y]._coordText.text = GridEntityView.FormatCoordText(x, y);
                    m_gridEntityViews[x, y]._coordText.gameObject.SetActive(SimulationEntry.instance._config._displayCoordinate);
                    m_gridEntityViews[x, y].RenderMap(m_globalGrid.gridMap[x, y].type);
                    m_gridEntityViews[x, y].Enable3DView(SimulationEntry.instance._config._display3DModel);
                }
            }
        }

        private void _RefreshView() {
            for (int x = 0; x < m_globalGrid.dimX; x++) {
                for (int y = 0; y < m_globalGrid.dimY; y++) {
                    if (SimulationEntry.instance._config._renderMode == SimulationConfig.RenderMode.Robot) {
                        //m_gridEntityViews[x, y].RenderRobot(m_globalGrid.gridRobot[x, y].type);
                        m_gridEntityViews[x, y].RenderRobot(m_globalGrid.gridRobot[x, y]);
                    } else {    // SimulationEntry.instance._config._renderMode == SimulationConfig.RenderMode.Heatmap
                        m_gridEntityViews[x, y].RenderHeat(m_globalGrid.gridRobot[x, y], m_globalGrid.globalHeatmap[x, y]);
                    }
                }
            }
        }
    }
}