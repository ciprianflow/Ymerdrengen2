﻿// <copyright file="BaseTile.cs" company="Team4">
// Copyright(c) 2016 All Rights Reserved
// </copyright>
// <author>Alexander Kirk Jørgensen</author>
// <date>27-09-2016</date>
// <summary>Base class for tiles in the Grid structure.</summary>
using System;

namespace Grid
{
    /// <summary>
    /// Base class for Grid tiles, implements the ITile interface which exposes the GetValue() method.
    /// </summary>
    public class BaseTile : ITile
    {
        /// <summary>
        /// Gets or sets the current value of the field, determining what kind of field it is.
        /// </summary>
        public FieldStatus Value { get; set; }

        /// <summary>
        /// Gets the current value of the field, determining what kind of field it is.
        /// </summary>
        /// <returns>FieldStatus describing the current state of the field.</returns>
        public FieldStatus GetStatus()
        {
            return Value;
        }

        /// <summary>
        /// Gets the current value of the field.
        /// </summary>
        /// <returns>The FieldStatus of this field.</returns>
        public bool HasFloor()
        {
            return (Value & FieldStatus.Floor) != FieldStatus.None;
        }

        public bool IsBlocked()
        {
            return (Value & FieldStatus.Blocked) != FieldStatus.None;
        }

        public bool IsOnFire()
        {
            return (Value & FieldStatus.OnFire) != FieldStatus.None;
        }

        public bool IsPickUp()
        {
            return (Value & FieldStatus.PickUp) != FieldStatus.None;
        }

        public bool IsNewTile()
        {
            return (Value & FieldStatus.NewTile) != FieldStatus.None;
        }

        public void ToggleFlags(FieldStatus flags)
        {
            Value = Value ^ flags;
        }
    }
}
