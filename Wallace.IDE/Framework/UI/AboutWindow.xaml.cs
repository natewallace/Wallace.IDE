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

using System.Windows;
using System.Windows.Media;

namespace Wallace.IDE.Framework.UI
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    internal partial class AboutWindow : Window
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aboutIcon">AboutIcon.</param>
        /// <param name="aboutTitle">AboutTitle.</param>
        /// <param name="aboutSubTitle">AboutSubTitle.</param>
        /// <param name="aboutVersion">AboutVersion.</param>
        /// <param name="aboutDevelopedBy">AboutDevelopedBy.</param>
        public AboutWindow(
            ImageSource aboutIcon,
            string aboutTitle,
            string aboutSubTitle,
            string aboutVersion,
            string aboutDevelopedBy)
        {
            InitializeComponent();

            AboutIcon = aboutIcon;
            AboutTitle = aboutTitle;
            AboutSubTitle = aboutSubTitle;
            AboutVersion = aboutVersion;
            AboutDevelopedBy = aboutDevelopedBy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The icon image.
        /// </summary>
        public ImageSource AboutIcon
        {
            get { return imageIcon.Source; }
            set { imageIcon.Source = value; }
        }

        /// <summary>
        /// The title line.
        /// </summary>
        public string AboutTitle
        {
            get { return textBlockTitle.Text; }
            set { textBlockTitle.Text = value; }
        }

        /// <summary>
        /// The sub title line.
        /// </summary>
        public string AboutSubTitle
        {
            get { return textBlockSubTitle.Text; }
            set { textBlockSubTitle.Text = value; }
        }

        /// <summary>
        /// The version line.
        /// </summary>
        public string AboutVersion
        {
            get { return textBlockVersion.Text; }
            set { textBlockVersion.Text = value; }
        }

        /// <summary>
        /// The developed by line.
        /// </summary>
        public string AboutDevelopedBy
        {
            get { return textBlockDevelopedBy.Text; }
            set { textBlockDevelopedBy.Text = value; }
        }

        #endregion
    }
}
