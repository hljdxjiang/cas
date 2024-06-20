using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCS.Entity
{
    public class RecordInfo
    {
        public String ID { get; set; }
        public required string FileId { get; set; }
        public required String Step { get; set; }
        public  String ?Iteration { get; set; }
        public  String? Segment { get; set; }
        public  String? Field { get; set; }
        public  String? Moment { get; set; }
        public  String? TimeStamp { get; set; }

        public  String? Coefficient { get; set; }
        public  String? FieldStatus { get; set; }
        public  String? MomentStatus { get; set; }

        public required int Seqno { get; set; }

    }
}
