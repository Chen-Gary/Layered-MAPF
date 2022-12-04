using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MAPF.View {
    public class GridEntityView : MonoBehaviour {
        #region Const
        public static readonly float CELL_SIZE = 1f;
        #endregion

        #region Inspector
        [SerializeField]
        public TextMeshPro _coordText = null;

        // map
        [SerializeField]
        private SpriteRenderer _goodsArea = null;
        [SerializeField]
        private SpriteRenderer _goodsShelf = null;
        [SerializeField]
        private SpriteRenderer _publicRoad = null;
        [SerializeField]
        private SpriteRenderer _barrier = null;
        [SerializeField]
        private SpriteRenderer _conveyor = null;

        // robot
        [SerializeField]
        private SpriteRenderer _fetchRobot = null;
        [SerializeField]
        private SpriteRenderer _freightRobot = null;
        #endregion

        public static string FormatCoordText(int x, int y) {
            return string.Format("({0},{1})", x.ToString(), y.ToString());
        }

        public void RenderMap(MapUnitEntity.MapUnitType type) {
            _goodsArea.gameObject.SetActive(type == MapUnitEntity.MapUnitType.GOODS_AREA);
            _goodsShelf.gameObject.SetActive(type == MapUnitEntity.MapUnitType.GOODS_SHELF);
            _publicRoad.gameObject.SetActive(type == MapUnitEntity.MapUnitType.PUBLIC_ROAD);
            _barrier.gameObject.SetActive(type == MapUnitEntity.MapUnitType.BARRIER);
            _conveyor.gameObject.SetActive(type == MapUnitEntity.MapUnitType.CONVEYOR);
        }

        public void RenderRobot(RobotEntity.RobotType type) {
            _fetchRobot.gameObject.SetActive(type == RobotEntity.RobotType.FETCH);
            _freightRobot.gameObject.SetActive(type == RobotEntity.RobotType.FREIGHT);
        }
        public void RenderRobot(RobotEntity robot) {
            _fetchRobot.gameObject.SetActive(robot.type == RobotEntity.RobotType.FETCH);
            _freightRobot.gameObject.SetActive(robot.type == RobotEntity.RobotType.FREIGHT);

            if (robot.type == RobotEntity.RobotType.FREIGHT) {
                FreightRobot freightRobot = (FreightRobot)robot;
                float SCALAR = 0.35f;    //limit rgbScale in [1-SCALAR, 1], so the color is lighter
                float rgbScale = 1f - SCALAR * (float)(50 * freightRobot.priority % 256) / 256f;
                _freightRobot.color = new Color(rgbScale, rgbScale, rgbScale);
            }
        }
    }
}