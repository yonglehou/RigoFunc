// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    /// <summary>
    /// Represents the all types of edges.
    /// </summary>
    internal enum EdgeType {
        /// <summary>
        /// Tree edges are edges in the search tree (or forest) constructed (implicitly or explicitly) by running a graph search algorithm over a graph.
        /// An edge (u,v) is a tree edge if v was first discovered while exploring edge (u,v). 
        /// </summary>
        TreeEdge,
        /// <summary>
        /// Back edges connect vertices to their ancestors in a search tree. So for edge (u,v) the vertex v must be the ancestor of vertex u. 
        /// Self loops are considered to be back edges.
        /// </summary>
        BackEdge,
        /// <summary>
        /// Forward edges are non-tree edges (u,v) that connect a vertex u to a descendant v in a search tree.
        /// </summary>
        ForwardEdge,
        /// <summary>
        /// Cross edges are edges that do not fall into the above three categories.
        /// </summary>
        CrossEdge,
    }
}
