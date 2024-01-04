// ReSharper disable Unity.InefficientPropertyAccess
// ReSharper disable CheckNamespace
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using Object = UnityEngine.Object;


namespace GraphEditor
{
    public abstract class GraphTool : EditorTool
    {
        private Edge monoEdgePrefab;
        private Node monoNodePrefab;
        private MonoGraph graph;
        
        private SelectionListener<Node> nodeListener;

        protected abstract Node GetNodePrefab();
        protected abstract Edge GetEdgePrefab();

        public override void OnActivated()
        {
            base.OnActivated();

            monoEdgePrefab = GetEdgePrefab();
            monoNodePrefab = GetNodePrefab();

            AssignGraph();
            

            foreach (var gameObject in FindObjectsOfType<GameObject>())
                SceneVisibilityManager.instance.DisablePicking(gameObject, false);

            SceneVisibilityManager.instance.EnablePicking(graph.gameObject, false);
            SceneVisibilityManager.instance.EnablePicking(graph.NodesParent.gameObject, true);

            EditorApplication.RepaintHierarchyWindow();

            nodeListener = new SelectionListener<Node>();
        }
        

        public override void OnWillBeDeactivated()
        {
            base.OnWillBeDeactivated();
            OnDisable();
        }

        private void OnDisable()
        {
            foreach (var gameObject in FindObjectsOfType<GameObject>())
                SceneVisibilityManager.instance.EnablePicking(gameObject, false);

            EditorApplication.RepaintHierarchyWindow();
        }

        public override void OnToolGUI(EditorWindow window)
        {
            UsePositionHandle();
            if (Event.current.Equals(Event.KeyboardEvent("k")))
            {
                PutNodeInMousePosition();
            }
            else if (Event.current.Equals(Event.KeyboardEvent("delete")))
            {
                DeleteSelectedNodes();
            }
        }

        private void AssignGraph()
        {
            var graphObj = GameObject.Find("Graph") ?? new GameObject("Graph");
            graph = graphObj.GetComponent<MonoGraph>() ??
                    graphObj.AddComponent<MonoGraph>();

            if (graph.NodesParent == null)
            {
                graph.NodesParent = new GameObject("Nodes");
                graph.NodesParent.transform.SetParent(graph.transform);
            }

            if (graph.EdgesParent == null)
            {
                graph.EdgesParent = new GameObject("Edges");
                graph.EdgesParent.transform.SetParent(graph.transform);
            }
        }

        private void DeleteSelectedNodes()
        {
            var allDeletedEdges = new List<Edge>();
            var allDeletedNodes = new List<Node>();
            var allNeighboringNodes = new List<Node>();

            foreach (var node in nodeListener.GetActive().ToArray())
            {
                allDeletedNodes.Add(node);
                allNeighboringNodes.AddRange(node.GetNeighbors());
                allDeletedEdges.AddRange(node.Edges);
            }

            allDeletedEdges = allDeletedEdges.Distinct().ToList();
            allNeighboringNodes = allNeighboringNodes.Distinct().ToList();

            Undo.IncrementCurrentGroup();
            foreach (var node in allNeighboringNodes)
            {
                Undo.RecordObject(node, "Delete Node");
                var myDeleteEdges = node.Edges.Intersect(allDeletedEdges).ToArray();
                foreach (var deleteEdge in myDeleteEdges)
                    node.RemoveEdge(deleteEdge);
                EditorUtility.SetDirty(node);
            }

            foreach (var node in allDeletedNodes)
            {
                Undo.RecordObject(node, "Delete Node");
                var myDeleteEdges = node.Edges.Intersect(allDeletedEdges).ToArray();
                foreach (var deleteEdge in myDeleteEdges)
                    node.RemoveEdge(deleteEdge);
            }

            foreach (var deletedEdge in allDeletedEdges)
            {
                Undo.RecordObject(deletedEdge, "Delete Node");
                deletedEdge.FirstNode = null;
                deletedEdge.SecondNode = null;
            }

            foreach (var node in allDeletedNodes)
            {
                Undo.DestroyObjectImmediate(node.gameObject);
            }

            foreach (var deletedEdge in allDeletedEdges)
            {
                Undo.DestroyObjectImmediate(deletedEdge.gameObject);
            }
        }

        private void PutNodeInMousePosition()
        {
            var activeNodes = nodeListener.GetActive().ToArray();
            switch (activeNodes.Length)
            {
                case 0:
                    CreateNode();
                    break;
                case 1:
                    var newNode = CreateNode();
                    ConnectNodes(newNode, activeNodes[0]);
                    break;
                case 2:
                    ConnectOrDisconnectNodes(activeNodes[0], activeNodes[1]);
                    break;
                case > 2:
                    Debug.LogError("Too many nodes");
                    break;
            }
        }

