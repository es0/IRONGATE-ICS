

using System.IO;
using System.Reflection;

namespace PackagingModule
{
  public class DllProxyInstaller
  {
    public void InstallProxy(string folder, string dllFilename, string newDllFilename)
    {
      string str = Path.Combine(folder, dllFilename);
      File.Move(str, Path.Combine(folder, newDllFilename));
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PackagingModule.Step7ProSim.dll"))
      {
        using (FileStream fileStream = new FileStream(str, FileMode.Create))
          manifestResourceStream.CopyTo((Stream) fileStream);
      }
    }
  }
}
