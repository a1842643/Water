using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WaterCaseTracking.Models
{
    public class SYS_announcementModel
    {
        public int? ID { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public string AttachedFile { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ReleaseDateStr { get; set; }

        public DateTime StopRelease { get; set; }

        public string StopReleaseStr { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreationUser { get; set; }

        public DateTime MODIFY_DATE { get; set; }

        public string MODIFY_USER { get; set; }
    }
}