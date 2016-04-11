﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Energy.cs" company="Units.NET">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides operators related to energy.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Units
{
    /// <summary>
    ///     Provides operators related to energy.
    /// </summary>
    public partial struct Energy
    {
        /// <summary>
        ///     Implements the operator /.
        /// </summary>
        /// <param name="x"> The x. </param>
        /// <param name="y"> The y. </param>
        /// <returns> The result of the operator. </returns>
        public static HeatCapacity operator /(Energy x, TemperatureDifference y)
        {
            return new HeatCapacity(x.value / y.Value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="Units.Torque" /> to <see cref="Units.Energy" />.
        /// </summary>
        /// <param name="m"> The m. </param>
        /// <returns> The result of the conversion. </returns>
        public static implicit operator Energy(Torque m)
        {
            return new Energy(m.Value);
        }
    }
}