        private void ConnectOrDisconnectNodes(Node firstMonoNode, Node secondMonoNode)
        {
            var intersect = GetIntersectEdges(firstMonoNode, secondMonoNode);
            if (intersect.Count == 0)
                ConnectNodes(firstMonoNode, secondMonoNode);
            else
                DisconnectNodes(firstMonoNode, secondMonoNode, intersect);
        }

        private Edge ConnectNodes(Node firstMonoNode, Node secondMonoNode)
        {
            Undo.IncrementCurrentGroup();

            var edge = CreateEdge();
            edge.Initialize(GetNextIndex(edge), firstMonoNode, secondMonoNode);


            Undo.RecordObject(firstMonoNode, "ConnectNodes");
            firstMonoNode.AddEdge(edge);
            Undo.RecordObject(secondMonoNode, "ConnectNodes");
            secondMonoNode.AddEdge(edge);

            EditorUtility.SetDirty(firstMonoNode);
            EditorUtility.SetDirty(secondMonoNode);
            EditorUtility.SetDirty(edge);
            RedrawEdgeEditor(edge);
            return edge;
        }

        private Edge CreateEdge()
        {
            var edge = (Edge) PrefabUtility.InstantiatePrefab(monoEdgePrefab, graph.EdgesParent.transform);
            Undo.RegisterCreatedObjectUndo(edge.gameObject, "CreateEdge");
            SceneVisibilityManager.instance.DisablePicking(edge.gameObject, false);
            return edge;
        }

        private void DisconnectNodes(Node firstMonoNode, Node secondMonoNode, List<Edge> intersect)
        {
            Undo.IncrementCurrentGroup();
            Undo.RecordObject(firstMonoNode, "DisconnectNodes");
            Undo.RecordObject(secondMonoNode, "DisconnectNodes");

            foreach (var edge in intersect)
            {
                firstMonoNode.RemoveEdge(edge);
                secondMonoNode.RemoveEdge(edge);
                Undo.DestroyObjectImmediate(edge.gameObject);
            }

            EditorUtility.SetDirty(firstMonoNode);
            EditorUtility.SetDirty(secondMonoNode);
        }

        private Node CreateNode()
        {
            Undo.IncrementCurrentGroup();

            var node = (Node) PrefabUtility.InstantiatePrefab(monoNodePrefab, graph.NodesParent.transform);
            node.transform.position = GetMousePosition2D();
            node.Initialize(GetNextIndex(node));
            node.Redraw();
            Selection.activeObject = node.gameObject;

            Undo.RegisterCreatedObjectUndo(node.gameObject, "CreateNode");
            EditorUtility.SetDirty(node);

            return node;
        }

        private void UsePositionHandle()
        {
            if (target is GameObject activeObj)
            {
                if (activeObj.GetComponent<Node>() == null) return;

                EditorGUI.BeginChangeCheck();
                var oldPos = activeObj.transform.position;
                var newPos = Handles.PositionHandle(oldPos, Quaternion.identity);
                var offset = newPos - oldPos;
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (var node in targets
                                 .OfType<GameObject>()
                                 .GetComponentMany<Node>()
                            )
                    {
                        Undo.RecordObject(node.transform, "Move Node");
                        node.transform.position += offset;
                        ReDrawEdges(node);
                    }
                }
            }
        }

        private void ReDrawEdges(Node node)
        {
            foreach (var edge in node.Edges)
            {
                RedrawEdgeEditor(edge);
            }
        }


        private List<Edge> GetIntersectEdges(Node firstMonoNode, Node secondMonoNode)
        {
            return firstMonoNode.Edges
                .Intersect(secondMonoNode.Edges)
                .ToList();
        }

        private Vector2 GetMousePosition2D()
        {
            var mousePos = Event.current.mousePosition;
            var mouseX = mousePos.x;
            var mouseY = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
            var coord = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0));
            return coord;
        }

        private int GetNextIndex<T>(T obj) where T : Object, INumbered
        {
            var objects = FindObjectsOfType<T>()
                .Where(x => x != obj)
                .OrderBy(x => x.Id);
            var nextIndex = 0;
            foreach (var o in objects)
            {
                if (o.Id == nextIndex)
                    nextIndex++;
                else
                    return nextIndex;
            }

            return nextIndex;
        }

        private void RedrawEdgeEditor(Edge monoEdge)
        {
            Undo.RecordObject(monoEdge.gameObject, "Redraw Edge");
            Undo.RecordObject(monoEdge.SpriteRenderer.transform, "Redraw Edge");
            Undo.RecordObject(monoEdge.SpriteRenderer, "Redraw Edge");
            Undo.RecordObject(monoEdge.BoxCollider2D, "Redraw Edge");
            
            monoEdge.Redraw();
        }
    }
}