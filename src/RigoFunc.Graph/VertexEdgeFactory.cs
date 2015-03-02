// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    /// <summary>
    /// Provides methods to create <see cref="Vertex"/> and <see cref="Edge"/>.
    /// </summary>
    internal class VertexEdgeFactory {
        /// <summary>
        /// Creates a new vertex with the specified unit.
        /// </summary>
        /// <param name="unit">The unit will be associated with vertex.</param>
        /// <returns>
        /// A new instance of vertex.
        /// </returns>
        public Vertex NewVertex(IUnit unit) {
            return new Vertex(unit);
        }

        /// <summary>
        /// Creates a new edge with the two vertices.
        /// </summary>
        /// <param name="out">The out vertex.</param>
        /// <param name="in">The in vertex.</param>
        /// <returns>A new instance of edge.</returns>
        public Edge NewEdge(Vertex @out, Vertex @in) {
            if (@out != null && @in != null)
                return new Edge(@out, @in);
            else
                return null;
        }
    }
}
