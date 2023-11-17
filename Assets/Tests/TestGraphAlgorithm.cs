// using System.Collections.Generic;
// using LineWars.Model;
// using NUnit.Framework;
// using UnityEngine;
//
// namespace Tests
// {
//     public class TestGraphAlgorithm
//     {
//
//         class Graph: Graph<Node, Edge>
//         {
//             public Graph(IEnumerable<Node> nodes, IEnumerable<Edge> edges) : base(nodes, edges)
//             {
//             }
//         }
//         class Node: INode<Node, Edge> 
//         {
//             public IEnumerable<Edge> Edges{ get; set; }
//         }
//
//         class Edge: IEdge<Node, Edge>
//         {
//             public Node FirstNode { get; set; }
//             public Node SecondNode { get; set; }
//         }
//
//
//         [Test]
//         public void TestAdvancedBfs()
//         {
//         }
//     }
// }