using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Timers;
using System.Windows.Threading;

namespace Abraham.WPFWindowLayoutManager
{
	/// <summary>
	/// Nuget package to easily save and restore Window positions
	/// The library can save attributes of WPF Windows and DataGrids (column widths, orders etc)
	/// 
	/// This package is hosted on github at https://github.com/OliverAbraham/WPFWindowLayoutManager.
	/// 
	/// Oliver Abraham
	/// www.oliver-abraham.de
	/// mail@oliver-abraham.de
	/// </summary>
	public class WindowLayoutManager
	{
		#region ------------- Properties ----------------------------------------------------------
		#endregion



		#region ------------- Fields --------------------------------------------------------------
        private DTO _data;
        private string _filename;
        private Window _mainWindow;
        private Dispatcher _dispatcher;
        private string _key;
        private bool _dataWasLoaded;
        private bool _disableAllSaving;
        private Timer _maximizeTimer;
        private const string DEFAULT_FILENAME = "UI-Layout.xml";
		#endregion



		#region ------------- Init ----------------------------------------------------------------
        public WindowLayoutManager()
        {
            _mainWindow = null;
            _data = new DTO();
            _filename = Environment.CurrentDirectory + Path.DirectorySeparatorChar + DEFAULT_FILENAME;
        }

        public WindowLayoutManager(string dateiname)
        {
            _mainWindow = null;
            _data = new DTO();
            _filename = dateiname;
        }

        public WindowLayoutManager(Window window, string key, Dispatcher dispatcher)
        {
            _mainWindow = window;
            _dispatcher = dispatcher;
            _key = key;

            _mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            _mainWindow.Loaded  += Window_Loaded;
            _mainWindow.Closing += Window_Closing;
            _mainWindow.Closed  += Window_Closed;
            _data = new DTO();
            _filename = Environment.CurrentDirectory + Path.DirectorySeparatorChar + DEFAULT_FILENAME;

            Load();
            if (_dataWasLoaded)
                RestoreSizeAndPosition(_mainWindow, _key);
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        #region ------------ Persistence --------------------------------------
        public void Save()
		{
            if (_disableAllSaving)
                return;
            var serializer = new XmlSerializer(typeof(DTO));
            var fs = new FileStream(_filename, FileMode.Create);
            serializer.Serialize(fs, _data);
            fs.Close();
		}

        public bool Load()
        {
            if (!File.Exists(_filename))
                return false;
            var serializer = new XmlSerializer(typeof(DTO));
            var fs = new FileStream(_filename, FileMode.Open);
            var temp = (DTO)serializer.Deserialize(fs);
            fs.Close();

            if (temp == null)
                return false;

            _data = temp;
            _dataWasLoaded = true;
            return true;
        }
		#endregion
		#region ------------ Common -------------------------------------------
		public bool Reset()
        {
            if (!File.Exists(_filename))
                return false;
            File.Delete(_filename);
            return true;
        }

        public void Disable()
        {
            _disableAllSaving = true;
        }
        #endregion
        #region ------------ WPF Window size and position ---------------------
        public void SaveSizeAndPosition(Window ctl, string key = "MainWindow")
        {
            LayoutElement e = FindOrCreateElement(key);
            e.Left   = (int)ctl.Left;
            e.Top    = (int)ctl.Top;
            e.Width  = (int)ctl.Width;
            e.Height = (int)ctl.Height;
            e.State  = ctl.WindowState.ToString();
        }

        public void RestoreSizeAndPosition(Window ctl, string key = "MainWindow")
        {
            LayoutElement e = FindElement(key);
            if (e == null)
                return;
            ctl.Left   = e.Left;
            ctl.Top    = e.Top;
            ctl.Width  = e.Width;
            ctl.Height = e.Height;
            if (e.State == "Maximized")
            {
                if (key == "MainWindow")
                    WaitAndThenMaximize();
                else
                    ctl.WindowState = WindowState.Maximized;
            }
            else if (e.State == "Minimized")
            {
                ctl.WindowState = WindowState.Minimized;
            }
        }

        public void RestoreWindowPosition(Window ctl, string key = "MainWindow")
        {
            LayoutElement e = FindElement(key);
            if (e == null)
                return;
            ctl.Left   = e.Left;
            ctl.Top    = e.Top;
        }

        public void RemoveWindowBorder(Window ctl)
        {
            ctl.WindowStyle = WindowStyle.SingleBorderWindow;
        }
        #endregion
        #region ------------ Controls -----------------------------------------
        public void SaveControls(List<Control> controls, string key)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                var individualKey = $"{key}_{i}";

                if (controls[i] is TextBox     textBox    ) SaveTextBox    (textBox    , individualKey);
                if (controls[i] is CheckBox    checkBox   ) SaveCheckBox   (checkBox   , individualKey);
                if (controls[i] is RadioButton radiobutton) SaveRadiobutton(radiobutton, individualKey);
            }
        }

