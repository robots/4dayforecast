// Decompiled with JetBrains decompiler
// Type: Forecast.citydata
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System;

namespace Forecast
{
  public class citydata : ICloneable
  {
    public weathernode[] weatherbank = new weathernode[10];
    public string cityid;
    public string cityname;
    public string statename;
    public double citytimediff;
    public double citytimeoffset;
    public string cityDSTmode;
    public string countryname;
    public byte DSTEN;
    public bool DSTvalid;
    public int cur_con;
    public int cur_country;
    public int cur_state;
    public int cur_city;
    public DateTime NextDSTtime;

    public citydata()
    {
      this.cityid = "";
      this.cityname = "";
      this.statename = "";
      this.citytimediff = 0.0;
      this.citytimeoffset = 0.0;
      this.countryname = "";
      this.cityDSTmode = "X";
      this.DSTEN = (byte) 0;
      this.DSTvalid = false;
      this.cur_con = -1;
      this.cur_country = -1;
      this.cur_state = -1;
      this.cur_city = -1;
      this.NextDSTtime = new DateTime(2000, 1, 1);
      for (int index = 0; index < this.weatherbank.Length; ++index)
        this.weatherbank[index] = new weathernode();
    }

    protected citydata(citydata right)
    {
      this.cityid = right.cityid;
      this.cityname = right.cityname;
      this.statename = right.statename;
      this.citytimediff = right.citytimediff;
      this.citytimeoffset = right.citytimeoffset;
      this.cityDSTmode = right.cityDSTmode;
      this.countryname = right.countryname;
      this.DSTEN = right.DSTEN;
      this.DSTvalid = right.DSTvalid;
      this.cur_con = right.cur_con;
      this.cur_country = right.cur_country;
      this.cur_state = right.cur_state;
      this.cur_city = right.cur_city;
      this.NextDSTtime = right.NextDSTtime;
      for (int index = 0; index < this.weatherbank.Length; ++index)
        this.weatherbank[index] = new weathernode();
    }

    public object Clone()
    {
      return (object) new citydata(this);
    }

    public static void copy_wo_data(citydata scr, ref citydata dst)
    {
      dst.cityid = scr.cityid;
      dst.cityname = scr.cityname;
      dst.statename = scr.statename;
      dst.citytimediff = scr.citytimediff;
      dst.citytimeoffset = scr.citytimeoffset;
      dst.cityDSTmode = scr.cityDSTmode;
      dst.countryname = scr.countryname;
      dst.DSTEN = scr.DSTEN;
      dst.DSTvalid = scr.DSTvalid;
      dst.cur_con = scr.cur_con;
      dst.cur_country = scr.cur_country;
      dst.cur_state = scr.cur_state;
      dst.cur_city = scr.cur_city;
      dst.NextDSTtime = scr.NextDSTtime;
    }
  }
}
