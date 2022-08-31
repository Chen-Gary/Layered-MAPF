using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridEntityView : MonoBehaviour
{
    #region Const
    public static readonly float CELL_SIZE = 1f;
    #endregion

    [SerializeField]
    public TextMeshPro _coordText = null;

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

    [SerializeField]
    private SpriteRenderer _fetchRobot = null;
    [SerializeField]
    private SpriteRenderer _freightRobot = null;

    public static string FormatCoordText(int x, int y) {
        return string.Format("({0},{1})", x.ToString(), y.ToString());
    }

    public void Render() {

    }
}
