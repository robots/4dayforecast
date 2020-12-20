// Decompiled with JetBrains decompiler
// Type: Forecast.usb_access
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Forecast
{
  public class usb_access
  {
    private object thisLock = new object();
    private int _job;
    private CUsb UsbObj;
    private double _citytimediff;
    private byte _DSTEN;
    private double _citytimeoffset;
    private TimeSpan _pc_utc_timediff;
    private bool _exitfg;
    private bool _success;
    private int _retrytime;
    private int _retrycount;
    private bool _locksuccess;
    private string _DataPath;
    private bool _gotinterrupt;
    private int _interruptcommand;
    private System.Windows.Forms.Timer usb_timer;

    public bool IsConnected
    {
      get
      {
        return this.UsbObj.IsConnected;
      }
      set
      {
      }
    }

    public string DataPath
    {
      get
      {
        return this._DataPath;
      }
      set
      {
        this._DataPath = value;
      }
    }

    public bool gotinterrupt
    {
      get
      {
        return this._gotinterrupt;
      }
      set
      {
        this._gotinterrupt = value;
      }
    }

    public int interruptcommand
    {
      get
      {
        return this._interruptcommand;
      }
      set
      {
        this._interruptcommand = value;
      }
    }

    public int job
    {
      get
      {
        return this._job;
      }
      set
      {
        this._job = value;
      }
    }

    public bool exitfg
    {
      get
      {
        return this._exitfg;
      }
      set
      {
        this._exitfg = value;
      }
    }

    public double citytimediff
    {
      get
      {
        return this._citytimediff;
      }
      set
      {
        this._citytimediff = value;
      }
    }

    public byte DSTEN
    {
      get
      {
        return this._DSTEN;
      }
      set
      {
        this._DSTEN = value;
      }
    }

    public double citytimeoffset
    {
      get
      {
        return this._citytimeoffset;
      }
      set
      {
        this._citytimeoffset = value;
      }
    }

    public TimeSpan pc_utc_timediff
    {
      get
      {
        return this._pc_utc_timediff;
      }
      set
      {
        this._pc_utc_timediff = value;
      }
    }

    public bool success
    {
      get
      {
        return this._success;
      }
      set
      {
        this._success = value;
      }
    }

    public int retrytime
    {
      get
      {
        return this._retrytime;
      }
      set
      {
        this._retrytime = value;
      }
    }

    public int retrycount
    {
      get
      {
        return this._retrycount;
      }
      set
      {
        this._retrycount = value;
      }
    }

    public bool locksuccess
    {
      get
      {
        return this._locksuccess;
      }
      set
      {
        this._locksuccess = value;
      }
    }

    public usb_access(string DataPath)
    {
      this._job = 0;
      this._exitfg = false;
      this._locksuccess = false;
      this._DataPath = DataPath;
      this.UsbObj = new CUsb(4400U, 26625U, 1024U, 1024U);
      this.usb_timer = new System.Windows.Forms.Timer();
      this.usb_timer.Tick += new EventHandler(this.usb_timer_Tick);
    }

    private void usb_timer_Tick(object sender, EventArgs e)
    {
      this.usb_timer.Stop();
      if (this._job != 1)
        return;
      this._job = 0;
    }

    public bool getsuccess()
    {
      lock (this.thisLock)
      {
        this._job = 0;
        return this._success;
      }
    }

    public void dofunc(object f_job)
    {
      if (!this.UsbObj.IsConnected)
        return;
      if ((int) f_job == 1)
      {
        lock (this.thisLock)
        {
          if (this._job == 0)
          {
            this._locksuccess = true;
            this._job = 1;
            this.usb_timer.Interval = 2000;
            this.usb_timer.Start();
          }
          else
            this._locksuccess = false;
        }
      }
      if (this._job == 0)
      {
        if ((int) f_job == 0)
        {
          this._job = 0;
          this._locksuccess = false;
        }
      }
      else if ((int) f_job == 0)
      {
        this._job = 0;
        this._locksuccess = false;
      }
      else if ((int) f_job == 2)
        this.Synclock();
      else if ((int) f_job == 3)
        this.UpdateWeatherRequest();
      else if ((int) f_job == 4)
        this.UpdateRecRequest();
      else if ((int) f_job == 5)
        this.SendData();
      else if ((int) f_job == 6)
        this.Usbcheck();
    }

    private void Synclock()
    {
      this._job = 2;
      this._success = false;
      byte[] Buffer = new byte[100];
      this.ResetUSB();
      DateTime dateTime1;
      do
      {
        Application.DoEvents();
        dateTime1 = DateTime.UtcNow.Add(this._pc_utc_timediff);
        dateTime1 = dateTime1.AddHours(this._citytimediff + (double) this._DSTEN + this._citytimeoffset);
        dateTime1 = dateTime1.AddSeconds(2.0);
      }
      while ((dateTime1.Millisecond > 750 || dateTime1.Millisecond < 650) && !this.exitfg);
      uint num1 = 0U;
      byte[] numArray1 = Buffer;
      int num2 = (int) num1;
      int num3 = 1;
      uint num4 = (uint) (num2 + num3);
      IntPtr index1 = (IntPtr) (uint) num2;
      int num5 = 20;
      numArray1[index1] = (byte) num5;
      byte[] numArray2 = Buffer;
      int num6 = (int) num4;
      int num7 = 1;
      uint num8 = (uint) (num6 + num7);
      IntPtr index2 = (IntPtr) (uint) num6;
      int num9 = (int) (byte) dateTime1.Second;
      numArray2[index2] = (byte) num9;
      byte[] numArray3 = Buffer;
      int num10 = (int) num8;
      int num11 = 1;
      uint num12 = (uint) (num10 + num11);
      IntPtr index3 = (IntPtr) (uint) num10;
      int num13 = (int) (byte) dateTime1.Minute;
      numArray3[index3] = (byte) num13;
      byte[] numArray4 = Buffer;
      int num14 = (int) num12;
      int num15 = 1;
      uint num16 = (uint) (num14 + num15);
      IntPtr index4 = (IntPtr) (uint) num14;
      int num17 = (int) (byte) dateTime1.Hour;
      numArray4[index4] = (byte) num17;
      byte[] numArray5 = Buffer;
      int num18 = (int) num16;
      int num19 = 1;
      uint num20 = (uint) (num18 + num19);
      IntPtr index5 = (IntPtr) (uint) num18;
      int num21 = (int) (byte) dateTime1.Day;
      numArray5[index5] = (byte) num21;
      byte[] numArray6 = Buffer;
      int num22 = (int) num20;
      int num23 = 1;
      uint num24 = (uint) (num22 + num23);
      IntPtr index6 = (IntPtr) (uint) num22;
      int num25 = (int) (byte) dateTime1.Month;
      numArray6[index6] = (byte) num25;
      byte[] numArray7 = Buffer;
      int num26 = (int) num24;
      int num27 = 1;
      uint num28 = (uint) (num26 + num27);
      IntPtr index7 = (IntPtr) (uint) num26;
      int num29 = (int) (byte) (dateTime1.Year % 100);
      numArray7[index7] = (byte) num29;
      Buffer[(IntPtr) num28] = (byte) 0;
      for (uint index8 = 0U; index8 < num28; ++index8)
        Buffer[(IntPtr) num28] ^= Buffer[(IntPtr) index8];
      int num30 = (int) this.UsbObj.Send(Buffer, num28 + 1U);
      if (!this.deadwait(1500) || !this.getack((byte) 20))
        return;
      int second1 = DateTime.UtcNow.Add(this._pc_utc_timediff).Second;
      DateTime dateTime2 = dateTime1.AddSeconds(-1.0);
      int second2 = dateTime2.Second;
      if (second1 <= second2)
      {
        Buffer[0] = (byte) 31;
        while (true)
        {
          dateTime2 = DateTime.UtcNow;
          dateTime2 = dateTime2.Add(this._pc_utc_timediff);
          if (dateTime2.Second != dateTime1.Second)
            Application.DoEvents();
          else
            break;
        }
        int num31 = (int) this.UsbObj.Send(Buffer, 1U);
        this._success = true;
      }
      else
      {
        Buffer[0] = (byte) 32;
        int num31 = (int) this.UsbObj.Send(Buffer, 1U);
      }
    }

    private bool wait(int msec)
    {
      long num = (long) Environment.TickCount;
      while ((int) this.UsbObj.ReadBufferSize == 0 && this.UsbObj.IsConnected && !this.exitfg)
      {
        if ((long) Environment.TickCount - num > (long) msec || !this.UsbObj.IsConnected)
          return false;
        Application.DoEvents();
        Thread.Sleep(10);
      }
      return true;
    }

    private bool wait(int msec, int count)
    {
      long num = (long) Environment.TickCount;
      while ((long) this.UsbObj.ReadBufferSize < (long) count && this.UsbObj.IsConnected && !this.exitfg)
      {
        if ((long) Environment.TickCount - num > (long) msec || !this.UsbObj.IsConnected)
          return false;
        Application.DoEvents();
        Thread.Sleep(10);
      }
      return true;
    }

    private bool deadwait(int msec)
    {
      long num = (long) Environment.TickCount;
      while ((int) this.UsbObj.ReadBufferSize == 0 && this.UsbObj.IsConnected && !this.exitfg)
      {
        if ((long) Environment.TickCount - num > (long) msec || !this.UsbObj.IsConnected)
          return false;
        Application.DoEvents();
      }
      return true;
    }

    private bool getack(byte ackno)
    {
      byte[] Buffer = new byte[1];
      while (this.UsbObj.ReadBufferSize > 0U)
      {
        int num1 = (int) this.UsbObj.Read(Buffer, 1U);
        if ((int) Buffer[0] == 79)
        {
          Buffer[0] = (byte) 63;
          int num2 = (int) this.UsbObj.Send(Buffer, 1U);
          return false;
        }
        if ((int) Buffer[0] == 81)
        {
          this._gotinterrupt = true;
          this._interruptcommand = (int) Buffer[0];
          Buffer[0] = (byte) 65;
          int num2 = (int) this.UsbObj.Send(Buffer, 1U);
          return false;
        }
        if ((int) Buffer[0] == (int) ackno || (int) ackno == (int) byte.MaxValue)
          return true;
      }
      return false;
    }

    private void UpdateWeatherRequest()
    {
      this._job = 3;
    }

    private void UpdateRecRequest()
    {
      this._job = 4;
    }

    private void SendData()
    {
      this._job = 5;
      this._success = false;
      byte[] Buffer1 = new byte[32];
      byte[] buffer = new byte[16384];
      FileStream fileStream = (FileStream) null;
      try
      {
        this.ResetUSB();
        Application.DoEvents();
        fileStream = new FileStream(this._DataPath + "mcudata.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
        int num1 = fileStream.Read(buffer, 0, 16384);
        int num2 = (int) buffer[18] * 256 + (int) buffer[19];
        if (num2 % 16 != 0)
          num2 = (num2 / 16 + 1) * 16;
        uint num3 = 0U;
        byte[] numArray1 = Buffer1;
        int num4 = (int) num3;
        int num5 = 1;
        uint num6 = (uint) (num4 + num5);
        IntPtr index1 = (IntPtr) (uint) num4;
        int num7 = 48;
        numArray1[index1] = (byte) num7;
        byte[] numArray2 = Buffer1;
        int num8 = (int) num6;
        int num9 = 1;
        uint num10 = (uint) (num8 + num9);
        IntPtr index2 = (IntPtr) (uint) num8;
        int num11 = 17;
        numArray2[index2] = (byte) num11;
        byte[] numArray3 = Buffer1;
        int num12 = (int) num10;
        int num13 = 1;
        uint num14 = (uint) (num12 + num13);
        IntPtr index3 = (IntPtr) (uint) num12;
        int num15 = 0;
        numArray3[index3] = (byte) num15;
        Buffer1[(IntPtr) num14] = (byte) ((uint) Buffer1[0] ^ (uint) Buffer1[1] ^ (uint) Buffer1[2]);
        int num16 = (int) this.UsbObj.Send(Buffer1, num14 + 1U);
        if (!this.wait(10000) || !this.getack((byte) 48))
          return;
        byte[] Buffer2 = new byte[4];
        if (!this.wait(5000, 4))
          return;
        int num17 = (int) this.UsbObj.Read(Buffer2, 4U);
        int num18;
        if ((int) Buffer2[2] == 1)
        {
          num18 = 32;
          buffer[16] = (byte) 1;
        }
        else
        {
          num18 = 0;
          buffer[16] = (byte) 0;
        }
        uint num19 = 0U;
        byte[] numArray4 = Buffer1;
        int num20 = (int) num19;
        int num21 = 1;
        uint num22 = (uint) (num20 + num21);
        IntPtr index4 = (IntPtr) (uint) num20;
        int num23 = 45;
        numArray4[index4] = (byte) num23;
        byte[] numArray5 = Buffer1;
        int num24 = (int) num22;
        int num25 = 1;
        uint num26 = (uint) (num24 + num25);
        IntPtr index5 = (IntPtr) (uint) num24;
        int num27 = 34;
        numArray5[index5] = (byte) num27;
        byte[] numArray6 = Buffer1;
        int num28 = (int) num26;
        int num29 = 1;
        uint num30 = (uint) (num28 + num29);
        IntPtr index6 = (IntPtr) (uint) num28;
        int num31 = (int) (byte) num18;
        numArray6[index6] = (byte) num31;
        for (int index7 = 0; index7 < 14; ++index7)
          Buffer1[(IntPtr) num30++] = buffer[34 + index7];
        Buffer1[(IntPtr) num30] = (byte) 0;
        for (int index7 = 0; (long) index7 < (long) num30; ++index7)
          Buffer1[(IntPtr) num30] ^= Buffer1[index7];
        int num32 = (int) this.UsbObj.Send(Buffer1, num30 + 1U);
        if (!this.wait(10000) || !this.getack((byte) 47))
          return;
        uint num33 = 48U;
        while ((long) num33 < (long) num2)
        {
          if ((int) num33 == 1280)
          {
            uint num34 = 0U;
            byte[] numArray7 = Buffer1;
            int num35 = (int) num34;
            int num36 = 1;
            uint num37 = (uint) (num35 + num36);
            IntPtr index7 = (IntPtr) (uint) num35;
            int num38 = 47;
            numArray7[index7] = (byte) num38;
            byte[] numArray8 = Buffer1;
            int num39 = (int) num37;
            int num40 = 1;
            uint num41 = (uint) (num39 + num40);
            IntPtr index8 = (IntPtr) (uint) num39;
            int num42 = (int) (byte) (num33 % 256U);
            numArray8[index8] = (byte) num42;
            byte[] numArray9 = Buffer1;
            int num43 = (int) num41;
            int num44 = 1;
            uint num45 = (uint) (num43 + num44);
            IntPtr index9 = (IntPtr) (uint) num43;
            int num46 = (int) (byte) ((ulong) (num33 / 256U) + (ulong) num18);
            numArray9[index9] = (byte) num46;
            for (int index10 = 0; index10 < 4; ++index10)
              Buffer1[(IntPtr) num45++] = (long) num33 < (long) num1 ? buffer[(long) num33 + (long) index10] : (byte) 0;
            Buffer1[(IntPtr) num45] = (byte) 0;
            for (int index10 = 0; (long) index10 < (long) num45; ++index10)
              Buffer1[(IntPtr) num45] ^= Buffer1[index10];
            int num47 = (int) this.UsbObj.Send(Buffer1, num45 + 1U);
            if (!this.wait(10000) || !this.getack((byte) 47))
              return;
            uint num48 = 0U;
            byte[] numArray10 = Buffer1;
            int num49 = (int) num48;
            int num50 = 1;
            uint num51 = (uint) (num49 + num50);
            IntPtr index11 = (IntPtr) (uint) num49;
            int num52 = 47;
            numArray10[index11] = (byte) num52;
            byte[] numArray11 = Buffer1;
            int num53 = (int) num51;
            int num54 = 1;
            uint num55 = (uint) (num53 + num54);
            IntPtr index12 = (IntPtr) (uint) num53;
            int num56 = (int) (byte) (num33 % 256U);
            numArray11[index12] = (byte) num56;
            byte[] numArray12 = Buffer1;
            int num57 = (int) num55;
            int num58 = 1;
            uint num59 = (uint) (num57 + num58);
            IntPtr index13 = (IntPtr) (uint) num57;
            int num60 = (int) (byte) ((ulong) (num33 / 256U) + (ulong) num18);
            numArray12[index13] = (byte) num60;
            for (int index10 = 0; index10 < 12; ++index10)
              Buffer1[(IntPtr) num59++] = (long) num33 < (long) num1 ? buffer[(long) num33 + (long) index10] : (byte) 0;
            Buffer1[(IntPtr) num59] = (byte) 0;
            for (int index10 = 0; (long) index10 < (long) num59; ++index10)
              Buffer1[(IntPtr) num59] ^= Buffer1[index10];
            int num61 = (int) this.UsbObj.Send(Buffer1, num59 + 1U);
            if (!this.wait(10000) || !this.getack((byte) 47))
              return;
          }
          else
          {
            uint num34 = 0U;
            byte[] numArray7 = Buffer1;
            int num35 = (int) num34;
            int num36 = 1;
            uint num37 = (uint) (num35 + num36);
            IntPtr index7 = (IntPtr) (uint) num35;
            int num38 = 47;
            numArray7[index7] = (byte) num38;
            byte[] numArray8 = Buffer1;
            int num39 = (int) num37;
            int num40 = 1;
            uint num41 = (uint) (num39 + num40);
            IntPtr index8 = (IntPtr) (uint) num39;
            int num42 = (int) (byte) (num33 % 256U);
            numArray8[index8] = (byte) num42;
            byte[] numArray9 = Buffer1;
            int num43 = (int) num41;
            int num44 = 1;
            uint num45 = (uint) (num43 + num44);
            IntPtr index9 = (IntPtr) (uint) num43;
            int num46 = (int) (byte) ((ulong) (num33 / 256U) + (ulong) num18);
            numArray9[index9] = (byte) num46;
            for (int index10 = 0; index10 < 16; ++index10)
              Buffer1[(IntPtr) num45++] = (long) num33 < (long) num1 ? buffer[(long) num33 + (long) index10] : (byte) 0;
            Buffer1[(IntPtr) num45] = (byte) 0;
            for (int index10 = 0; (long) index10 < (long) num45; ++index10)
              Buffer1[(IntPtr) num45] ^= Buffer1[index10];
            int num47 = (int) this.UsbObj.Send(Buffer1, num45 + 1U);
            if (!this.wait(10000) || !this.getack((byte) 47))
              return;
          }
          num33 += 16U;
        }
        uint num62 = 0U;
        byte[] numArray13 = Buffer1;
        int num63 = (int) num62;
        int num64 = 1;
        uint num65 = (uint) (num63 + num64);
        IntPtr index14 = (IntPtr) (uint) num63;
        int num66 = 46;
        numArray13[index14] = (byte) num66;
        byte[] numArray14 = Buffer1;
        int num67 = (int) num65;
        int num68 = 1;
        uint num69 = (uint) (num67 + num68);
        IntPtr index15 = (IntPtr) (uint) num67;
        int num70 = 16;
        numArray14[index15] = (byte) num70;
        byte[] numArray15 = Buffer1;
        int num71 = (int) num69;
        int num72 = 1;
        uint num73 = (uint) (num71 + num72);
        IntPtr index16 = (IntPtr) (uint) num71;
        int num74 = (int) (byte) num18;
        numArray15[index16] = (byte) num74;
        for (int index7 = 0; index7 < 15; ++index7)
          Buffer1[(IntPtr) num73++] = buffer[16 + index7];
        Buffer1[(IntPtr) num73] = (byte) 0;
        for (int index7 = 0; (long) index7 < (long) num73; ++index7)
          Buffer1[(IntPtr) num73] ^= Buffer1[index7];
        int num75 = (int) this.UsbObj.Send(Buffer1, num73 + 1U);
        if (!this.wait(10000) || !this.getack((byte) 47))
          return;
        int num76 = num18 != 32 ? 32 : 0;
        uint num77 = 0U;
        byte[] numArray16 = Buffer1;
        int num78 = (int) num77;
        int num79 = 1;
        uint num80 = (uint) (num78 + num79);
        IntPtr index17 = (IntPtr) (uint) num78;
        int num81 = 32;
        numArray16[index17] = (byte) num81;
        byte[] numArray17 = Buffer1;
        int num82 = (int) num80;
        int num83 = 1;
        uint num84 = (uint) (num82 + num83);
        IntPtr index18 = (IntPtr) (uint) num82;
        int num85 = 17;
        numArray17[index18] = (byte) num85;
        byte[] numArray18 = Buffer1;
        int num86 = (int) num84;
        int num87 = 1;
        uint num88 = (uint) (num86 + num87);
        IntPtr index19 = (IntPtr) (uint) num86;
        int num89 = (int) (byte) num76;
        numArray18[index19] = (byte) num89;
        byte[] numArray19 = Buffer1;
        int num90 = (int) num88;
        int num91 = 1;
        uint num92 = (uint) (num90 + num91);
        IntPtr index20 = (IntPtr) (uint) num90;
        int num93 = 0;
        numArray19[index20] = (byte) num93;
        Buffer1[(IntPtr) num92] = (byte) 0;
        for (int index7 = 0; (long) index7 < (long) num92; ++index7)
          Buffer1[(IntPtr) num92] ^= Buffer1[index7];
        int num94 = (int) this.UsbObj.Send(Buffer1, num92 + 1U);
        if (!this.wait(10000) || !this.getack((byte) 47))
          return;
        this._success = true;
      }
      finally
      {
        if (fileStream != null)
          fileStream.Close();
      }
    }

    private void Usbcheck()
    {
      this._job = 6;
      this._success = false;
      byte[] Buffer = new byte[1];
      while (this.UsbObj.ReadBufferSize > 0U)
      {
        int num1 = (int) this.UsbObj.Read(Buffer, 1U);
        if ((int) Buffer[0] == 79)
        {
          this._gotinterrupt = true;
          this._interruptcommand = (int) Buffer[0];
          Buffer[0] = (byte) 63;
          int num2 = (int) this.UsbObj.Send(Buffer, 1U);
          this._success = true;
          break;
        }
        if ((int) Buffer[0] == 81)
        {
          this._gotinterrupt = true;
          this._interruptcommand = (int) Buffer[0];
          Buffer[0] = (byte) 65;
          int num2 = (int) this.UsbObj.Send(Buffer, 1U);
          this._success = true;
          break;
        }
      }
    }

    private void ResetUSB()
    {
      byte[] Buffer1 = new byte[19];
      for (int index = 0; index < Buffer1.Length; ++index)
        Buffer1[index] = (byte) 0;
      int num1 = (int) this.UsbObj.Send(Buffer1, (uint) Buffer1.Length);
      if ((int) this.UsbObj.ReadBufferSize == 0)
        return;
      byte[] Buffer2 = new byte[(IntPtr) this.UsbObj.ReadBufferSize];
      int num2 = (int) this.UsbObj.Read(Buffer2, (uint) Buffer2.Length);
    }
  }
}
