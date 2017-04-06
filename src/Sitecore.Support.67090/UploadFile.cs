using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Form.Core.Controls.Data;
using Sitecore.Support.Form.Core.Media;

namespace Sitecore.Support.Form.Web.UI.Controls
{
    public class UploadFile: Sitecore.Form.Web.UI.Controls.UploadFile
    {
        public override ControlResult Result
        {
            get
            {
                if (this.upload.HasFile)
                {
                    return new ControlResult(this.ControlName, new PostedFile(this.upload.FileBytes, this.upload.FileName, this.UploadTo), "medialink");
                }
                return new ControlResult(this.ControlName, null, string.Empty);
            }

            set
            {
               
            }
        }
    }
}