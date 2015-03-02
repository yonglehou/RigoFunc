// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    /// <summary>
    /// Used to keep track of which vertices have been discovered in the graph algorithms.
    /// </summary>
    internal enum VertexMarks {
        /// <summary>
        /// White marks vertices that have yet to be discovered.
        /// </summary>
        White,
        /// <summary>
        /// Black marks vertex is discovered vertex that is not adjacent to any white vertices.
        /// </summary>
        Black,
        /// <summary>
        /// Gray marks a vertex that is discovered but still has vertices adjacent to it that are undiscovered.
        /// </summary>
        Gray,
    }
}
