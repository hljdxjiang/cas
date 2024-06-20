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
using TCS.Services;
using TCS.Entity;

namespace TCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HistoryWIndow : Window
    {
        public String fileId;


        private OxyplotService oxyplotService;


        public HistoryWIndow()
        {
            InitializeComponent();
            oxyplotService = new OxyplotService();
            
          }

        private void History_Loaded(object sender, RoutedEventArgs e)
        {
            Process();
        }

        private void Process()
        {
            PlotModel plotModel = null;
            List<DataGridItem> list = null;
            String weight = null;
            oxyplotService.buildPlotModel(fileId, out plotModel, out list, out weight);
            SegmentWeight.Content = "样品重量:" + weight;
            plotView.Model = plotModel;
            var nextButtons = new StackPanel();
            nextButtons.Orientation = Orientation.Horizontal;
            nextButtons.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            //下载按钮
            Button downButtoon = new Button
            {
                Content = "下载",
                Margin = new Thickness(5)
            };
            //下载按钮绑定方法
            downButtoon.Click += (sender, args) => download_click(plotModel);
            nextButtons.Children.Add(downButtoon);
            btns.Children.Clear();
            btns.Children.Add(nextButtons);
            dataGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            dataGrid.Height = 450;
            dataGrid.ItemsSource = list;


        }

        private async void download_click(PlotModel plotModel)
        {
            var result = new OpenFolderDialog();
            if (result.ShowDialog() ==true)
            {
                var fileName = System.IO.Path.GetFileName("") + ".svg";
                var outPath = result.FolderName;

                if (!Directory.Exists(outPath))
                {
                    Directory.CreateDirectory(outPath);
                }
                // 创建 SVG 渲染器
                var svgExporter = new OxyPlot.Wpf.SvgExporter { Width = 800, Height = 600 };

                // 将 PlotModel 导出为 SVG 字符串
                var svgString = svgExporter.ExportToString(plotModel);

                System.IO.File.WriteAllText(System.IO.Path.Combine(outPath, fileName), svgString);

                MessageBox.Show("下载成功，文件路径：" + outPath,"提示", MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        /**
        *
        * 点击关闭按钮
        */
        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}