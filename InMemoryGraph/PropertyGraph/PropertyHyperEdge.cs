﻿/*
 * Copyright (c) 2010-2011, Achim 'ahzf' Friedland <code@ahzf.de>
 * This file is part of Blueprints.NET
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Collections.Generic;

using de.ahzf.blueprints.Datastructures;

#endregion

namespace de.ahzf.blueprints.InMemory.PropertyGraph.Generic
{

    /// <summary>
    /// A vertex maintains pointers to both a set of incoming and outgoing edges.
    /// The outgoing edges are those edges for which the vertex is the tail.
    /// The incoming edges are those edges for which the vertex is the head.
    /// Diagrammatically, ---inEdges---> vertex ---outEdges--->.
    /// </summary>
    public class PropertyHyperEdge : PropertyHyperEdge<VertexId,    RevisionId, String, Object, IDictionary<String, Object>,
                                                       EdgeId,      RevisionId, String, Object, IDictionary<String, Object>,
                                                       HyperEdgeId, RevisionId, String, Object, IDictionary<String, Object>,
                                                       ICollection<IGenericVertex<VertexId,    RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                                                  EdgeId,      RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                                                  HyperEdgeId, RevisionId, IProperties<String, Object, IDictionary<String, Object>>>>>
    {

        #region Constructor(s)

        #region PropertyHyperEdge(myIGraph, myOutVertex, myInVertex, myEdgeId, myLabel, myEdgeInitializer = null)

        /// <summary>
        /// Creates a new property hyperedge.
        /// </summary>
        /// <param name="myIGraph">The associated graph.</param>
        /// <param name="myOutVertex">The vertex at the tail of the edge.</param>
        /// <param name="myInVertex">The vertex at the head of the edge.</param>
        /// <param name="myEdgeId">The identification of this edge.</param>
        /// <param name="myLabel">A label stored within this edge.</param>
        /// <param name="myEdgeInitializer">A delegate to initialize the newly created edge.</param>
        internal protected PropertyHyperEdge(IGenericVertex<VertexId, RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                            EdgeId,      RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                            HyperEdgeId, RevisionId, IProperties<String, Object, IDictionary<String, Object>>>
                                                            myOutVertex,

                                             IEnumerable<IGenericVertex<VertexId,    RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                                        EdgeId,      RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                                        HyperEdgeId, RevisionId, IProperties<String, Object, IDictionary<String, Object>>>>
                                                                        myInVertices,

                                             HyperEdgeId myHyperEdgeId,
                                             String      myLabel,

                                             Action<IPropertyHyperEdge<VertexId,    RevisionId, String, Object, IDictionary<String, Object>,
                                                                       EdgeId,      RevisionId, String, Object, IDictionary<String, Object>,
                                                                       HyperEdgeId, RevisionId, String, Object, IDictionary<String, Object>>>
                                                                       myHyperEdgeInitializer = null)

            : base(myOutVertex, myInVertices,
                   myHyperEdgeId, myLabel,
                   "Id", "RevisionId",
                   () => new Dictionary<String, Object>(),
                   () => new HashSet<IGenericVertex<VertexId,    RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                    EdgeId,      RevisionId, IProperties<String, Object, IDictionary<String, Object>>,
                                                    HyperEdgeId, RevisionId, IProperties<String, Object, IDictionary<String, Object>>>>(),
                   myHyperEdgeInitializer)

        { }

        #endregion

        #endregion

    }

}