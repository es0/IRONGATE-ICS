
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PackagingModule
{
  public static class ProcessManager
  {
    public static void KillProcess(string processName)
    {
      processName = Path.GetFileNameWithoutExtension(processName);
      foreach (Process process in ((IEnumerable<Process>) Process.GetProcesses()).Where<Process>((Func<Process, bool>) (p => p.ProcessName.ToLower() == processName)))
      {
        try
        {
          process.Kill();
        }
        catch (Exception ex)
        {
        }
      }
    }

    public static void StartProcess(string filePath, string workingDirectory)
    {
      try
      {
        Process.Start(new ProcessStartInfo()
        {
          FileName = filePath,
          UseShellExecute = true,
          WorkingDirectory = workingDirectory
        });
      }
      catch (Exception ex)
      {
      }
    }
  }
}
