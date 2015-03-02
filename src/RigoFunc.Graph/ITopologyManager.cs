// Copyright (c) xyting. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace RigoFunc.Graph {
    using System.Collections.Generic;

    /// <summary>
    /// Provides the interfaces for all the topology managers.
    /// </summary>
    public interface ITopologyManager {
        /// <summary>
        /// Gets all units from manager.
        /// </summary>
        /// <returns>IEnumerable&lt;IUnit&gt;.</returns>
        IEnumerable<IUnit> GetUnits();

        /// <summary>
        /// Gets the subsequents of the specified unit.
        /// </summary>
        /// <param name="unit">The unit to retrieve.</param>
        /// <returns>IEnumerable&lt;IUnit&gt;.</returns>
        IEnumerable<IUnit> GetSubsequents(IUnit unit);
    }
}
