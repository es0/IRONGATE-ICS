// Decompiled with JetBrains decompiler
// Type: Step7ProSim.Logger
// Assembly: Step7ProSim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 244CE799-C2D9-4B20-BDBA-ED6AC3DE845C
// Assembly location: Z:\warez\IRONGATE\scada\Step7ProSim.dll

using Step7ProSim.Interfaces;
using System.IO;

namespace Step7ProSim
{
  internal class Logger : ILogger
  {
    private readonly string logFilePath;

    public bool Enabled { get; set; }

    public Logger(string pathToLogFile)
    {
      this.logFilePath = pathToLogFile;
      this.Enabled = true;
    }

    public void Log(string message)
    {
      if (!this.Enabled)
        return;
      using (StreamWriter streamWriter = File.AppendText(this.logFilePath))
        streamWriter.WriteLine(message);
    }
  }
}
