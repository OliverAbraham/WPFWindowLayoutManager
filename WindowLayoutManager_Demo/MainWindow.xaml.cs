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
			_layoutManager = new WindowLayoutManager(window:this, key:"MainWindow");
			InitializeComponent();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_layoutManager.Save();
		}
	}
}
