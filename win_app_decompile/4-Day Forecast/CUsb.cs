// Decompiled with JetBrains decompiler
// Type: Forecast.CUsb
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Forecast
{
  public class CUsb
  {
    private const uint USB_RXCHAR = 1U;
    private const uint USB_TXREADY = 2U;
    private const uint USB_DEVOPEN = 4U;
    private const uint USB_DEVCLOSE = 8U;
    private const uint USB_CLOSING = 16U;
    private const uint USB_CLOSED = 32U;
    private const uint USB_TXTIMEOUT = 64U;
    private const uint USB_RXBUFOVRF = 128U;
    private const uint USB_RXOVRF = 256U;
    private const uint USB_RXOVRF1 = 512U;
    private const uint USB_RXPARITYERR = 1024U;
    private const uint USB_TXERROR = 2048U;
    private const uint USB_RXCUSTOM = 4096U;
    private const uint POLLING_INTERVAL = 50U;
    private uint hUsb;
    private Timer eventTimer;

    public uint ReadBufferSize
    {
      get
      {
        return CUsb.USBGetReadBufferSize(this.hUsb);
      }
    }

    public uint WriteBufferSize
    {
      get
      {
        return CUsb.USBGetWriteBufferSize(this.hUsb);
      }
    }

    public uint Status
    {
      get
      {
        return CUsb.USBGetStatus(this.hUsb);
      }
    }

    public bool IsConnected
    {
      get
      {
        return CUsb.USBCheck(this.hUsb) != 0;
      }
    }

    public event UsbOnConnectedEventHandler OnConnected;

    public event UsbOnDisconnectedEventHandler OnDisconnected;

    public event UsbOnReceiveEventHandler OnReceive;

    public event UsbOnCustomReceiveEventHandler OnCustomReceive;

    public CUsb(uint nVendor, uint nProductID, uint nReadBuffer, uint nWriteBuffer)
    {
      this.hUsb = 0U;
      this.hUsb = CUsb.USBOpen(this.hUsb, nVendor, nProductID, nReadBuffer, nWriteBuffer, (CUsb.UsbEventHandler) null);
      this.eventTimer = new Timer();
      this.eventTimer.Interval = 50;
      this.eventTimer.Enabled = true;
      this.eventTimer.Tick += new EventHandler(this.eventTimer_Tick);
    }

    public CUsb(uint nVendor, uint nProductID, uint nReadBuffer, uint nWriteBuffer, int option)
    {
      this.hUsb = 0U;
      if (option == 0)
      {
        this.hUsb = CUsb.USBOpen(this.hUsb, nVendor, nProductID, nReadBuffer, nWriteBuffer, (CUsb.UsbEventHandler) null);
        this.eventTimer = new Timer();
        this.eventTimer.Interval = 50;
        this.eventTimer.Enabled = true;
        this.eventTimer.Tick += new EventHandler(this.eventTimer_Tick);
      }
      else
      {
        CUsb.UsbEventHandler USBProc = new CUsb.UsbEventHandler(this._UsbEvent);
        this.hUsb = CUsb.USBOpen(this.hUsb, nVendor, nProductID, nReadBuffer, nWriteBuffer, USBProc);
      }
    }

    ~CUsb()
    {
      int num = (int) CUsb.USBClose(this.hUsb, 0U);
    }

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBOpen(uint hUsb, uint nVendor, uint nProductID, uint nReadBuffer, uint nWriteBuffer, CUsb.UsbEventHandler USBProc);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern int USBCheck(uint hUsb);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBRead(uint hUSB, byte[] Buffer, uint nNumByte);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBWrite(uint hUSB, byte[] Buffer, uint nNumByte, uint WaitSec);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBClose(uint hUSB, uint Op);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBGetError(uint hUSB);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBGetStatus(uint hUSB);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBGetReadBufferSize(uint hUSB);

    [DllImport("usb.dll", SetLastError = true)]
    private static extern uint USBGetWriteBufferSize(uint hUSB);

    public uint Send(byte[] Buffer, uint nNumByte, uint WaitSec)
    {
      return CUsb.USBWrite(this.hUsb, Buffer, nNumByte, WaitSec);
    }

    public uint Send(byte[] Buffer, uint nNumByte)
    {
      return CUsb.USBWrite(this.hUsb, Buffer, nNumByte, 0U);
    }

    public uint Read(byte[] Buffer, uint nNumByte)
    {
      return CUsb.USBRead(this.hUsb, Buffer, nNumByte);
    }

    public string GetFileVersion(ref int versionMajor, ref int versionMinor)
    {
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("USB.DLL");
      versionMajor = versionInfo.FileMajorPart;
      versionMinor = versionInfo.FileMinorPart;
      return versionInfo.FileVersion;
    }

    public string GetProductVersion(ref int versionMajor, ref int versionMinor)
    {
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("USB.DLL");
      versionMajor = versionInfo.ProductMajorPart;
      versionMinor = versionInfo.ProductMinorPart;
      return versionInfo.ProductVersion;
    }

    private void _UsbEvent(uint msg)
    {
      if (((int) msg & 4) != 0 && this.OnConnected != null)
        this.OnConnected((object) this, new UsbOnConnectedEventArgs());
      if (((int) msg & 8) != 0 && this.OnDisconnected != null)
        this.OnDisconnected((object) this, new UsbOnDisconnectedEventArgs());
      if (((int) msg & 1) != 0 && this.OnReceive != null)
        this.OnReceive((object) this, new UsbOnReceiveEventArgs()
        {
          ReadBufferSize = this.ReadBufferSize
        });
      if (((int) msg & 4096) == 0)
        ;
    }

    private void eventTimer_Tick(object sender, EventArgs e)
    {
      this._UsbEvent(this.Status);
    }

    private delegate void UsbEventHandler(uint msg);
  }
}
