using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using TCS.EFCore;

namespace TCS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 调用初始化数据库的方法
            DatabaseInitializer.init();

            deleteHistoryData();
            // 继续应用程序启动逻辑
        }

        /**
         * 
         * 启动时删除历史数据，只保留最近的100条
         */
        private void deleteHistoryData() {
            using (var context = new TCSDbContext()) {
                //查询要保存的数据
                var keepList = context.FileInfos.OrderByDescending(e => e.CreateTime).Take(100).Select(e=>e.FileId).ToList();
                //要删除的记录
                var fileToDelete = context.FileInfos
                    .Where(e => !keepList.Contains(e.FileId))  // 排除要保留的记录
                    .ToList();
                //删除记录
                context.FileInfos.RemoveRange(fileToDelete);

                //删除详情
                var recordToDelete=context.RecordInfos.Where(e => !keepList.Contains(e.FileId))  // 排除要保留的记录
                    .ToList();
                context.RecordInfos.RemoveRange(recordToDelete);
            }
        }
    }

}
