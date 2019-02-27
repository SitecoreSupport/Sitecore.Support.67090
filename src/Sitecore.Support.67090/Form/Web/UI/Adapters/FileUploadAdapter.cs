using Sitecore.WFFM.Abstractions.Data;
using FileUploadAdapterOrigin = Sitecore.Form.UI.Adapters.FileUploadAdapter;

namespace Sitecore.Support.Form.Web.UI.Adapters
{
  public class FileUploadAdapter : FileUploadAdapterOrigin
  {
    public override string AdaptToFriendlyValue(IFieldItem field, string value)
    {
      return base.AdaptToFriendlyValue(field, value).Replace("?db=master", string.Empty);
    }
  }
}