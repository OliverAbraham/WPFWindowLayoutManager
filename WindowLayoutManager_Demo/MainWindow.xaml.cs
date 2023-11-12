using Abraham.WPFWindowLayoutManager;
using System.Windows;

namespace WindowLayoutManager_Demo
{
	/// <summary>
	/// Demo app to demonstrate save and restore of the window size and position.
	/// The library can save attributes of WPF Windows and DataGrids (column widths, orders etc)
	/// </summary>
	public partial class MainWindow : Window
	{
		private WindowLayoutManager _layoutManager;

		public MainWindow()
		{
			InitializeComponent();
			_layoutManager = new WindowLayoutManager(window:this, key:"MainWindow");
			_layoutManager.RestoreWindowPosition(this);
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			var wnd = new ChildWindow();
			wnd.LayoutManager = _layoutManager;
			wnd.ShowDialog();
        }
    }
}
