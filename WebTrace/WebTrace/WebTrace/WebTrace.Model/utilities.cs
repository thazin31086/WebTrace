using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTrace.Model
{
    //JSON parsing methods
    public struct LinkFields
    {
        public String self;
    }

    public struct FileInfo
    {
        public String name;
        public String type;
        public String download_url;
        public LinkFields _links;
    }

    //Structs used to hold file data
    public struct FileData
    {
        public String name;
        public String contents;
    }
    public struct Directory
    {
        public String name;
        public List<Directory> subDirs;
        public List<FileData> files;
    }
}
