﻿// <copyright file="ITile.cs" company="Team4">
// Copyright(c) 2016 All Rights Reserved
// </copyright>
// <author>Alexander Kirk Jørgensen</author>
// <date>27-09-2016</date>
// <summary>Interface defining grid tiles.</summary>
namespace Grid
{
    /// <summary>
    /// Defines whether field has a floor or is special.
    /// </summary>
    public enum FieldStatus
    {
        /// <summary>
        /// Field has no floor.
        /// </summary>
        None,

        /// <summary>
        /// Field has floor
        /// </summary>
        Floor
    }

    /// <summary>
    /// Interface defines valid Grid tiles.
    /// </summary>
    public interface ITile
    {
        /// <summary>
        /// Get the boolean value of the Tile, describing whether it has a floor or not.
        /// </summary>
        /// <returns>The current state of the tile; true means "Has floor", false means "Has no floor".</returns>
        bool GetValue();
    }
}