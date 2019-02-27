using Sitecore.Form.Web.UI.Controls;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Form.Core.Attributes;
using Sitecore.Form.Core.Visual;
using Sitecore.WFFM.Abstractions.Actions;
using Sitecore.Support.Form.Web.UI.Adapters;

namespace Sitecore.Support.Form.Web.UI.Controls
{
  [Adapter(typeof(FileUploadAdapter))]
  public class UploadFile : ValidateControl, IHasTitle
  {
    #region Fields

    private static readonly string baseCssClassName = "scfFileUploadBorder";

    protected Panel generalPanel = new Panel();
    protected System.Web.UI.WebControls.Label title = new System.Web.UI.WebControls.Label();
    protected FileUpload upload = new FileUpload();
    private string uploadDir = "/sitecore/media library";

    #endregion

    #region Methods

    public UploadFile(HtmlTextWriterTag tag)
       : base(tag)
    {
      CssClass = baseCssClassName;
    }

    public UploadFile()
       : this(HtmlTextWriterTag.Div)
    {
    }

    public override void RenderControl(HtmlTextWriter writer)
    {
      DoRender(writer);
    }

    protected virtual void DoRender(HtmlTextWriter writer)
    {
      base.RenderControl(writer);
    }

    protected override void OnInit(EventArgs e)
    {
      upload.CssClass = "scfFileUpload";
      help.CssClass = "scfFileUploadUsefulInfo";
      title.CssClass = "scfFileUploadLabel";
      title.AssociatedControlID = upload.ID;
      generalPanel.CssClass = "scfFileUploadGeneralPanel";

      Controls.AddAt(0, generalPanel);
      Controls.AddAt(0, title);

      generalPanel.Controls.AddAt(0, help);
      generalPanel.Controls.AddAt(0, upload);
    }

    #endregion

    #region Properties

    public override string ID
    {
      get
      {
        return upload.ID;
      }
      set
      {
        title.ID = value + "text";
        upload.ID = value;
        base.ID = value + "scope";
      }
    }

    [VisualProperty("Upload To:", 0), DefaultValue("/sitecore/media library"), VisualCategory("Upload"),
     VisualFieldTypeAttribute(typeof(SelectDirectoryField)), NotNull]
    public string UploadTo
    {
      set
      {
        uploadDir = value;
      }
      get
      {
        return uploadDir;
      }
    }

    public override ControlResult Result
    {
      get
      {
        if (upload.HasFile)
        {
          return new ControlResult(ControlName,
                                   new PostedFile(upload.FileBytes, upload.FileName, UploadTo),
                                   "medialink")
          {
            AdaptForAnalyticsTag = false
          };
        }

        return new ControlResult(ControlName, null, string.Empty);
      }
      set
      {

      }
    }

    #region IHasTitle Members

    public string Title
    {
      set
      {
        title.Text = value;
      }
      get
      {
        return title.Text;
      }
    }

    #endregion

    [DefaultValue("scfFileUploadBorder")]
    [VisualProperty("CSS Class:", 600), VisualFieldType(typeof(CssClassField))]
    public new string CssClass
    {
      get
      {
        return base.CssClass;
      }
      set
      {
        base.CssClass = value;
      }
    }

    protected override Control ValidatorContainer
    {
      get
      {
        return this;
      }
    }

    protected override Control InnerValidatorContainer
    {
      get
      {
        return generalPanel;
      }
    }

    #endregion
  }
}