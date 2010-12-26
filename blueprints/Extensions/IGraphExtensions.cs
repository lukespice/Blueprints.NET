﻿/*
 * Copyright (c) 2010, Achim 'ahzf' Friedland <code@ahzf.de>
 * 
 * This file is part of blueprints.NET and licensed
 * as free software under the New BSD License.
 */

#region Usings

using de.ahzf.blueprints.Datastructures;
using System;
using System.Reflection;

#endregion

namespace de.ahzf.blueprints
{

    /// <summary>
    /// Extensions to the IGraph interface
    /// </summary>
    public static class IGraphExtensions
    {

        #region AsDynamic(this myIGraph)

        /// <summary>
        /// Converts the given IGraph into a dynamic object
        /// </summary>
        /// <param name="myIGraph">An object implementing IGraph.</param>
        /// <returns>A dynamic object</returns>
        public static dynamic AsDynamic(this IGraph myIGraph)
        {
            return myIGraph as dynamic;
        }

        #endregion
        

        #region AddVertex<TVertex>(myVertexId = null, myVertexInitializer = null)

        /// <summary>
        /// Adds a vertex of type TVertex to the graph using the given VertexId and
        /// initializes its properties by invoking the given vertex initializer.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex to add.</typeparam>
        /// <param name="myIGraph"></param>
        /// <param name="myVertexId">A VertexId. If none was given a new one will be generated.</param>
        /// <param name="myVertexInitializer">A delegate to initialize the newly generated vertex.</param>
        /// <returns>The new vertex</returns>
        public static TVertex AddVertex<TVertex>(this IGraph myIGraph, VertexId myVertexId = null, Action<IVertex> myVertexInitializer = null)
            where TVertex : class, IVertex
        {

            // Get constructor for TVertex
            var _Type = typeof(TVertex).
                        GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                       null,
                                       new Type[] {
                                           typeof(IGraph),
                                           typeof(VertexId),
                                           typeof(Action<IVertex>)
                                       },
                                       null);

            if (_Type == null)
                throw new ArgumentException("A appropriate constructor for type TVertex could not be found!");


            // Invoke constructor of TVertex
            var _TVertex = _Type.Invoke(new Object[] { myIGraph, myVertexId, myVertexInitializer }) as TVertex;

            if (_TVertex == null)
                throw new ArgumentException("A vertex of type TVertex could not be created!");


            // Add to IGraph
            myIGraph.AddVertex(myVertexId, myVertexInitializer);

            return _TVertex;

        }

        #endregion

        #region GetVertex<TVertex>(this myIGraph, myVertexId)

        /// <summary>
        /// Return the vertex referenced by the provided vertex identifier.
        /// If no vertex is referenced by that identifier, then return null.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <param name="myIGraph">An object implementing IGraph.</param>
        /// <param name="myVertexId">The identifier of the vertex to retrieved from the graph.</param>
        /// <returns>The vertex referenced by the provided identifier or null when no such vertex exists.</returns>
        public static TVertex GetVertex<TVertex>(this IGraph myIGraph, VertexId myVertexId)
            where TVertex : class, IVertex
        {
            return myIGraph.GetVertex(myVertexId) as TVertex;
        }

        #endregion


        #region AddEdge<TEdge>(myOutVertex, myInVertex, myEdgeId = null, myLabel = null, myEdgeInitializer = null)

        /// <summary>
        /// Adds an edge to the graph using the given myEdgeId and initializes
        /// its properties by invoking the given edge initializer.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge to add.</typeparam>
        /// <param name="myIGraph"></param>
        /// <param name="myOutVertex"></param>
        /// <param name="myInVertex"></param>
        /// <param name="myEdgeId">A EdgeId. If none was given a new one will be generated.</param>
        /// <param name="myLabel"></param>
        /// <param name="myEdgeInitializer">A delegate to initialize the newly generated edge.</param>
        /// <returns>The new edge</returns>
        public static TEdge AddEdge<TEdge>(this IGraph myIGraph, IVertex myOutVertex, IVertex myInVertex, EdgeId myEdgeId = null, String myLabel = null, Action<IEdge> myEdgeInitializer = null)
            where TEdge : class, IEdge
        {

            // Get constructor for TEdge
            var _Type  = typeof(TEdge).
                         GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                        null,
                                        new Type[] {
                                            typeof(IGraph),
                                            typeof(IVertex),
                                            typeof(IVertex),
                                            typeof(EdgeId),
                                            typeof(Action<IEdge>)
                                        },
                                        null);

            if (_Type == null)
                throw new ArgumentException("A appropriate constructor for type TEdge could not be found!");


            // Invoke constructor of TEdge
            var _TEdge = _Type.Invoke(new Object[] { myIGraph, myOutVertex, myInVertex, myEdgeId, myEdgeInitializer }) as TEdge;
            if (_TEdge == null)
                throw new ArgumentException("An edge of type TEdge could not be created!");


            // Add to IGraph
            myIGraph.AddEdge(myOutVertex, myInVertex, myEdgeId, myLabel, myEdgeInitializer);

            return _TEdge;

        }

        #endregion

        #region GetEdge<TEdge>(this myIGraph, myEdgeId)

        /// <summary>
        /// Return the edge referenced by the provided edge identifier.
        /// If no edge is referenced by that identifier, then return null.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="myIGraph">An object implementing IGraph.</param>
        /// <param name="myEdgeId">The identifier of the edge to retrieved from the graph.</param>
        /// <returns>The edge referenced by the provided identifier or null when no such edge exists.</returns>
        public static TEdge GetEdge<TEdge>(this IGraph myIGraph, EdgeId myEdgeId)
            where TEdge : class, IVertex
        {
            return myIGraph.GetEdge(myEdgeId) as TEdge;
        }

        #endregion


    }

}