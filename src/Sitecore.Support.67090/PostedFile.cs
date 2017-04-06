using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sitecore.Support.Form.Core.Media
{
    public class PostedFile: Sitecore.Form.Core.Media.PostedFile    
    {
        public PostedFile()
        {
        }

        public PostedFile(byte[] data, string fileName, string destination) : base(data,fileName,destination)
        {
            
        }

        public override string ToString()
        {
            return  this.Destination +"/"+this.FileName;
        }
    }
}