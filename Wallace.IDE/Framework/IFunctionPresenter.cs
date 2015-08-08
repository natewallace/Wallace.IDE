﻿/*
 * Copyright (c) 2015 Nathaniel Wallace
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

namespace Wallace.IDE.Framework
{
    /// <summary>
    /// Interface to object that displays a function.
    /// </summary>
    public interface IFunctionPresenter
    {
        /// <summary>
        /// The header for the node.
        /// </summary>
        object Header { get; set; }

        /// <summary>
        /// The icon for the node.
        /// </summary>
        object Icon { get; set; }

        /// <summary>
        /// Get/Set tooltip that is displayed.
        /// </summary>
        object ToolTip { get; set; }

        /// <summary>
        /// Shortcut key(s) for this function if any.
        /// </summary>
        string InputGestureText { get; set; }

        /// <summary>
        /// The manager this presenter belongs to.
        /// </summary>
        IFunctionManager Manager { get; }
    }
}