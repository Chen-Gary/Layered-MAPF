using UnityEngine;
using UnityEditor;

namespace MAPF.CustomizedEditor { 
    /// <summary>
    /// 在3d项目中，如创建一个cube，在Inspector中并没有sorting layer和order in layer这两个字段。
    /// 如果需要更改排序，我们可以通过直接通过脚本写入；
    /// 也可以通过编写editor脚本将sorting layer和order in layer这两个字段在Inspector中显示出来，并进行设置。
    /// Copy from: https://blog.csdn.net/yq398934906/article/details/104881406
    /// </summary>
    [CustomEditor(typeof(MeshRenderer))]
    public class MeshRendererEditor : Editor {
        MeshRenderer meshRenderer;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            meshRenderer = target as MeshRenderer;

            string[] layerNames = new string[SortingLayer.layers.Length];
            for (int i = 0; i < SortingLayer.layers.Length; i++)
                layerNames[i] = SortingLayer.layers[i].name;

            int layerValue = SortingLayer.GetLayerValueFromID(meshRenderer.sortingLayerID);
            layerValue = EditorGUILayout.Popup("Sorting Layer", layerValue, layerNames);

            SortingLayer layer = SortingLayer.layers[layerValue];
            meshRenderer.sortingLayerName = layer.name;
            meshRenderer.sortingLayerID = layer.id;
            meshRenderer.sortingOrder = EditorGUILayout.IntField("Order in Layer", meshRenderer.sortingOrder);
        }
    }
}