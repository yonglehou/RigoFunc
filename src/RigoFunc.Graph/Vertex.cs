// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a vertex of the graph.
    /// </summary>
    public class Vertex {
        private IList<IUnit> _units;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        public Vertex() {
            _units = new List<IUnit>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public Vertex(IUnit unit)
            : this() {

        }

        /// <summary>
        /// Gets or sets the in degree.
        /// </summary>
        /// <value>The in degree.</value>
        public int InDegree { get; set; }

        /// <summary>
        /// Gets or sets the out degree.
        /// </summary>
        /// <value>The out degree.</value>
        public int OutDegree { get; set; }

        /// <summary>
        /// Gets or sets the first out edge.
        /// </summary>
        /// <value>The first out edge.</value>
        public Edge FirstOut { get; set; }

        /// <summary>
        /// Gets or sets the first in edge.
        /// </summary>
        /// <value>The first in edge.</value>
        public Edge FirstIn { get; set; }

        /// <summary>
        /// Gets or sets the vertex mark.
        /// </summary>
        /// <value>The vertex mark.</value>
        internal VertexMarks Mark { get; set; }

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <value>The units.</value>
        public IList<IUnit> Units {
            get {
                return _units;
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        /// <summary>
        /// Appends the specified unit.
        /// </summary>
        /// <param name="unit">The unit to append.</param>
        public void Append(IUnit unit) {
            if (!_units.Contains(unit)) {
                _units.Add(unit);
            }
        }

        /// <summary>
        /// Appends a range of units
        /// </summary>
        /// <param name="units">The units to append</param>
        public void AppendRange(IEnumerable<IUnit> units) {
            foreach (var unit in units) {
                Append(unit);
            }
        }

        /// <summary>
        /// Performs the activity on vertex.
        /// </summary>
        /// <param name="func">The function.</param>
        public void PerformAOV(Func<IList<IUnit>, bool> func) {
            func(_units);
        }
    }
}
