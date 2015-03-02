// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    /// <summary>
    /// Represents a directed edge consists of two vertices.
    /// </summary>
    /// <remarks>
    /// If have a directed edge &lt;Vi,Vj&gt;, we say that Vi is <see cref="Out"/> vertex, and Vj is <see cref="In"/> vertex.
    /// </remarks>
    public class Edge {
        private readonly Vertex _out;
        private readonly Vertex _in;

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class with the in and out vertices.
        /// </summary>
        /// <param name="out">The source vertex.</param>
        /// <param name="in">The target vertex.</param>
        public Edge(Vertex @out, Vertex @in) {
            _out = @out;
            _in = @in;

            // establish relationship
            NextOut = @out.FirstOut;
            NextIn = @in.FirstIn;
            @out.FirstOut = this;
            @in.FirstIn = this;

            // Increment the in vertex in degree and out degree.
            @in.InDegree++;
            @out.OutDegree++;
        }

        /// <summary>
        /// Gets or sets the next out edge.
        /// </summary>
        /// <value>The next out edge.</value>
        public Edge NextOut { get; set; }

        /// <summary>
        /// Gets or sets the next in edge.
        /// </summary>
        /// <value>The next in edge.</value>
        public Edge NextIn { get; set; }

        /// <summary>
        /// Gets or sets the weight associated with this <see cref="Edge"/>.
        /// </summary>
        /// <value>The weight associated with this <see cref="Edge"/>.</value>
        public double? Weight { get; set; }

        /// <summary>
        /// Gets the out vertex of this <see cref="Edge"/>.
        /// </summary>
        /// <value>The out vertex of this <see cref="Edge"/>.</value>
        public Vertex Out {
            get { return _out; }
        }

        /// <summary>
        /// Gets the in vertex of this <see cref="Edge"/>.
        /// </summary>
        /// <value>The in vertex of this <see cref="Edge"/>.</value>
        public Vertex In {
            get { return _in; }
        }

        /// <summary>
        /// Gets or sets the edge type.
        /// </summary>
        /// <value>The edge type.</value>
        internal EdgeType Type { get; set; }
    }
}
