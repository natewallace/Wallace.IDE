/*
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

using System.Windows.Controls;

namespace Wallace.IDE.Framework.UI
{
    /// <summary>
    /// Implements IFunctionPresenter using MenuItem.
    /// </summary>
    internal class MenuItemFunctionPresenter : HostBase<MenuItem>, IFunctionPresenter
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <param name="manager">Manager.</param>
        public MenuItemFunctionPresenter(MenuItem host, IFunctionManager manager)
        {
            Host = host;
            Manager = manager;
        }

        #endregion

        #region IFunctionPresenter Members

        /// <summary>
        /// The header for the node.
        /// </summary>
        public object Header
        {
            get
            {
                EnsureHost();
                return Host.Header;
            }
            set
            {
                EnsureHost();

                if (value is string)
                    Host.Header = (value as string).Replace("_", "__").Replace('&', '_');
                else              
                    Host.Header = value;
            }
        }

        /// <summary>
        /// The icon for the node.
        /// </summary>
        public object Icon
        {
            get
            {
                EnsureHost();
                return Host.Icon;
            }
            set
            {
                EnsureHost();
                Host.Icon = value;
            }
        }

        /// <summary>
        /// Get/Set if the function is displayed as enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                EnsureHost();
                return Host.IsEnabled;
            }
            set
            {
                EnsureHost();
                Host.IsEnabled = value;
            }
        }

        /// <summary>
        /// Get/Set if the function is visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                EnsureHost();
                return (Host.Visibility == System.Windows.Visibility.Visible);
            }
            set
            {
                EnsureHost();
                Host.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Get/Set tooltip that is displayed.
        /// </summary>
        public object ToolTip
        {
            get
            {
                EnsureHost();
                return Host.ToolTip;
            }
            set
            {
                EnsureHost();
                Host.ToolTip = value;
            }
        }

        /// <summary>
        /// Shortcut key(s) for this function if any.
        /// </summary>
        public string InputGestureText
        {
            get
            {
                EnsureHost();
                return Host.InputGestureText;
            }
            set
            {
                EnsureHost();
                Host.InputGestureText = value;
            }
        }

        /// <summary>
        /// The manager this presenter belongs to.
        /// </summary>
        public IFunctionManager Manager { get; private set; }

        #endregion
    }
}
