// Decompiled with JetBrains decompiler
// Type: Forecast.weathernode
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

namespace Forecast
{
  public class weathernode
  {
    public int year;
    public int month;
    public int day;
    public byte[] forecastbytes;
    public byte[] pollen;

    public weathernode()
    {
      this.year = 1950;
      this.month = 1;
      this.day = 1;
      this.forecastbytes = new byte[25];
      for (int index = 0; index < this.forecastbytes.Length; ++index)
        this.forecastbytes[index] = byte.MaxValue;
      this.pollen = new byte[3];
      for (int index = 0; index < this.pollen.Length; ++index)
        this.pollen[index] = byte.MaxValue;
    }
  }
}
