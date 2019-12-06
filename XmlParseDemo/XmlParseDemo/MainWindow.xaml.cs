using System.Windows;
using System.Windows.Controls;
using XmlParseDemo.ViewModel;

namespace XmlParseDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            mainWindowViewModel = this.DataContext as MainWindowViewModel;
        }

        /// <summary>
        /// 读取xml文件并显示到treeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel.ReadXml();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ItemTreeData treeViewItem = (ItemTreeData)(treeView.SelectedItem);
            if (treeViewItem == null)
            {
                return;
            }
            mainWindowViewModel.SelectItemChange(treeViewItem.itemId);
        }
    }
}
