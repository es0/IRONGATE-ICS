

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PackagingModule
{
  public class FileFinder
  {
    public List<string> FindFile(string filename)
    {
      List<string> files = new List<string>();
      foreach (DriveInfo driveInfo in ((IEnumerable<DriveInfo>) DriveInfo.GetDrives()).Where<DriveInfo>((Func<DriveInfo, bool>) (x => x.IsReady)))
        FileFinder.FindFileInDrive(driveInfo.RootDirectory.FullName, filename, files);
      return files;
    }

    private static void FindFileInDrive(string path, string filename, List<string> files)
    {
      filename = filename.ToLower();
      try
      {
        ((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>().ForEach((Action<string>) (f =>
        {
          if (!f.ToLower().EndsWith(filename))
            return;
          files.Add(f);
        }));
        ((IEnumerable<string>) Directory.GetDirectories(path)).ToList<string>().ForEach((Action<string>) (f => FileFinder.FindFileInDrive(f, filename, files)));
      }
      catch (UnauthorizedAccessException ex)
      {
      }
    }
  }
}