        public void RestoreControls(List<Control> controls, string key)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                var individualKey = $"{key}_{i}";

                if (controls[i] is TextBox     textBox    ) RestoreTextBox    (textBox    , individualKey);
                if (controls[i] is CheckBox    checkBox   ) RestoreCheckBox   (checkBox   , individualKey);
                if (controls[i] is RadioButton radiobutton) RestoreRadiobutton(radiobutton, individualKey);
            }
        }

        public void SaveTextBox(TextBox control, string key)
        {
            SaveTextValue(control.Text, key);
        }

        public void RestoreTextBox(TextBox control, string key)
        {
            var value = GetTextValue(key);
            if (!string.IsNullOrWhiteSpace(value))
                control.Text = value;
        }

        public void SaveCheckBox(CheckBox control, string key)
        {
            var value = control.IsChecked == true ? "1" : "0";
            SaveTextValue(value, key);
        }

        public void RestoreCheckBox(CheckBox control, string key)
        {
            var value = GetTextValue(key);
            if (!string.IsNullOrWhiteSpace(value))
                control.IsChecked = (value == "1");
        }

        public void SaveRadiobutton(RadioButton control, string key)
        {
            var value = control.IsChecked == true ? "1" : "0";
            SaveTextValue(value, key);
        }

        public void RestoreRadiobutton(RadioButton control, string key)
        {
            var value = GetTextValue(key);
            if (!string.IsNullOrWhiteSpace(value))
                control.IsChecked = (value == "1");
        }
        #endregion
        #region ------------ Single values ------------------------------------
        public void SaveValue(double value, string key = "SomeValue1")
        {
            LayoutElement e = FindOrCreateElement(key);
            e.Value = (int)value;
        }

		public bool ValueExists(string key = "SomeValue1")
        {
            LayoutElement e = FindElement(key);
            return e != null;
        }

        public double GetValue(string key = "SomeValue1")
        {
            LayoutElement e = FindElement(key);
            return e.Value;
        }

		public void SaveTextValue(string value, string key)
		{
            LayoutElement e = FindOrCreateElement(key);
            e.State = value;
		}

		public string GetTextValue(string key)
		{
            LayoutElement e = FindElement(key);
            if (e == null)
                return "";
            else
                return e.State;
		}
        #endregion
        #region ------------ DataGrids ----------------------------------------
        #region ------------ Column width -------------------------------------
        public void SaveListboxColumnWidths(DataGrid ctl, string key = "Listbox1")
        {
            LayoutElement e = FindOrCreateElement(key);
            e.Values = GetListboxColumnWidths(ctl);
        }

        public void RestoreListboxColumnWidths(DataGrid ctl, string key = "Listbox1")
        {
            LayoutElement e = FindElement(key);
            if (e == null)
                return;
            RestoreListboxColumnWidths(ctl, e.Values);
        }

        public void SaveListboxColumnWidths(GridView ctl, string key = "Listbox1")
        {
            LayoutElement e = FindOrCreateElement(key);
            e.Values = GetListboxColumnWidths(ctl);
        }

        public void RestoreListboxColumnWidths(GridView ctl, string key = "Listbox1")
        {
            LayoutElement e = FindElement(key);
            if (e == null)
                return;
            RestoreListboxColumnWidths(ctl, e.Values);
        }

        public List<int> GetListboxColumnWidths(DataGrid ctl)
        {
            List<int> widths = new List<int>();
            foreach (var column in ctl.Columns)
                widths.Add((int)column.Width.Value);
            return widths;
        }

        public void RestoreListboxColumnWidths(DataGrid ctl, List<int> widths)
        {
            if (widths == null || widths.Count == 0)
                return;

            int ColumnCount = ctl.Columns.Count;
            int Index = 0;
            foreach (var width in widths)
            {
                if (Index >= ColumnCount)
                    break;
                ctl.Columns[Index].Width = new DataGridLength(width);
                Index++;
            }
        }
        #endregion
        #region ------------ Column order -------------------------------------
        public void SaveListboxColumnOrder(DataGrid ctl, string key = "Listbox1")
        {
            LayoutElement e = FindOrCreateElement(key);
            e.Values = GetListboxColumnOrder(ctl);
        }

        public void RestoreListboxColumnOrder(DataGrid ctl, string key = "Listbox1")
        {
            LayoutElement e = FindElement(key);
            if (e == null)
                return;
            RestoreListboxColumnOrder(ctl, e.Values);
        }

        public void SaveListboxColumnOrder(GridView ctl, string key = "Listbox1")
        {
            LayoutElement e = FindOrCreateElement(key);
            e.Values = GetListboxColumnOrder(ctl);
        }

        public void RestoreListboxColumnOrder(GridView ctl, string key = "Listbox1")
        {
            LayoutElement e = FindElement(key);
            if (e == null)
                return;
            RestoreListboxColumnOrder(ctl, e.Values);
        }
        #endregion
        #region ------------ Sort direction -----------------------------------
		public void SaveListboxColumnSortDirections(DataGrid ctl, string key = "Listbox1_SortDirections")
		{
            LayoutElement e = FindOrCreateElement(key);
            e.Values = GetListboxSortDirections(ctl);
		}

        public void RestoreListboxColumnSortDirections(DataGrid control, string key = "Listbox1_SortDirections")
		{
            LayoutElement e = FindElement(key);
            if (e == null)
                return;

            int ColumnCount = control.Columns.Count;
            int Index = 0;
            foreach (var value in e.Values)
            {
                if (Index >= ColumnCount)
                    break;
                if (value != 0)
                    control.Columns[Index].SortDirection = DeserializeSortDirection(value);
                Index++;
            }
        }
        #endregion
        #endregion
        #endregion



        #region ------------- Implementation ------------------------------------------------------
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_dataWasLoaded)
                Load();

            if (_dataWasLoaded)
                RestoreSizeAndPosition(_mainWindow, _key);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"SaveSizeAndPosition: {_mainWindow.Left} {_mainWindow.Top} {_mainWindow.Width} {_mainWindow.Height}  {_mainWindow.WindowState}");
            SaveSizeAndPosition(_mainWindow, _key);
        }

		private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow.Loaded  -= Window_Loaded;
            _mainWindow.Closing -= Window_Closing;
            _mainWindow.Closed  -= Window_Closed;
            Save();
        }

        private List<int> GetListboxSortDirections(DataGrid ctl)
        {
            var orders = new List<int>();
            foreach (DataGridColumn c in ctl.Columns)
                orders.Add( SerializeSortDirection(c.SortDirection));
            return orders;
        }

        private int SerializeSortDirection(ListSortDirection? sortDirection)
        {
            if (sortDirection == null)
                return 0;
            switch (sortDirection)
            {
                case ListSortDirection.Ascending: return 1;
                case ListSortDirection.Descending: return -1;
                default: return 0;
            }
        }

        private ListSortDirection DeserializeSortDirection(int value)
        {
            switch (value)
            {
                case 1: return ListSortDirection.Ascending;
                case 2: return ListSortDirection.Descending;
                default: return ListSortDirection.Ascending;
            }
        }

        private List<int> GetListboxColumnWidths(GridView ctl)
        {
            var widths = new List<int>();
            foreach (var column in ctl.Columns)
                widths.Add((int)column.Width);
            return widths;
        }

        private void RestoreListboxColumnWidths(GridView ctl, List<int> widths)
        {
            if (widths == null || widths.Count == 0)
                return;

            int columnCount = ctl.Columns.Count;
            int index = 0;
            foreach (var width in widths)
            {
                if (index >= columnCount)
                    break;
                ctl.Columns[index].Width = width;
                index++;
            }
        }

        private List<int> GetListboxColumnOrder(DataGrid control)
        {
            var orders = new List<int>();
            foreach (var column in control.Columns)
                orders.Add(column.DisplayIndex);
            return orders;
        }

        private void RestoreListboxColumnOrder(DataGrid control, List<int> displayIndices)
        {
            if (displayIndices == null || displayIndices.Count == 0)
                return;

            var columnCount = control.Columns.Count;
            var index = 0;
            foreach (var displayIndex in displayIndices)
            {
                if (index >= columnCount)
                    break;
                control.Columns[index].DisplayIndex = displayIndex;
                index++;
            }
        }

        private List<int> GetListboxColumnOrder(GridView control)
        {
            var widths = new List<int>();
            //foreach (var column in control.Columns)
            //    widths.Add(column.);
            return widths;
        }

        private void RestoreListboxColumnOrder(GridView control, List<int> widths)
        {
            //if (widths == null || widths.Count == 0)
            //    return;

            //var columnCount = control.Columns.Count;
            //var index = 0;
            //foreach (int width in widths)
            //{
            //    if (index >= columnCount)
            //        break;
            //    control.Columns[Index].Width = width;
            //    index++;
            //}
        }
       
        private LayoutElement FindOrCreateElement(string key)
        {
            LayoutElement e = FindElement(key);
            if (e == null)
            {
                _data.Elements.Add(new LayoutElement(key));
                e = FindElement(key);
            }
            return e;
        }

        private LayoutElement FindElement(string key)
        {
            foreach (var element in _data.Elements)
            {
                if (element.Key == key)
                    return element;
            }
            return null;
        }

        private void WaitAndThenMaximize()
        {
            _maximizeTimer = new Timer();
            _maximizeTimer.Interval = 100;
            _maximizeTimer.Elapsed += MaximizeWindow;
            _maximizeTimer.AutoReset = false;
            _maximizeTimer.Start();
        }

        private void MaximizeWindow(object? sender, ElapsedEventArgs e)
        {
            _maximizeTimer.Stop();
            _dispatcher.Invoke(() =>
            {
                _mainWindow.WindowState = WindowState.Maximized;
            });
        }
        #endregion
    }
}
