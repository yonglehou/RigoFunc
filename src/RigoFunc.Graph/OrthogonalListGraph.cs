// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    using System.Collections.Generic;

    /// <summary>
    /// Represents a graph which's internal storage is orthogonal list. This class cannot be inherited.
    /// </summary>
    public sealed class OrthogonalListGraph {
        private VertexEdgeFactory _factory;
        private List<Edge> _edgeList;
        private List<Vertex> _vertexList;
        private ITopologyManager _topologyManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrthogonalListGraph" /> class.
        /// </summary>
        /// <param name="topologyManager">The topology manager.</param>
        public OrthogonalListGraph(ITopologyManager topologyManager) {
            _topologyManager = topologyManager;

            _vertexList = new List<Vertex>();
            _edgeList = new List<Edge>();

            _factory = new VertexEdgeFactory();

            this.Update();
        }

        /// <summary>
        /// Updates the orthogonal list graph.
        /// </summary>
        public void Update() {
            this.Clear();

            var units = _topologyManager.GetUnits();

            // construct vertex
            foreach (var unit in units) {
                var v = _factory.NewVertex(unit);

                AddVertex(v);
            }

            // establish edge
            foreach (var unit in units) {
                var subsequents = _topologyManager.GetSubsequents(unit);
                if (subsequents != null) {
                    var outVertex = SearchVertex(unit);
                    foreach (var sub in subsequents) {
                        var inVertex = SearchVertex(sub);
                        if (inVertex != null) {
                            AddEdge(outVertex, inVertex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>The cloned OrthogonalListGraph.</returns>
        public OrthogonalListGraph Clone() {
            var graph = new OrthogonalListGraph(_topologyManager);
            foreach (var item in _vertexList) {
                graph._vertexList.Add(item);
            }
            foreach (var item in _edgeList) {
                graph._edgeList.Add(item);
            }

            return graph;
        }

        /// <summary>
        /// Adds a new <see cref="Vertex" /> to the graph and returns it.
        /// </summary>
        /// <param name="v">The vertex.</param>
        public void AddVertex(Vertex v) {
            if (!_vertexList.Contains(v)) {
                _vertexList.Add(v);
            }
        }

        /// <summary>
        /// Adds a new <see cref="Edge" /> with the out and in vertices to the graph and returns it.
        /// </summary>
        /// <param name="out">The out vertex to create edge.</param>
        /// <param name="in">The in vertex to create edge.</param>
        /// <returns>
        /// The new edge added to the graph.
        /// </returns>
        public Edge AddEdge(Vertex @out, Vertex @in) {
            bool exist = false;
            foreach (var edge in _edgeList) {
                if (edge.Out == @out && edge.In == @in) {
                    exist = true;
                    break;
                }
            }

            if (!exist) {
                // create edge
                var edge = _factory.NewEdge(@out, @in);

                _edgeList.Add(edge);

                return edge;
            }

            return null;
        }

        /// <summary>
        /// Removes the <see cref="Vertex" /> from the graph.
        /// </summary>
        /// <param name="v">The vertex.</param>
        public void RemoveVertex(Vertex v) {
            if (!_vertexList.Contains(v)) {
                _vertexList.Remove(v);
            }
        }

        /// <summary>
        /// Removes the specified edge from the graph.
        /// </summary>
        /// <param name="e">The edge to remove.</param>
        public void RemoveEdge(Edge e) {
            if (e != null) {
                e.In.InDegree--;
                e.Out.OutDegree--;

                var prev = e.Out.FirstOut;
                if (prev == e) {
                    e.Out.FirstOut = e.NextOut;
                }
                else {
                    while (prev.NextOut != e) {
                        prev = prev.NextOut;
                    }

                    prev.NextOut = e.NextOut;
                }

                prev = e.In.FirstIn;
                if (prev == e) {
                    prev.In.FirstIn = e.NextIn;
                }
                else {
                    while (prev.NextIn != e) {
                        prev = prev.NextIn;
                    }

                    prev.NextIn = e.NextIn;
                }

                _edgeList.Remove(e);
            }
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <value>The vertices.</value>
        public IEnumerable<Vertex> Vertices {
            get {
                return _vertexList;
            }
        }

        /// <summary>
        /// Gets the edges.
        /// </summary>
        /// <value>The edges.</value>
        public IEnumerable<Edge> Edges {
            get {
                return _edgeList;
            }
        }

        /// <summary>
        /// Gets the vertex count.
        /// </summary>
        /// <value>The vertex count.</value>
        public int VertexCount {
            get {
                return _vertexList.Count;
            }
        }

        /// <summary>
        /// Clears the vertices and edges.
        /// </summary>
        public void Clear() {
            _edgeList.Clear();
            _vertexList.Clear();
        }

        private Vertex SearchVertex(IUnit unit) {
            foreach (var v in Vertices) {
                if (v.Units.Contains(unit))
                    return v;
            }
            return null;
        }
    }
}
