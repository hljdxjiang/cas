using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCS.EFCore
{
    class DatabaseInitializer
    {
        public static string databasePath = "database.db";
        public static void init()
        {
            if (!File.Exists(databasePath))
            {
                using (var dbContext = new TCSDbContext())
                {
                    // 创建数据库和表（如果尚不存在）
                    dbContext.Database.EnsureCreated();
                    //执行数据迁移 暂时不需要
                    //dbContext.Database.Migrate();
                }
            }
        }
    }
}
