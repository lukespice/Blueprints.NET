﻿/*
 * Copyright (c) 2010, Achim 'ahzf' Friedland
 * 
 * This file is part of blueprints.NET and licensed
 * as free software under the New BSD License.
 */

#region Usings

using System;

#endregion

namespace de.ahzf.blueprints
{

    /// <summary>
    /// An edge links two vertices. Along with its key/value properties,
    /// an edge has both a directionality and a label.
    /// The directionality determines which vertex is the tail vertex
    /// (out vertex) and which vertex is the head vertex (in vertex).
    /// The edge label determines the type of relationship that exists
    /// between the two vertices.
    /// Diagrammatically, outVertex ---label---> inVertex.
    /// </summary>
    public interface IEdge : IElement
    {

        /// <summary>
        /// Return the vertex on the tail of the edge.
        /// </summary>
        IVertex OutVertex { get; }


        /// <summary>
        /// Return the vertex on the head of the edge.
        /// </summary>
        IVertex InVertex { get; }


        /// <summary>
        /// Return the label associated with the edge.
        /// </summary>
        String Label { get; }

    }

}