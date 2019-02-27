using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Form.Core.Configuration;
using Sitecore.Form.Core.Media;
using Sitecore.Form.Core.Pipelines.FormUploadFile;
using Sitecore.Globalization;
using Sitecore.Pipelines.Upload;
using Sitecore.Publishing;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;

namespace Sitecore.Support.Form.Core.Pipelines.FormUploadFile
{
  public class Save
  {
    public void Process(FormUploadFileArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(args.File, "file");
      if (!string.IsNullOrEmpty(args.File.FileName))
      {
        SecurityDisabler disabler = null;
        if (!args.UseSecurity)
        {
          disabler = new SecurityDisabler();
        }
        try
        {
          List<MediaUploadResultEx> list = new MediaUploaderEx
          {
            File = args.File,
            Unpack = false,
            Folder = args.Folder,
            Versioned = args.Versioned,
            Language = args.Language,
            AlternateText = args.GetFileParameter("alt"),
            Overwrite = args.Overwrite,
            Database = StaticSettings.MasterDatabase,
            FileBased = args.Destination == UploadDestination.File
          }.Upload();
          string[] parameters = new string[] { args.File.FileName };
          Log.Audit(this, "Upload: {0}", parameters);
          foreach (MediaUploadResultEx ex in list)
          {
            this.ProcessItem(args, ex.Item, ex.Path);
          }
        }
        catch (Exception exception)
        {
          Log.Error("Could not save posted file: " + args.File.FileName, exception, this);
          throw;
        }
        finally
        {
          if (disabler != null)
          {
            disabler.Dispose();
          }
        }
      }
    }

    private void ProcessItem(FormUploadFileArgs args, MediaItem mediaItem, string path)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.ArgumentNotNull(mediaItem, "mediaItem");
      Assert.ArgumentNotNull(path, "path");
      if (mediaItem != null)
      {
        if (args.Destination == UploadDestination.Database)
        {
          Log.Info("Media Item has been uploaded to database: " + path, this);
        }
        else
        {
          Log.Info("Media Item has been uploaded to file system: " + path, this);
        }
      }
      else
      {
        Log.Error("Failed to create Media Item in database: " + path, this);
      }

      PublishManager.PublishItem(mediaItem.InnerItem, ExtractDatabases(), ExtractLanguages(), false, true);

      args.Result = mediaItem.InnerItem.Uri.ToString().Replace("?db=master", string.Empty);
    }

    private Database[] ExtractDatabases()
    {
      List<Database> databases = new List<Database>();
      var databasesSetting = Configuration.Settings.GetSetting("Wffm.DatabasesForMediaUpload");
      var databasesNames = databasesSetting.Replace(" ", string.Empty).Split(',');

      foreach (string dbName in databasesNames)
      {
        try
        {
          databases.Add(Database.GetDatabase(dbName));
        }
        catch (Exception ex)
        {
          Log.Warn($"[WFFM] database {dbName} is not found.", this);
        }
      }

      return databases.ToArray();
    }

    private Language[] ExtractLanguages()
    {
      List<Language> languages = new List<Language>();

      var languageSetting = Configuration.Settings.GetSetting("Wffm.LanguagesForMediaUpload");
      var languageNames = languageSetting.Replace(" ", string.Empty).Split(',');

      foreach (string lang in languageNames)
      {
        try
        {
          languages.Add(Language.Parse(lang));
        }
        catch (Exception ex)
        {
          Log.Warn($"[WFFM] language {lang} cannot be parsed.", this);
        }
      }

      return languages.ToArray();
    }
  }
}