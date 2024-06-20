using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCS.EFCore;
using TCS.Entity;
using TCS.Tools;
using static System.Net.WebRequestMethods;

namespace TCS.Services
{
    public class FileProcessService
    {
        /**
         * 
         * 处理图片并保存数据
         */
        public void process(String filePath,String weight,out String fileId)
        {
            //生成文件ID并返回
            fileId = SnowflakeIdGenerator.NextId().ToString();
            int lineCnt = 0;
            //是否开始处理
            bool isBegin = false;
            if (!filePath.Equals(String.Empty))
            {
                using (var context = new TCSDbContext()) {
    
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        String line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //查找标题行
                            if (line.StartsWith("Step,")) {
                                isBegin = true;
                                continue;
                            }
                            if (isBegin) {
                                lineCnt++;
                                processLine(line, fileId, context,lineCnt);
                            }
                        }
                    }
                    saveFileInfo(fileId, weight,filePath, lineCnt, context);
                    context.SaveChanges();
                }
            }
        }

        /**
         * 
         * 保存文件信息
         */
        private void saveFileInfo(String fileId,String weight,String filePath,int lineCnt,TCSDbContext dbContext) {
            TCSFileInfo fileInfo = new TCSFileInfo { 
                FileId = fileId,
                FileName=getFileName(filePath),
                Weight=weight,
                UserID = "Admin",
                CreateTime = DateTime.Now,
                SampleCnt = lineCnt,
            };
            dbContext.FileInfos.Add(fileInfo);
        }

        /**
         * 
         * 保存单行记录
         */
        private void processLine(String line, String fileId, TCSDbContext context,int seqno)
        {
            String[] strs = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length >= 8)
            {
                RecordInfo record = new RecordInfo
                {
                    FileId = fileId,
                    Step = strs[0],
                    Seqno=seqno,
                    Iteration = strs[1],
                    Segment = strs[2],
                    Field = strs[3],
                    Moment = strs[4],
                    TimeStamp = strs[5],
                    FieldStatus = strs[6],
                    MomentStatus = strs[7],
                };
                context.RecordInfos.Add(record);
            }
        }

        /**
         * 
         * 保存数据
         */
        private void saveData(String filePath, List<string> _list, int lineCnt)
        {

        }




        /**
         * 
         * 获取文件名
         */
        private string getFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }
    }
}
