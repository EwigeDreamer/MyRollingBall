using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VisualizeColliders : MonoBehaviour
{
#if UNITY_EDITOR
    List<Collider> cols = new List<Collider>();

    private void OnDrawGizmos()
    {
        this.cols.Clear();
        var matrix = transform.localToWorldMatrix;
        GetComponents(this.cols);
        foreach (var col in this.cols) DrawCollider(col, matrix);
    }

    void DrawCollider(Collider col, Matrix4x4 tr_matrix)
    {
        var color = Gizmos.color;
        var matrix = Gizmos.matrix;
        Gizmos.color = col.enabled ? new Color32(145, 244, 139, 192) : new Color32(145, 244, 139, 92);
        Gizmos.matrix = tr_matrix;
        
        if (col is SphereCollider sphere)
        {
            var center = sphere.center;
            var radius = sphere.radius;
            Gizmos.DrawWireSphere(center, radius);
        }
        else if (col is BoxCollider box)
        {
            var center = box.center;
            var size = box.size;
            Gizmos.DrawWireCube(center, size);
        }

        Gizmos.color = color;
        Gizmos.matrix = matrix;
    }
#endif
}
