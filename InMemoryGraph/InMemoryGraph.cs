﻿/*
 * Copyright (c) 2010, Achim 'ahzf' Friedland <code@ahzf.de>
 * 
 * This file is part of blueprints.NET and licensed
 * as free software under the New BSD License.
 */

#region Usings

using System;
using System.Collections.Generic;
using de.ahzf.blueprints.Datastructures;
using System.Reflection;

#endregion

namespace de.ahzf.blueprints.InMemoryGraph
{

    /// <summary>
    /// An in-memory implementation of the IGraph interface.
    /// </summary>
    public class InMemoryGraph : IGraph
    {

        #region Data

        private readonly IDictionary<VertexId, IVertex> _Vertices;
        private readonly IDictionary<EdgeId,   IEdge>   _Edges;
        //protected Map<String, TinkerIndex> indices = new HashMap<String, TinkerIndex>();
        //protected Map<String, TinkerAutomaticIndex> autoIndices = new HashMap<String, TinkerAutomaticIndex>();

        #endregion

        #region Constructor(s)

        #region InMemoryGraph()

        /// <summary>
        /// Created a new in-memory graph.
        /// </summary>
        public InMemoryGraph()
        {
            _Vertices = new SortedDictionary<VertexId, IVertex>();
            _Edges    = new SortedDictionary<EdgeId,   IEdge>();
        //this.createIndex(Index.VERTICES, TinkerVertex.class, Index.Type.AUTOMATIC);
        //this.createIndex(Index.EDGES, TinkerEdge.class, Index.Type.AUTOMATIC);
        }

        #endregion

        #endregion


        #region Vertex methods

        #region AddVertex(myVertexId = null, myVertexInitializer = null)

        /// <summary>
        /// Adds a vertex to the graph using the given VertexId and initializes
        /// its properties by invoking the given vertex initializer.
        /// </summary>
        /// <param name="myVertexId">A VertexId. If none was given a new one will be generated.</param>
        /// <param name="myVertexInitializer">A delegate to initialize the newly generated vertex</param>
        /// <returns>The new vertex</returns>
        public IVertex AddVertex(VertexId myVertexId = null, Action<IVertex> myVertexInitializer = null)
        {

            if (myVertexId != null && _Vertices.ContainsKey(myVertexId))
                throw new ArgumentException("Another vertex with id " + myVertexId + " already exists");

            if (myVertexId == null)
                myVertexId = new VertexId(Guid.NewGuid().ToString());

            var _Vertex = new Vertex(this, myVertexId, myVertexInitializer);


            lock (this)
            {
                _Vertices.Add(myVertexId, _Vertex);
            }

            return _Vertex as IVertex;

        }

        #endregion

        #region GetVertex(myVertexId)

        /// <summary>
        /// Return the vertex referenced by the given vertex identifier.
        /// If no vertex is referenced by that identifier, then return null.
        /// </summary>
        /// <param name="myVertexId">The identifier of the vertex.</param>
        /// <returns>The vertex referenced by the provided identifier or null when no such edge exists.</returns>
        public IVertex GetVertex(VertexId myVertexId)
        {
            
            IVertex _IVertex = null;

            _Vertices.TryGetValue(myVertexId, out _IVertex);

            return _IVertex;

        }

        #endregion

        #region GetVertices(myVertexFilter = null)

        /// <summary>
        /// Get an enumeration of all vertices in the graph.
        /// An additional vertex filter may be applied for filtering.
        /// </summary>
        /// <param name="myVertexFilter">A delegate for vertex filtering.</param>
        public IEnumerable<IVertex> GetVertices(Func<IVertex, Boolean> myVertexFilter = null)
        {

            foreach (var _IVertex in _Vertices.Values)
                if (myVertexFilter == null)
                    yield return _IVertex;

                else if (myVertexFilter(_IVertex))
                    yield return _IVertex;

        }

        #endregion

        #region RemoveVertex(myVertexId)

        /// <summary>
        /// Remove the vertex identified by the given VertexId from the graph
        /// </summary>
        /// <param name="myVertexId">The VertexId of the vertex to remove</param>
        public void RemoveVertex(VertexId myVertexId)
        {
            RemoveVertex(GetVertex(myVertexId));
        }

        #endregion

        #region RemoveVertex(myIVertex)

        /// <summary>
        ///  Remove the given vertex from the graph
        /// </summary>
        /// <param name="myIVertex">A vertex</param>
        public void RemoveVertex(IVertex myIVertex)
        {

            lock (this)
            {

                if (_Vertices.ContainsKey(myIVertex.Id))
                {

                    var _EdgeList = new List<IEdge>();

                    _EdgeList.AddRange(myIVertex.InEdges);
                    _EdgeList.AddRange(myIVertex.OutEdges);

                    // removal requires removal from all indices
                    //for (TinkerIndex index : this.indices.values()) {
                    //    index.remove(vertex);
                    //}

                    _Vertices.Remove(myIVertex.Id);

                }

            }

        }

