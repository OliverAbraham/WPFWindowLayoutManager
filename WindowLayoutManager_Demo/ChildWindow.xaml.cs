using Abraham.WPFWindowLayoutManager;
using System.Windows;

namespace WindowLayoutManager_Demo
{
    public partial class ChildWindow : Window
    {
        public WindowLayoutManager LayoutManager { get; internal set; }

        public ChildWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutManager.RestoreSizeAndPosition(this, "ChildWindow1");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LayoutManager.SaveSizeAndPosition(this, "ChildWindow1");
        }
    }
}
