using Microsoft.Win32;
using OxyPlot.Wpf;
using OxyPlot;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using TCS.EFCore;
using TCS.Entity;
using TCS;
using System.Collections.Generic;

namespace IRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataHistory : Page
    {
        public DataHistory()
        {
            InitializeComponent();
        }

        private void loadList() {
            using (var context = new TCSDbContext())
            {
                DateTime currentDate = DateTime.Now;
                DateTime thirtyDaysAgo = currentDate.AddDays(-30);
                var list = context.FileInfos.Where(data => data.CreateTime >= thirtyDaysAgo).ToList();//只查30天以内的数据
                dataGrid.ItemsSource = list.ToArray();
                //TODO 给dataGrid绑定list；
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            loadList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认删除?删除后数据将无法恢复。请确认","警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) {
                // Get the selected item
                TCSFileInfo selectedItem = (TCSFileInfo)dataGrid.SelectedItem;
                using (var context = new TCSDbContext())
                {
                    context.FileInfos.Remove(selectedItem);
                    var list = context.RecordInfos.Where(item => item.FileId == selectedItem.FileId);
                    if (list.Any())
                    {
                        context.RecordInfos.RemoveRange(list);
                    }
                    context.SaveChanges();

                }
                loadList();
            }
            
        }
        

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item
            TCSFileInfo selectedItem = (TCSFileInfo)dataGrid.SelectedItem;
            HistoryWIndow historyWIndow = new HistoryWIndow();
            historyWIndow.fileId =selectedItem.FileId;
            historyWIndow.ShowDialog();
        }
    }
}