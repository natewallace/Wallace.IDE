using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wallace.IDE.Framework
{
    /// <summary>
    /// A hot key.
    /// </summary>
    public class HotKey
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="modifiers">Modifiers.</param>
        public HotKey(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key">Key.</param>
        public HotKey(Key key)
            : this(key, ModifierKeys.None)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The key.
        /// </summary>
        public Key Key { get; private set; }

        /// <summary>
        /// The modifiers if any.
        /// </summary>
        public ModifierKeys Modifiers { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Checks to see if the given KeyEventArgs matches this hotkey.
        /// </summary>
        /// <param name="e">Event arguments to check.</param>
        /// <returns>true if it's a match, false otherwise.</returns>
        public bool IsMatch(KeyEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (e.Key == System.Windows.Input.Key.System)
                return (e.SystemKey == Key && Keyboard.Modifiers == Modifiers);
            else
                return (e.Key == Key && Keyboard.Modifiers == Modifiers);
        }

        #endregion
    }
}
