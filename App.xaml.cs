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

            // 继续应用程序启动逻辑
        }
    }

}
