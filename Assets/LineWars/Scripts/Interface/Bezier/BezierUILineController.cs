using PathCreation;
using UnityEngine;

namespace LineWars.Interface
{
    [RequireComponent(typeof(PathCreator))]
    public class BezierUILineController : PathSceneTool
    {
        [Header("Line Settings")] public float lineWidth = 1;

        [Header("Render settings")] public Color lineColor = Color.black;

        [SerializeField, HideInInspector] private BezierUILine line;

        protected override void PathUpdated()
        {
            if (pathCreator != null)
            {
                AssignMeshComponents();
                AssignRendersSettings();
                CreateLineMesh();
            }
        }

        private void CreateLineMesh()
        {
            var points = GetRelativeRectCoordinates();
            CreateAllVertexAndTris(points, out var verts, out var tris);
            line.Redraw(verts, tris);
        }

        void AssignMeshComponents()
        {
            if (line == null)
            {
                var lineObj = new GameObject("Bezier UI Line Holder");
                line = lineObj.AddComponent<BezierUILine>();
                if (transform.parent != null)
                    line.transform.SetParent(transform.parent);
            }

            line.transform.rotation = Quaternion.identity;
            line.transform.position = transform.position;

            line.transform.localScale = Vector3.one;

            if (!line.gameObject.GetComponent<BezierUILine>())
            {
                line.gameObject.AddComponent<BezierUILine>();
            }
        }

        void AssignRendersSettings()
        {
            line.color = lineColor;
        }

        private Vector2[] GetRelativeRectCoordinates()
        {
            var points = new Vector2[path.NumPoints];
            for (int i = 0; i < path.NumPoints; i++)
            {
                var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, path.GetPoint(i));
                RectTransformUtility.ScreenPointToLocalPointInRectangle(line.rectTransform, screenPoint, Camera.main,
                    out var localPoint);
                points[i] = localPoint;
            }

            return points;
        }

        private void CreateAllVertexAndTris(Vector2[] points, out Vector3[] verts, out int[] tris)
        {
            verts = new Vector3[points.Length * 2];
            tris = new int[2 * (points.Length - 1) * 3];
            int vertsIndex = 0;
            int trisIndex = 0;

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 forward = Vector2.zero;
                if (i < points.Length - 1)
                {
                    forward += points[i + 1] - points[i];
                }

                if (i > 0)
                {
                    forward += points[i] - points[i - 1];
                }

                forward.Normalize();

                Vector2 left = new Vector2(-forward.y, forward.x);
                Vector2 right = -left;

                // сместили точку относительно центра влево на половину ширины дороги
                verts[vertsIndex] = points[i] + left * lineWidth * 0.5f;
                // сместили точку относительно центра вправо на половину ширины дороги
                verts[vertsIndex + 1] = points[i] + right * lineWidth * 0.5f;

                // по два треугольника на каждую точку
                if (i < points.Length - 1)
                {
                    tris[trisIndex] = vertsIndex;
                    tris[trisIndex + 1] = vertsIndex + 2;
                    tris[trisIndex + 2] = vertsIndex + 1;

                    tris[trisIndex + 3] = vertsIndex + 1;
                    tris[trisIndex + 4] = vertsIndex + 2;
                    tris[trisIndex + 5] = vertsIndex + 3;
                }

                vertsIndex += 2;
                trisIndex += 6;
            }
        }
    }
}