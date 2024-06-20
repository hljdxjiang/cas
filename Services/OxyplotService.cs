using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCS.EFCore;
using TCS.Entity;
using TCS.Tools;
using OxyPlot.Series;
using Microsoft.EntityFrameworkCore;

namespace TCS.Services
{
    public class OxyplotService
    {

        /**
         * 
         * 根据图片生成数据
         */
        public void buildPlotModel(string fileId,out PlotModel plotModel,out List<DataGridItem>? list,out String weight) {
            buildDefaultPlotView("", out plotModel);
            list = new List<DataGridItem>();
            TCSFileInfo fileInfo = GetFileInfo(fileId);
            List<RecordInfo> recordInfos = GetRecordInfos(fileId);
            weight = fileInfo.Weight;
            if (recordInfos != null) {
                LineSeries lineSeries = new LineSeries
                {
                    Color = OxyColors.Red,
                    LineJoin = OxyPlot.LineJoin.Bevel,
                    StrokeThickness = 3,
                    //Title = "medium",
                    //MarkerType = MarkerType.Square, // 设置标记类型
                    //MarkerSize = 6,                 // 设置标记大小
                    //MarkerFill = OxyColors.Red    // 设置标记填充颜色
                };
                foreach (var info in recordInfos)
                {
                    if (info != null)
                    {
                        var xValue = Double.Parse(info.Field);
                        
                        var yvalue = Double.Parse(info.Moment) / Double.Parse(fileInfo.Weight) * 1000;
                        lineSeries.Points.Add(new DataPoint(xValue, yvalue));
                        list.Add(new DataGridItem
                        {
                            Field = xValue.ToString(),
                            Moment = yvalue.ToString()
                        });
                    }
                }
                //将线加入到图片上
                plotModel.Series.Add(lineSeries);
                
            }
        }



        /**
         * 
         * 创建初始化的图片模板
         */
        private void buildDefaultPlotView(string title, out PlotModel plotModel)
        {
            plotModel = new PlotModel { Title = title, Legends = { }, IsLegendVisible = true };
            var xares = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Field(T)"
            };
            var yares = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Moment(×10\u207B\u00B5Am\u00B2/kg)",
            };
            plotModel.Axes.Add(xares);
            plotModel.Axes.Add(yares);

            var xlegen = new Legend
            {
                LegendTitleColor = OxyColor.FromUInt32(666666),
                LegendPosition = LegendPosition.RightTop,
                LegendSymbolLength = 80,
                LegendItemSpacing = 40,
                LegendFontSize = 20,
                LegendOrientation = LegendOrientation.Horizontal,
            };
            plotModel.Legends.Add(xlegen);
        }

        /**
         * 
         * 查询单个文件的详情数据
         */
        private List<RecordInfo> GetRecordInfos(String fileId)
        {
            using (var context = new TCSDbContext())
            {
                return context.RecordInfos
                    .Where(x => x.FileId == fileId).ToList();
            }
        }

        /**
         * 
         * 查找记录
         */
        private TCSFileInfo GetFileInfo(String fileId) {
            using (var context = new TCSDbContext())
            {
                return context.FileInfos.FirstOrDefault(x => x.FileId == fileId);
            }
        }

    }

   
}
