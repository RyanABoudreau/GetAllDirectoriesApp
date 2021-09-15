using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAllDirectoriesApp
{
    
        public class Folder
        {
            public string name { get; set; }
            public string path { get; set; }
            public long size { get; set; }


            public Folder(string aName, string aPath, long aSize)
            {
                name = aName;
                path = aPath;
                size = aSize;


            }



        }

    
}
