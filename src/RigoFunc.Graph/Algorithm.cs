// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides algorithms (such as DFS, BFS) for the graph.
    /// </summary>
    public class Algorithm {
        private OrthogonalListGraph _graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="Algorithm"/> class.
        /// </summary>
        /// <param name="graph">The orthogonal list graph.</param>
        public Algorithm(OrthogonalListGraph graph) {
            _graph = graph;
        }

        /// <summary>
        /// Performs the Depth First Search.
        /// </summary>
        public void PerformDFS() {
            PerformDFS(null);
        }

        /// <summary>
        /// Performs the Depth First Search start from specified vertex.
        /// </summary>
        /// <param name="s">The depth first search start vertex.</param>
        public void PerformDFS(Vertex s) {
            //  White marks vertices that have yet to be discovered.
            foreach (var u in _graph.Vertices) {
                u.Mark = VertexMarks.White;
            }

            // if there is a starting vertex, start from it:
            if (s != null) {
                VisitDFS(s);
            }

            // process each vertex
            foreach (var u in _graph.Vertices) {
                if (u.Mark == VertexMarks.White) {
                    VisitDFS(u);
                }
            }
        }

        /// <summary>
        /// Removes the back edges from the graph.
        /// </summary>
        public void RemoveBackEdges() {
            // remove all the back edges
            foreach (var edge in _graph.Edges) {
                if (edge.Type == EdgeType.BackEdge)
                    _graph.RemoveEdge(edge);
            }
        }

        /// <summary>
        /// Performs the Breadth First Search.
        /// </summary>
        public void PerformBFS() {
            PerformBFS(null);
        }

        /// <summary>
        /// Performs the Breadth First Search start from specified vertex.
        /// </summary>
        /// <param name="s">The s.</param>
        public void PerformBFS(Vertex s) {

        }

        /// <summary>
        /// Performs the topology sort.
        /// </summary>
        /// <returns>IList&lt;IUnit&gt;.</returns>
        public IList<IUnit> TopologySort() {
            return null;
        }

        private void VisitDFS(Vertex u) {
            if (u == null)
                throw new ArgumentNullException("u", "Vertex cannot be null!");

            // Gray marks a vertex that is discovered 
            // but still has vertices adjacent to it that are undiscovered.
            u.Mark = VertexMarks.Gray;

            Vertex v = null;

            for (var edge = u.FirstOut; edge != null; edge = edge.NextOut) {
                v = edge.In;
                if (v.Mark == VertexMarks.White) {
                    VisitDFS(v);
                    edge.Type = EdgeType.TreeEdge;
                }
                else if (v.Mark == VertexMarks.Gray) {
                    edge.Type = EdgeType.BackEdge;
                }
                else {
                    edge.Type = EdgeType.ForwardEdge;
                }
            }

            // Black marks vertex is discovered vertex that is not adjacent to any white vertices.
            u.Mark = VertexMarks.Black;
        }
    }
}
