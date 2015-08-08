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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Wallace.IDE.Framework;
using Wallace.IDE.Framework.Function;
using Wallace.IDE.Framework.UI;

namespace Wallace.IDE
{
    /// <summary>
    /// Main IDE application object.
    /// </summary>
    public class IDE
    {
        #region Fields

        /// <summary>
        /// Holds the main window for the application.
        /// </summary>
        private MainWindow _window;

        /// <summary>
        /// Holds registered functions.
        /// </summary>
        private Dictionary<Type, IFunction> _functions;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the one instance of this application.
        /// </summary>
        public static IDE Current { get; private set; }

        /// <summary>
        /// The dispatcher for the main window.
        /// </summary>
        public Dispatcher Dispatcher
        {
            get { return _window.Dispatcher; }
        }

        /// <summary>
        /// Text to display for the current session.
        /// </summary>
        public string SessionTitle
        {
            get { return _window.SessionTitle; }
            set { _window.SessionTitle = value; }
        }

        /// <summary>
        /// Text to display in the navigation title area.
        /// </summary>
        public string NavigationTitle
        {
            get { return _window.NavigationTitle; }
            set { _window.NavigationTitle = value; }
        }

        /// <summary>
        /// The menu that displays functions.
        /// </summary>
        public IFunctionManager Menu { get; private set; }

        /// <summary>
        /// The toolbar that displays functions.
        /// </summary>
        public IFunctionManager ToolBar { get; private set; }

        /// <summary>
        /// The documents workspace.
        /// </summary>
        public IDocumentManager Content { get; private set; }

        /// <summary>
        /// The nodes workspace.
        /// </summary>
        public INodeManager Navigation { get; private set; }

        /// <summary>
        /// The settings manager for the IDE.
        /// </summary>
        public ISettingsManager Settings { get; private set; }

        /// <summary>
        /// The details for the IDE.
        /// </summary>
        public IDEDetails Details { get; private set; }

        /// <summary>
        /// Indicates if settings need to be upgraded.
        /// </summary>
        public bool IsSettingsUpgradeRequired
        {
            get { return Wallace.IDE.Properties.Settings.Default.UpgradeRequired; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the IDE.
        /// </summary>
        /// <param name="e">Startup arguments.</param>
        /// <param name="details">The title for the app.</param>
        public static void Load(StartupEventArgs e, IDEDetails details)
        {
            try
            {
                if (Current != null)
                    throw new Exception("IDE has already been loaded.");
                if (details == null)
                    throw new ArgumentNullException("details");

                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new System.Uri("/Wallace.IDE;component/Framework/Styles/ThemeColorStyle.xaml", UriKind.Relative)
                });
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new System.Uri("/Wallace.IDE;component/Themes/Generic.xaml", UriKind.Relative)
                });

                Current = new IDE();
                Current.Details = details;

                Current._window = new MainWindow();
                Current._window.Closing += Current.window_Closing;
                Current._window.KeyDown += Current.window_KeyDown;
                Current._window.WindowState = Wallace.IDE.Properties.Settings.Default.LastWindowState;

                Current._functions = new Dictionary<Type, IFunction>();

                Current.Menu = new MenuFunctionManager(Current._window.MainMenu);
                Current.ToolBar = new ToolBarFunctionManager(Current._window.MainToolBar);
                Current.Navigation = new TabTreeNodeManager(Current._window.Nodes);
                Current.Content = new TabControlDocumentManager(Current._window.Documents);
                Current.Settings = new SettingsManager();

                Current.Navigation.ActiveNodeChanged += Current.Navigation_ActiveNodeChanged;
                Current.Content.ActiveDocumentChanged += Current.Content_ActiveDocumentChanged;

                Current.InitializeFunctions();

                Current._window.Title = details.Title;
                Current._window.Icon = details.GetIconFrame(16, 16);

                Current.UpdateWorkspaces();

