using GraphEditor;
using LineWars.Model;
using UnityEditor.EditorTools;
using UnityEngine;

namespace LineWarsEditor
{
    [EditorTool("Create Graph")]
    public class GraphTool: GraphToolBase<Node, Edge, MonoGraph>
    {
        protected override Node GetNodePrefab()
        {
            return Resources.Load<Node>("Prefabs/Node");
        }

        protected override Edge GetEdgePrefab()
        {
            return Resources.Load<Edge>("Prefabs/Edge");
        }
    }
}