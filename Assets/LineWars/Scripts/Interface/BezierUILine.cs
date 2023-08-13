using System;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class BezierUILine : MaskableGraphic
    {
        [SerializeField, HideInInspector] private Vector3[] verts;
        [SerializeField, HideInInspector] private int[] tris;

        public void Redraw(Vector3[] verts, int[] tris)
        {
            this.verts = verts;
            this.tris = tris;
            SetAllDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (verts == null || tris == null) return;
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            foreach (var vert in verts)
            {
                vertex.position = vert;
                vh.AddVert(vertex);
            }

            for (int i = 0; i < tris.Length; i += 3)
            {
                vh.AddTriangle(tris[i], tris[i + 1], tris[i + 2]);
            }
        }
    }
}