                Current._window.Show();
            }
            catch (Exception err)
            {
                IDE.HandleException(err);

                if (Application.Current != null)
                    Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Upgrade the IDE settings.
        /// </summary>
        public void UpgradeSetings()
        {
            if (Wallace.IDE.Properties.Settings.Default.UpgradeRequired)
            {
                Wallace.IDE.Properties.Settings.Default.Upgrade();
                Wallace.IDE.Properties.Settings.Default.UpgradeRequired = false;
                Wallace.IDE.Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Reset all settings.
        /// </summary>
        public void ResetSettings()
        {
            Wallace.IDE.Properties.Settings.Default.Reset();
        }

        /// <summary>
        /// Issue Update to all workspaces.
        /// </summary>
        public void UpdateWorkspaces()
        {
            IDE.Current.Menu.UpdateFunctions();
            IDE.Current.ToolBar.UpdateFunctions();
            IDE.Current.Navigation.UpdateNodes();

            _window.NavigationVisibility = (IDE.Current.Navigation.Nodes.Count == 0) ? Visibility.Collapsed : Visibility.Visible;
            if (IDE.Current.Navigation.Nodes.Count == 0)
                _window.IsNavigationHidden = false;
        }

        /// <summary>
        /// Initialize functions for the application.
        /// </summary>
        private void InitializeFunctions()
        {
            Menu.AddFunction(new FunctionGrouping("SYSTEM", "&SYSTEM", false));

            SettingsFunction settingsFunction = new SettingsFunction();
            Menu.AddFunction(new FunctionSeparator(settingsFunction), "SYSTEM");
            Menu.AddFunction(settingsFunction, "SYSTEM");

            Menu.AddFunction(new FunctionSeparator(), "SYSTEM");
            Menu.AddFunction(new ExitFunction(), "SYSTEM");

            Menu.AddFunction(new FunctionGrouping("PROJECT", "&PROJECT", false));

            Menu.AddFunction(new FunctionGrouping("DOCUMENT", "&DOCUMENT", false));

            Menu.AddFunction(new FunctionGrouping("HELP", "&HELP", false));
            Menu.AddFunction(new UserGuideFunction(), "HELP");
            Menu.AddFunction(new AboutFunction(), "HELP");
        }

        /// <summary>
        /// Shows the given window as a modal dialog.
        /// </summary>
        /// <param name="window">The window to display as a dialog.</param>
        /// <returns>The dialog result of the window that was displayed.</returns>
        public static bool ShowDialog(Window window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (Current != null && Current._window != null && Current._window.IsLoaded)
            {
                window.Owner = Current._window;
                window.ShowInTaskbar = false;
            }

            bool? result = window.ShowDialog();

            return (result.HasValue && result.Value);
        }

        /// <summary>
        /// Sets app to waiting state and returns object that will set app to not waiting when disposed.
        /// </summary>
        /// <param name="message">The message to display while waiting.</param>
        /// <returns>The object that will set the app to not waiting when disposed.</returns>
        public static IDisposable Wait(string message)
        {
            return new DisposableWait();
        }

        /// <summary>
        /// Set the status of the application.
        /// </summary>
        /// <param name="text"></param>
        public static void SetStatus(string text)
        {
            Current._window.StatusText = text;
        }

        /// <summary>
        /// Set the app waiting state.
        /// </summary>
        /// <param name="flag">true to set the app to waiting, false to remove the waiting state.</param>
        public static void SetWaiting(bool flag)
        {
            if (flag)
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            else
                System.Windows.Input.Mouse.OverrideCursor = null;
        }

        /// <summary>
        /// Handle an exception.
        /// </summary>
        /// <param name="err">The exception to handle.</param>
        public static void HandleException(Exception err)
        {
            if (err == null)
            {
                IDE.MessageUser("Error message is null.", "Error", MessageBoxImage.Error, new string[] { "OK" });
            }
            else
            {
                if (IDE.MessageUser(err.Message, "Error", MessageBoxImage.Error, new string[] { "Details", "OK" }) == "Details")
                {
                    ErrorDetailsWindow dlg = new ErrorDetailsWindow();
                    dlg.Error = err;
                    IDE.ShowDialog(dlg);
                }
            }
        }

        /// <summary>
        /// Post a message to the user.
        /// </summary>
        /// <param name="message">The message to post.</param>
        /// <param name="caption">The caption for the message.</param>
        /// <param name="image">The image to display with the message.</param>
        /// <param name="answers">Possible answers the user can select in response to the message.</param>
        /// <returns>The answer the user selected.</returns>
        public static string MessageUser(string message, string caption, MessageBoxImage image, params string[] answers)
        {
            AskUserWindow dlg = new AskUserWindow();
            dlg.Message = message;
            dlg.Title = caption;
            dlg.Image = image;
            dlg.Answers = answers;
            IDE.ShowDialog(dlg);
            return dlg.Answer;
        }

        /// <summary>
        /// Register a function for global use.
        /// </summary>
        /// <param name="function">The function to register.</param>
        public void RegisterFunction(IFunction function)
        {
            if (function == null)
                throw new ArgumentNullException("function");

            if (_functions.ContainsKey(function.GetType()))
                _functions.Remove(function.GetType());

            _functions.Add(function.GetType(), function);
        }

        /// <summary>
        /// Get a function that has been registered previously.
        /// </summary>
        /// <typeparam name="TType">The type of the function to get.</typeparam>
        /// <returns>The function if found or null if not found.</returns>
        public TType GetFunction<TType>() where TType : IFunction
        {
            if (_functions.ContainsKey(typeof(TType)))
                return (TType)_functions[typeof(TType)];
            else
                throw new Exception("Function is not registered: " + typeof(TType).Name);
        }

        /// <summary>
        /// Remove a function that has been registered previously.
        /// </summary>
        /// <typeparam name="TType">The type of the function to remove.</typeparam>
        public void UnregisterFunction<TType>() where TType : IFunction
        {
            if (_functions.ContainsKey(typeof(TType)))
                _functions.Remove(typeof(TType));
        }

        /// <summary>
        /// Raises the ApplicationClosing event.
        /// </summary>
        /// <param name="e">Arguments to pass with the event.</param>
        protected virtual void OnApplicationClosing(CancelEventArgs e)
        {
            if (ApplicationClosing != null)
                ApplicationClosing(this, e);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Notify apps of application exit.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                CancelEventArgs cArgs = new CancelEventArgs();
                OnApplicationClosing(cArgs);
                e.Cancel = cArgs.Cancel;

                if (!e.Cancel)
                {
                    Wallace.IDE.Properties.Settings.Default.LastWindowState = _window.WindowState;
                    Wallace.IDE.Properties.Settings.Default.Save();
                }
            }
            catch (Exception err)
            {
                IDE.HandleException(err);
            }
        }

        /// <summary>
        /// Process hot keys.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                foreach (IFunction function in _functions.Values)
                {
                    if (function.HotKey != null && 
                        function.HotKey.IsMatch(e) &&
                        function.IsEnabled)
                    {
                        function.Execute();
                    }
                }
            }
            catch (Exception err)
            {
                IDE.HandleException(err);
            }            
        }

        /// <summary>
        /// Issue update to all workspaces.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void Navigation_ActiveNodeChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateWorkspaces();
            }
            catch (Exception err)
            {
                IDE.HandleException(err);
            }
        }

        /// <summary>
        /// Issue update to all workspaces.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void Content_ActiveDocumentChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateWorkspaces();
            }
            catch (Exception err)
            {
                IDE.HandleException(err);
            }
        }

        /// <summary>
        /// Handle the exception that occured.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            IDE.HandleException(e.Exception);
            e.Handled = true;
        }        

        #endregion

        #region Events

        /// <summary>
        /// Raised when the application is being closed.
        /// </summary>
        public event CancelEventHandler ApplicationClosing;

        #endregion
    }
}
