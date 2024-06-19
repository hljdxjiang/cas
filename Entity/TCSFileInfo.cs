using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCS.Entity
{
    public class TCSFileInfo
    {
        public int ID { get; set; }

        public required string FileId { get; set; }

        public required string FileName { get; set; }

        public required String Weight { get; set; }

        public DateTime CreateTime { get; set; }

        public string ?UserID { get; set; }

        public int SampleCnt { get; set; }
    }
}