        #endregion

        #endregion


        #region Edge methods

        #region AddEdge(myOutVertex, myInVertex, myEdgeId = null, myLabel = null, myEdgeInitializer = null)

        /// <summary>
        /// Adds an edge to the graph using the given myEdgeId and initializes
        /// its properties by invoking the given edge initializer.
        /// </summary>
        /// <param name="myOutVertex"></param>
        /// <param name="myInVertex"></param>
        /// <param name="myEdgeId">A EdgeId. If none was given a new one will be generated.</param>
        /// <param name="myLabel"></param>
        /// <param name="myEdgeInitializer">A delegate to initialize the newly generated edge.</param>
        /// <returns>The new edge</returns>
        public IEdge AddEdge(IVertex myOutVertex, IVertex myInVertex, EdgeId myEdgeId = null, String myLabel = null, Action<IEdge> myEdgeInitializer = null)
        {

            if (myEdgeId != null && _Edges.ContainsKey(myEdgeId))
                throw new ArgumentException("Another edge with id " + myEdgeId + " already exists");

            if (myEdgeId == null)
                myEdgeId = new EdgeId(Guid.NewGuid().ToString());

            var _Edge = new Edge(this, myOutVertex, myInVertex, myEdgeId, myLabel, myEdgeInitializer);
            lock (this)
            {
                _Edges.Add(myEdgeId, _Edge);
            }

            myOutVertex.AddOutEdge(_Edge);
            myInVertex.AddInEdge(_Edge);

            return _Edge as IEdge;

        }

        #endregion

        #region GetEdge(myEdgeId)

        /// <summary>
        /// Return the edge referenced by the given edge identifier.
        /// If no edge is referenced by that identifier, then return null.
        /// </summary>
        /// <param name="myEdgeId">The identifier of the edge.</param>
        /// <returns>The edge referenced by the provided identifier or null when no such edge exists.</returns>
        public IEdge GetEdge(EdgeId myEdgeId)
        {

            IEdge _IEdge = null;

            _Edges.TryGetValue(myEdgeId, out _IEdge);

            return _IEdge;

        }

        #endregion

        #region GetEdges(myEdgeFilter = null)

        /// <summary>
        /// Get an enumeration of all edges in the graph.
        /// An additional edge filter may be applied for filtering.
        /// </summary>
        /// <param name="myEdgeFilter">A delegate for edge filtering.</param>
        public IEnumerable<IEdge> GetEdges(Func<IEdge, Boolean> myEdgeFilter = null)
        {

            if (myEdgeFilter == null)
                foreach (var _IEdge in _Edges.Values)
                    yield return _IEdge;

            foreach (var _IEdge in _Edges.Values)
                if (myEdgeFilter(_IEdge))
                    yield return _IEdge;

        }

        #endregion

        #region RemoveEdge(myEdgeId)

        /// <summary>
        /// Remove the edge identified by the given EdgeId from the graph
        /// </summary>
        /// <param name="myEdgeId">The myEdgeId of the edge to remove</param>
        public void RemoveEdge(EdgeId myEdgeId)
        {
            RemoveEdge(GetEdge(myEdgeId));
        }

        #endregion

        #region RemoveEdge(myIEdge)

        /// <summary>
        ///  Remove the given edge from the graph
        /// </summary>
        /// <param name="myIEdge">An edge</param>
        public void RemoveEdge(IEdge myIEdge)
        {

            lock (this)
            {

                if (_Edges.ContainsKey(myIEdge.Id))
                {

                    var _OutVertex = myIEdge.OutVertex;
                    var _InVertex  = myIEdge.InVertex;

                    if (_OutVertex != null && _OutVertex.OutEdges != null)
                        _OutVertex.RemoveOutEdge(myIEdge);

                    if (_InVertex != null && _InVertex.InEdges != null)
                        _InVertex.RemoveInEdge(myIEdge);

                    // removal requires removal from all indices
                    //for (TinkerIndex index : this.indices.values()) {
                    //    index.remove(edge);
                    //}

                    _Edges.Remove(myIEdge.Id);

                }

            }

        }

        #endregion

        #endregion


        #region Clear()

        /// <summary>
        /// Remove all the edges and vertices from the graph.
        /// </summary>
        public void Clear()
        {
            _Vertices.Clear();
            _Edges.Clear();
        }

        #endregion

        #region Shutdown()

        /// <summary>
        /// Shutdown and close the graph.
        /// </summary>
        public void Shutdown()
        {
            Clear();
        }

        #endregion

    }

}