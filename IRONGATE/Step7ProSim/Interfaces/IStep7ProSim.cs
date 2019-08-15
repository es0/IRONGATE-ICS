namespace Step7ProSim.Interfaces
{
  public interface IStep7ProSim
  {
    void Connect();

    void Dispose();

    float ReadDataBlockSingle(int blockNumber, int byteIndex, int bitIndex);

    void ReadDataBlockValue(
      int blockNumber,
      int byteIndex,
      int bitIndex,
      COM.ePointDataTypeConstants dataType,
      ref object data);

    void ReadOutputPoint(
      int byteIndex,
      int bitIndex,
      COM.ePointDataTypeConstants dataType,
      ref object data);

    bool RunPlcSim(string PlcFile);

    void SetScanMode(COM.eScanModeConstants scanMode);

    void SetState(string state);

    void WriteDataBlockSingle(int blockNumber, int byteIndex, int bitIndex, float value);

    void WriteDataBlockValue(int blockNumber, int byteIndex, int bitIndex, ref object value);

    void WriteInputPoint(int byteIndex, int bitIndex, object value);

    void WriteInputPointBool(int byteIndex, int bitIndex, bool value);
  }
}
