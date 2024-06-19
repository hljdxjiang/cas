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
using System.Collections.Generic;
using TCS.Entity;

namespace TCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataProcess : Page
    {
        private String file;

        private FileProcessService fileProcessService;

        private OxyplotService oxyplotService;

        public DataProcess()
        {
            InitializeComponent();
            fileProcessService = new FileProcessService();
            oxyplotService = new OxyplotService();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = new OpenFileDialog();
            if (result.ShowDialog() == true) {

                filePath.Content = "文件路径:"+result.FileName;
                file = result.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (file.Equals(String.Empty))
            {
                var mbx = MessageBox.Show("请选择要处理的文件", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (mbx == MessageBoxResult.Yes) {
                    Button_Click(sender, e);
                }
            }
            if (weight.Text.Equals(String.Empty)) {
                var mbx = MessageBox.Show("请输入样品质量", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (mbx == MessageBoxResult.Yes)
                {
                    weight.Focus();
                }
            }
            String fileId = null;
            String wt = weight.Text;
            PlotModel plotModel = null;
            List<DataGridItem> list = null;
            fileProcessService.process(file, wt, out fileId);
            oxyplotService.buildPlotModel(fileId, out plotModel,out list,out wt);
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
                var fileName = System.IO.Path.GetFileName(file) + ".svg";
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
    }
}