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

using System;
using Wallace.IDE.Framework.UI;

namespace Wallace.IDE.Framework.Function
{
    /// <summary>
    /// Displays the about dialog.
    /// </summary>
    internal class AboutFunction : FunctionBase
    {
        #region Methods

        /// <summary>
        /// Set header displayed.
        /// </summary>
        /// <param name="host">The type of the host.</param>
        /// <param name="presenter">Presenter to use.</param>
        public override void Init(FunctionHost host, IFunctionPresenter presenter)
        {
            presenter.Header = String.Format("&About {0}...", IDE.Current.Details.Title);
            presenter.Icon = VisualHelper.CreateIconHeader(null, "Empty.png");
        }

        /// <summary>
        /// Display about dialog.
        /// </summary>
        public override void Execute()
        {
            IDE.ShowDialog(new AboutWindow(
                IDE.Current.Details.Icon,
                IDE.Current.Details.Title,
                IDE.Current.Details.SubTitle,
                IDE.Current.Details.Version.ToString(),
                IDE.Current.Details.DevelopedBy)
                {
                    Title = String.Format("About {0}", IDE.Current.Details.Title)
                });
        }

        #endregion
    }
}
