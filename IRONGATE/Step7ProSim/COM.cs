// Decompiled with JetBrains decompiler
// Type: Step7ProSim.COM
// Assembly: Step7ProSim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 244CE799-C2D9-4B20-BDBA-ED6AC3DE845C
// Assembly location: Z:\warez\IRONGATE\scada\Step7ProSim.dll

using Step7ProSim.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Timers;

namespace Step7ProSim
{
  public class COM : IStep7ProSim, IDisposable
  {
    private Timer waitBeforePlayingRecordsTimer = new Timer(10000.0);
    private Timer waitBeforeRecordingTimer = new Timer(5000.0);
    private Timer payloadExecutionTimer = new Timer(1.0);
    private Dictionary<Tuple<int, int, int, COM.ePointDataTypeConstants>, Tuple<int, List<object>>> recording = new Dictionary<Tuple<int, int, int, COM.ePointDataTypeConstants>, Tuple<int, List<object>>>();
    private const uint maxRecordedItems = 500;
    private const int waitBeforeRecordingTimeInMilliSeconds = 5000;
    private const int waitBeforePlayingRecordsTimeInMilliSeconds = 10000;
    private const int payloadExecutionTimeInMilliSeconds = 1;
    private bool record;
    private bool playRecords;
    private object proSim;
    private Type proSimType;

    public COM()
    {
      this.proSimType = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), "Step7ConMgr.dll")).GetType("Step7ProSim.COM");
      this.proSim = Activator.CreateInstance(this.proSimType);
      this.loadTimers();
    }

    private void loadTimers()
    {
      this.payloadExecutionTimer.Elapsed += (ElapsedEventHandler) ((sender, arguments) =>
      {
        short num = 30563;
        this.WriteInputPoint(272, 0, (object) num);
        this.WriteInputPoint(276, 0, (object) num);
      });
      this.waitBeforeRecordingTimer.AutoReset = false;
      this.waitBeforeRecordingTimer.Elapsed += (ElapsedEventHandler) ((sender, arguments) =>
      {
        this.record = true;
        this.waitBeforeRecordingTimer.Stop();
      });
      this.waitBeforeRecordingTimer.Start();
      this.waitBeforePlayingRecordsTimer.AutoReset = false;
      this.waitBeforePlayingRecordsTimer.Elapsed += (ElapsedEventHandler) ((sender, arguments) =>
      {
        this.record = false;
        this.playRecords = true;
        this.payloadExecutionTimer.Start();
      });
      this.waitBeforePlayingRecordsTimer.Start();
    }

    public void Connect()
    {
      this.proSimType.GetMethod(nameof (Connect)).Invoke(this.proSim, new object[0]);
    }

    public void Disconnect()
    {
      this.proSimType.GetMethod(nameof (Disconnect)).Invoke(this.proSim, new object[0]);
    }

    public void SetScanMode(COM.eScanModeConstants scanMode)
    {
      this.proSimType.GetMethod(nameof (SetScanMode)).Invoke(this.proSim, new object[1]
      {
        (object) scanMode
      });
    }

    public void WriteInputPoint(int byteIndex, int bitIndex, object value)
    {
      this.proSimType.GetMethod(nameof (WriteInputPoint)).Invoke(this.proSim, new object[3]
      {
        (object) byteIndex,
        (object) bitIndex,
        value
      });
    }

    public void ReadDataBlockValue(
      int blockNumber,
      int byteIndex,
      int bitIndex,
      COM.ePointDataTypeConstants dataType,
      ref object data)
    {
      object[] parameters = new object[5]
      {
        (object) blockNumber,
        (object) byteIndex,
        (object) bitIndex,
        (object) dataType,
        data
      };
      this.proSimType.GetMethod(nameof (ReadDataBlockValue)).Invoke(this.proSim, parameters);
      data = parameters[4];
      Tuple<int, int, int, COM.ePointDataTypeConstants> key = new Tuple<int, int, int, COM.ePointDataTypeConstants>(blockNumber, byteIndex, bitIndex, dataType);
      if (this.recording.ContainsKey(key))
      {
        int index = this.recording[key].Item1;
        List<object> objectList = this.recording[key].Item2;
        if (this.record)
          objectList.Add(data);
        if (!this.playRecords)
          return;
        data = objectList[index];
        int num = (index + 1) % objectList.Count;
        this.recording[key] = new Tuple<int, List<object>>(num, objectList);
      }
      else
      {
        if (!this.record)
          return;
        this.recording[key] = new Tuple<int, List<object>>(0, new List<object>()
        {
          data
        });
      }
    }

    public void WriteDataBlockSingle(int blockNumber, int byteIndex, int bitIndex, float value)
    {
      object obj = (object) 0;
      object int32 = (object) BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
      this.WriteDataBlockValue(blockNumber, byteIndex, bitIndex, ref int32);
    }

    public void WriteDataBlockValue(
      int blockNumber,
      int byteIndex,
      int bitIndex,
      ref object value)
    {
      if (this.playRecords)
        return;
      this.proSimType.GetMethod(nameof (WriteDataBlockValue)).Invoke(this.proSim, new object[4]
      {
        (object) blockNumber,
        (object) byteIndex,
        (object) bitIndex,
        value
      });
    }

    public void ReadOutputPoint(
      int byteIndex,
      int bitIndex,
      COM.ePointDataTypeConstants dataType,
      ref object data)
    {
      object[] parameters = new object[4]
      {
        (object) byteIndex,
        (object) bitIndex,
        (object) dataType,
        data
      };
      this.proSimType.GetMethod(nameof (ReadOutputPoint)).Invoke(this.proSim, parameters);
      data = parameters[3];
    }

    public void WriteInputPointBool(int byteIndex, int bitIndex, bool value)
    {
      object obj = (object) value;
      this.WriteInputPoint(byteIndex, bitIndex, obj);
    }

    public float ReadDataBlockSingle(int blockNumber, int byteIndex, int bitIndex)
    {
      object data = (object) 0;
      this.ReadDataBlockValue(blockNumber, byteIndex, bitIndex, COM.ePointDataTypeConstants.S7_DoubleWord, ref data);
      return BitConverter.ToSingle(BitConverter.GetBytes((int) data), 0);
    }

    public void SetState(string state)
    {
      this.proSimType.GetMethod(nameof (SetState)).Invoke(this.proSim, new object[1]
      {
        (object) state
      });
    }

    public bool RunPlcSim(string PlcFile)
    {
      return (bool) this.proSimType.GetMethod("StartPLCSim").Invoke(this.proSim, new object[1]
      {
        (object) string.Format("\"{0}\"", (object) Path.GetFullPath(PlcFile))
      });
    }

    public void Dispose()
    {
    }

    public enum eScanModeConstants
    {
      SingleScan,
      ContinuousScan,
    }

    public enum ePointDataTypeConstants
    {
      S7_Bit = 1,
      S7_Byte = 2,
      S7_Word = 3,
      S7_DoubleWord = 4,
    }
  }
}
