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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wallace.IDE
{
    /// <summary>
    /// Details for the IDE application.
    /// </summary>
    public class IDEDetails
    {
        #region Fields

        /// <summary>
        /// The uri of the icon.
        /// </summary>
        private string _iconUri;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="subTitle">SubTitle.</param>
        /// <param name="iconUri">A uri that is the location of the icon.</param>
        /// <param name="version">Version.</param>
        /// <param name="developedBy">DevelopedBy.</param>
        /// <param name="userGuideUrl">UserGuideUrl.</param>
        public IDEDetails(
            string title,
            string subTitle,
            string iconUri,
            Version version,
            string developedBy,
            string userGuideUrl)
        {
            Title = title;
            SubTitle = subTitle;
            Version = version;
            DevelopedBy = developedBy;
            UserGuideUrl = userGuideUrl;

            _iconUri = iconUri;
            if (_iconUri != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_iconUri);
                bitmap.EndInit();
                Icon = bitmap;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The title of the application.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// A sub title for the application.
        /// </summary>
        public string SubTitle { get; private set; }

        /// <summary>
        /// The icon for the application.
        /// </summary>
        public ImageSource Icon { get; private set; }

        /// <summary>
        /// The version number for the application.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// The developed by line.
        /// </summary>
        public string DevelopedBy { get; private set; }

        /// <summary>
        /// The url for the user guide.
        /// </summary>
        public string UserGuideUrl { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get a frame from the icon with the given size.
        /// </summary>
        /// <returns>The requested frame.</returns>
        public ImageSource GetIconFrame(int width, int height)
        {
            if (_iconUri == null)
                return null;

            IconBitmapDecoder decoder = new IconBitmapDecoder(
                new Uri(_iconUri),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);

            foreach (BitmapFrame frame in decoder.Frames)
            {
                if (frame.PixelWidth == width && frame.PixelHeight == height)
                    return frame;
            }

            if (decoder.Frames.Count > 0)
                return decoder.Frames[0];

            return Icon;
        }

        #endregion
    }
}
