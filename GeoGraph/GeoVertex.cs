﻿/*
 * Copyright (c) 2010-2011, Achim 'ahzf' Friedland <code@ahzf.de>
 * This file is part of Blueprints.NET <http://www.github.com/ahzf/Blueprints.NET>
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
using System.Linq;
using System.Collections.Generic;

using de.ahzf.Blueprints.GenericGraph;

#endregion

namespace de.ahzf.Blueprints.GeoGraph
{

    /// <summary>
    /// A vertex maintains pointers to both a set of incoming and outgoing edges.
    /// The outgoing edges are those edges for which the vertex is the tail.
    /// The incoming edges are those edges for which the vertex is the head.
    /// Diagrammatically, ---inEdges---> vertex ---outEdges--->.
    /// </summary>
    public class GeoVertex : IGeoVertex
    {

        #region Data

        /// <summary>
        /// The edges emanating from, or leaving, this vertex.
        /// </summary>
        protected readonly HashSet<IGenericEdge<VertexId,    RevisionId, GeoCoordinate,
                                                EdgeId,      RevisionId, Distance,
                                                HyperEdgeId, RevisionId, Distance>> _OutEdges;

        /// <summary>
        /// The edges incoming to, or arriving at, this vertex.
        /// </summary>
        protected readonly HashSet<IGenericEdge<VertexId,    RevisionId, GeoCoordinate,
                                                EdgeId,      RevisionId, Distance,
                                                HyperEdgeId, RevisionId, Distance>> _InEdges;

        #endregion

        #region Properties

        #region Id

        /// <summary>
        /// An identifier that is unique to its inheriting class.
        /// All vertices of a graph must have unique identifiers.
        /// All edges of a graph must have unique identifiers.
        /// </summary>
        public VertexId Id { get; private set; }

        #endregion

        public RevisionId RevisionId { get; private set; }


        public String Type
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<String> Properties
        {
            get { throw new NotImplementedException(); }
        }



        public GeoCoordinate Data { get; private set; }

        #endregion

        #region Constructor(s)

        #region GeoVertex(myIGraph, myVertexId, myVertexInitializer = null)

        /// <summary>
        /// Creates a new vertex.
        /// </summary>
        /// <param name="myIGraph">The associated graph.</param>
        /// <param name="myVertexId">The identification of this vertex.</param>
        /// <param name="myVertexInitializer">A delegate to initialize the newly created vertex.</param>
        internal protected GeoVertex(IGenericGraph myIGraph, VertexId myVertexId, Action<IGeoVertex> myVertexInitializer = null)
        {
            
            _OutEdges = new HashSet<IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance>>();
            _InEdges  = new HashSet<IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance>>();

            Id = myVertexId;

            if (myVertexInitializer != null)
                myVertexInitializer(this);

        }

        #endregion

        #endregion


        #region IGeoVertex Members

        public GeoCoordinate GeoCoordinate { get; set; }

        public String Name      { get { return GeoCoordinate.Name;      } }
        public Double Latitude  { get { return GeoCoordinate.Latitude;  } }
        public Double Longitude { get { return GeoCoordinate.Longitude; } }
        public Double Height    { get { return GeoCoordinate.Height;    } }

        #endregion

        #region OutEdges

        #region AddOutEdge(myIEdge)

        /// <summary>
        /// Add an outgoing edge.
        /// </summary>
        /// <param name="myIEdge">The edge to add.</param>
        public void AddOutEdge(IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance> myIEdge)
        {
            _OutEdges.Add(myIEdge);
        }

        #endregion

        #region OutEdges

        /// <summary>
        /// The edges emanating from, or leaving, this vertex.
        /// </summary>
        public IEnumerable<IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance>> OutEdges
        {
            get
            {
                return _OutEdges;
            }
        }

        #endregion

        #region GetOutEdges(myLabel)

        /// <summary>
        /// The edges emanating from, or leaving, this vertex
        /// filtered by their label.
        /// </summary>
        public IEnumerable<IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance>> GetOutEdges(String myLabel)
        {
            return from _Edge in _OutEdges where _Edge.Label == myLabel select _Edge;
        }

        #endregion

        #region RemoveOutEdge(myIEdge)

        /// <summary>
        /// Remove an outgoing edge.
        /// </summary>
        /// <param name="myIEdge">The edge to remove.</param>
        public void RemoveOutEdge(IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance> myIEdge)
        {
            lock (this)
            {
                _OutEdges.Remove(myIEdge);
            }
        }

        #endregion

        #endregion

        #region InEdges

        #region AddInEdge(myIEdge)

        /// <summary>
        /// Add an incoming edge.
        /// </summary>
        /// <param name="myIEdge">The edge to add.</param>
        public void AddInEdge(IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance> myIEdge)
        {
            _InEdges.Add(myIEdge);
        }

        #endregion

        #region InEdges

        /// <summary>
        /// The edges incoming to, or arriving at, this vertex.
        /// </summary>
        public IEnumerable<IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance>> InEdges
        {
            get
            {
                return _InEdges;
            }
        }

        #endregion

        #region GetInEdges(myLabel)

        /// <summary>
        /// The edges incoming to, or arriving at, this vertex
        /// filtered by their label.
        /// </summary>
        public IEnumerable<IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance>> GetInEdges(String myLabel)
        {
            return from _Edge in _InEdges where _Edge.Label == myLabel select _Edge;
        }

        #endregion

        #region RemoveInEdge(myIEdge)

        /// <summary>
        /// Remove an incoming edge.
        /// </summary>
        /// <param name="myIEdge">The edge to remove.</param>
        public void RemoveInEdge(IGenericEdge<VertexId, RevisionId, GeoCoordinate,
                                                 EdgeId, RevisionId, Distance,
                                                 HyperEdgeId, RevisionId, Distance> myIEdge)
        {
            lock (this)
            {
                _InEdges.Remove(myIEdge);
            }
        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (myGeoVertex1, myGeoVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myGeoVertex1">A GeoVertex.</param>
        /// <param name="myGeoVertex2">Another GeoVertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (GeoVertex myGeoVertex1, GeoVertex myGeoVertex2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(myGeoVertex1, myGeoVertex2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) myGeoVertex1 == null) || ((Object) myGeoVertex2 == null))
                return false;

            return myGeoVertex1.Equals(myGeoVertex2);

        }

        #endregion

        #region Operator != (myGeoVertex1, myGeoVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myGeoVertex1">A GeoVertex.</param>
        /// <param name="myGeoVertex2">Another GeoVertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (GeoVertex myGeoVertex1, GeoVertex myGeoVertex2)
        {
            return !(myGeoVertex1 == myGeoVertex2);
        }

        #endregion

        #region Operator <  (myGeoVertex1, myGeoVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myGeoVertex1">A GeoVertex.</param>
        /// <param name="myGeoVertex2">Another GeoVertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (GeoVertex myGeoVertex1, GeoVertex myGeoVertex2)
        {

            // Check if myGeoVertex1 is null
            if ((Object) myGeoVertex1 == null)
                throw new ArgumentNullException("Parameter myGeoVertex1 must not be null!");

            // Check if myGeoVertex2 is null
            if ((Object) myGeoVertex2 == null)
                throw new ArgumentNullException("Parameter myGeoVertex2 must not be null!");

            return myGeoVertex1.CompareTo(myGeoVertex2) < 0;

        }

        #endregion

        #region Operator >  (myGeoVertex1, myGeoVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myGeoVertex1">A GeoVertex.</param>
        /// <param name="myGeoVertex2">Another GeoVertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (GeoVertex myGeoVertex1, GeoVertex myGeoVertex2)
        {

            // Check if myGeoVertex1 is null
            if ((Object) myGeoVertex1 == null)
                throw new ArgumentNullException("Parameter myGeoVertex1 must not be null!");

            // Check if myGeoVertex2 is null
            if ((Object) myGeoVertex2 == null)
                throw new ArgumentNullException("Parameter myGeoVertex2 must not be null!");

            return myGeoVertex1.CompareTo(myGeoVertex2) > 0;

        }

        #endregion

        #region Operator <= (myGeoVertex1, myGeoVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myGeoVertex1">A GeoVertex.</param>
        /// <param name="myGeoVertex2">Another GeoVertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (GeoVertex myGeoVertex1, GeoVertex myGeoVertex2)
        {
            return !(myGeoVertex1 > myGeoVertex2);
        }

        #endregion

        #region Operator >= (myGeoVertex1, myGeoVertex2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myGeoVertex1">A GeoVertex.</param>
        /// <param name="myGeoVertex2">Another GeoVertex.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (GeoVertex myGeoVertex1, GeoVertex myGeoVertex2)
        {
            return !(myGeoVertex1 < myGeoVertex2);
        }

        #endregion

        #endregion

        #region IComparable<IGeoVertex> Members

        #region CompareTo(myObject)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myObject">An object to compare with.</param>
        /// <returns>true|false</returns>
        public Int32 CompareTo(Object myObject)
        {

            // Check if myObject is null
            if (myObject == null)
                throw new ArgumentNullException("myObject must not be null!");

            // Check if myObject can be casted to an IEdge object
            var myIGeoVertex = myObject as IGeoVertex;
            if ((Object) myIGeoVertex == null)
                throw new ArgumentException("myObject is not of type myIGeoVertex!");

            return CompareTo(myIGeoVertex);

        }

        #endregion

        #region CompareTo(myIGeoVertex)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myIGenericVertex">An object to compare with.</param>
        /// <returns>true|false</returns>
        public Int32 CompareTo(IGenericVertex<VertexId,    RevisionId, GeoCoordinate,
                                              EdgeId,      RevisionId, Distance,
                                              HyperEdgeId, RevisionId, Distance> myIGenericVertex)
        {

            // Check if myIGeoVertex is null
            if (myIGenericVertex == null)
                throw new ArgumentNullException("myIGeoVertex must not be null!");

            return Id.CompareTo(myIGenericVertex.Id);

        }

        #endregion

        #region CompareTo(myIGeoVertex)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myIGenericVertex">An object to compare with.</param>
        /// <returns>true|false</returns>
        public Int32 CompareTo(VertexId myIGenericVertex)
        {

            // Check if myIGeoVertex is null
            if (myIGenericVertex == null)
                throw new ArgumentNullException("myIGeoVertex must not be null!");

            return Id.CompareTo(myIGenericVertex);

        }

        #endregion

        #endregion

        #region IEquatable<IGeoVertex> Members

        #region Equals(myObject)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myObject">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object myObject)
        {

            if (myObject == null)
                return false;

            var _Object = myObject as IGeoVertex;
            if (_Object != null)
                return Equals(_Object);

            return false;

        }

        #endregion

        #region Equals(myIGenericVertex)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myIGenericVertex">An object to compare with.</param>
        /// <returns>true|false</returns>
        public Boolean Equals(IGenericVertex<VertexId,    RevisionId, GeoCoordinate,
                                             EdgeId,      RevisionId, Distance,
                                             HyperEdgeId, RevisionId, Distance> myIGenericVertex)
        {

            if ((Object)myIGenericVertex == null)
                return false;

            //TODO: Here it might be good to check all attributes of the UNIQUE constraint!
            return (this.Id == myIGenericVertex.Id);

        }

        #endregion

        #region Equals(myIGenericVertex)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="myIGenericVertex">An object to compare with.</param>
        /// <returns>true|false</returns>
        public Boolean Equals(VertexId myIGenericVertex)
        {

            if ((Object) myIGenericVertex == null)
                return false;

            //TODO: Here it might be good to check all attributes of the UNIQUE constraint!
            return (this.Id == myIGenericVertex);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

    }

}
