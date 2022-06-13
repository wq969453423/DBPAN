using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPAN.Model
{
    public class MainModel
    {

        public string category { get; set; }

        public string fs_id { get; set; }

        public string isdir { get; set; }

        public string local_ctime { get; set; }


        public string local_mtime { get; set; }

        public string server_ctime { get; set; }


        public string server_filename { get; set; }

        public string server_mtime { get; set; }
        public string size { get; set; }

        public string path { get; set; }


        public string folder { get; set; }

        public string tips { get; set; }

    }
}
