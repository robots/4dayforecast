// Decompiled with JetBrains decompiler
// Type: Forecast.Main
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using Forecast.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Forecast
{
  public class Main : Form
  {
    private IContainer components = (IContainer) null;
    private string[] ip = new string[3];
    private Thread usbfunc = (Thread) null;
    private string[] apver = new string[3];
    private citydata[] cdata = new citydata[5];
    private DateTimeFormatInfo timeformat = new DateTimeFormatInfo();
    private Label lb_HOMEWEATHER;
    private Label lb_CITY1;
    private Label lb_CITY2;
    private Label lb_CITY3;
    private Label lb_CITY4;
    private Label lb_Time;
    private Label lb_Date;
    private Label tb_Title;
    private Label lb_CityName;
    private Label lb_Lastupdate;
    private Label poweredby;
    private Label lb_Sunrise;
    private Label lb_Sunset;
    private Label sunrise;
    private Label sunset;
    private Label D0DAY;
    private Label D1DAY;
    private Label D2DAY;
    private Label D3DAY;
    private Label D0TEXT;
    private Label D1TEXT;
    private Label D2TEXT;
    private Label D3TEXT;
    private Label lb_HIGH;
    private Label lb_LOW;
    private Label lb_HUMIDIY;
    private Label lb_uvindex;
    private Label lb_precipitation;
    private Label lb_wind;
    private Label lb_direction;
    private Label D0HT;
    private Label D0LT;
    private Label D0HUM;
    private Label D0PRE;
    private Label D0UV;
    private Label D0WS;
    private Label D0WDN;
    private PictureBox D0PIC;
    private PictureBox D0WD;
    private PictureBox D1PIC;
    private PictureBox D2PIC;
    private PictureBox D3PIC;
    private PictureBox D1WD;
    private Label D1WDN;
    private Label D1WS;
    private Label D1UV;
    private Label D1PRE;
    private Label D1HUM;
    private Label D1LT;
    private Label D1HT;
    private PictureBox D2WD;
    private Label D2WDN;
    private Label D2WS;
    private Label D2UV;
    private Label D2PRE;
    private Label D2HUM;
    private Label D2LT;
    private Label D2HT;
    private PictureBox D3WD;
    private Label D3WDN;
    private Label D3WS;
    private Label D3UV;
    private Label D3PRE;
    private Label D3HUM;
    private Label D3LT;
    private Label D3HT;
    private Label settingbutton;
    private Label updatebutton;
    private Label updaterecbutton;
    private Label closebutton;
    private System.Windows.Forms.Timer timer1;
    private NotifyIcon notifyIcon1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem oPENToolStripMenuItem;
    private ToolStripMenuItem eXITToolStripMenuItem;
    private System.Windows.Forms.Timer timer2;
    private Bitmap renderBmp;
    private Point mouse_offset;
    private bool networkok;
    private string DataPath;
    private int curcity;
    private bool exitfg;
    private bool time12hr;
    private bool daymonth;
    private bool CEL;
    private int windspeedfor;
    private usb_access usbobj;
    private TimeSpan pc_utc_timediff;
    private DateTime LastUpdateTime;
    private DateTime NextUpdateTime;
    private DateTime last_displaytime;
    private DateTime last_displaytime0;
    private string citylistver;
    private bool newap;
    private string newapver;
    private int updatedevice;
    private Hashtable controlHashtable;
    private CultureInfo us_culture;
    private RNGCryptoServiceProvider Gen;

    public override Image BackgroundImage
    {
      get
      {
        return (Image) this.renderBmp;
      }
      set
      {
        if (value == null)
          return;
        Image image = value;
        this.renderBmp = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppPArgb);
        Graphics graphics = Graphics.FromImage((Image) this.renderBmp);
        graphics.DrawImageUnscaled(image, 0, 0, this.Width, this.Height);
        graphics.Dispose();
      }
    }

    public Main()
    {
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      byte[] numArray = new byte[16];
      this.Gen = new RNGCryptoServiceProvider();
      this.InitializeComponent();
      this.newap = false;
      this.apver[0] = "NIS01_INSTALL_1940.EXE;NIS01_INSTALL_1940.ZIP";
      this.apver[1] = "NIS01_INSTALL_1940.EXE;NIS01_INSTALL_1940.ZIP";
      this.apver[2] = "NIS01_INSTALL_1940.EXE;NIS01_INSTALL_1940.ZIP";
      this.networkok = false;
      this.exitfg = false;
      this.updatedevice = 0;
      this.pc_utc_timediff = new TimeSpan(0L);
      this.curcity = 0;
      for (int index = 0; index < this.cdata.Length; ++index)
        this.cdata[index] = new citydata();
      this.timeformat.AMDesignator = "am";
      this.timeformat.PMDesignator = "pm";
      this.time12hr = false;
      this.daymonth = true;
      this.timeformat.FullDateTimePattern = "M/d/yyyy h:mm tt";
      this.changeformat();
      this.CEL = true;
      this.windspeedfor = 0;
      this.us_culture = new CultureInfo("en-US");
      this.LastUpdateTime = DateTime.UtcNow;
      this.DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\4-Day Forecast\\";
      if (!Directory.Exists(this.DataPath + "db\\"))
      {
        string str = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\4-Day Forecast\\";
        if (Directory.Exists(str + "db\\"))
        {
          Directory.CreateDirectory(this.DataPath + "\\db\\");
          System.IO.File.Copy(str + "db\\CityListAsiaAustralia.txt", this.DataPath + "\\db\\CityListAsiaAustralia.txt", true);
          System.IO.File.Copy(str + "db\\CityListEurope.txt", this.DataPath + "\\db\\CityListEurope.txt", true);
          System.IO.File.Copy(str + "db\\CityListNorthAmerica.txt", this.DataPath + "\\db\\CityListNorthAmerica.txt", true);
          System.IO.File.Copy(str + "db\\DSTAU1.txt", this.DataPath + "\\db\\DSTAU1.txt", true);
          System.IO.File.Copy(str + "db\\DSTAU2.txt", this.DataPath + "\\db\\DSTAU2.txt", true);
          System.IO.File.Copy(str + "db\\DSTAU3.txt", this.DataPath + "\\db\\DSTAU3.txt", true);
          System.IO.File.Copy(str + "db\\DSTEU0.txt", this.DataPath + "\\db\\DSTEU0.txt", true);
          System.IO.File.Copy(str + "db\\DSTEU1.txt", this.DataPath + "\\db\\DSTEU1.txt", true);
          System.IO.File.Copy(str + "db\\DSTEU2.txt", this.DataPath + "\\db\\DSTEU2.txt", true);
          System.IO.File.Copy(str + "db\\DSTRU.txt", this.DataPath + "\\db\\DSTRU.txt", true);
          System.IO.File.Copy(str + "db\\DSTUS.txt", this.DataPath + "\\db\\DSTUS.txt", true);
        }
        else
          this.exitfg = true;
      }
      this.usbobj = new usb_access(this.DataPath);
      this.ip[0] = "http://server1.netinfostation.com/";
      this.ip[1] = "http://server2.netinfostation.com/";
      this.ip[2] = "http://server3.netinfostation.com/";
      this.BackgroundImage = (Image) Resources.Background;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Main));
      this.lb_HOMEWEATHER = new Label();
      this.lb_CITY1 = new Label();
      this.lb_CITY2 = new Label();
      this.lb_CITY3 = new Label();
      this.lb_CITY4 = new Label();
      this.lb_Time = new Label();
      this.lb_Date = new Label();
      this.tb_Title = new Label();
      this.lb_CityName = new Label();
      this.lb_Lastupdate = new Label();
      this.poweredby = new Label();
      this.lb_Sunrise = new Label();
      this.lb_Sunset = new Label();
      this.sunrise = new Label();
      this.sunset = new Label();
      this.D0DAY = new Label();
      this.D1DAY = new Label();
      this.D2DAY = new Label();
      this.D3DAY = new Label();
      this.D0TEXT = new Label();
      this.D1TEXT = new Label();
      this.D2TEXT = new Label();
      this.D3TEXT = new Label();
      this.lb_HIGH = new Label();
      this.lb_LOW = new Label();
      this.lb_HUMIDIY = new Label();
      this.lb_uvindex = new Label();
      this.lb_precipitation = new Label();
      this.lb_wind = new Label();
      this.lb_direction = new Label();
      this.D0HT = new Label();
      this.D0LT = new Label();
      this.D0HUM = new Label();
      this.D0PRE = new Label();
      this.D0UV = new Label();
      this.D0WS = new Label();
      this.D0WDN = new Label();
      this.D0PIC = new PictureBox();
      this.D0WD = new PictureBox();
      this.D1PIC = new PictureBox();
      this.D2PIC = new PictureBox();
      this.D3PIC = new PictureBox();
      this.D1WD = new PictureBox();
      this.D1WDN = new Label();
      this.D1WS = new Label();
      this.D1UV = new Label();
      this.D1PRE = new Label();
      this.D1HUM = new Label();
      this.D1LT = new Label();
      this.D1HT = new Label();
      this.D2WD = new PictureBox();
      this.D2WDN = new Label();
      this.D2WS = new Label();
      this.D2UV = new Label();
      this.D2PRE = new Label();
      this.D2HUM = new Label();
      this.D2LT = new Label();
      this.D2HT = new Label();
      this.D3WD = new PictureBox();
      this.D3WDN = new Label();
      this.D3WS = new Label();
      this.D3UV = new Label();
      this.D3PRE = new Label();
      this.D3HUM = new Label();
      this.D3LT = new Label();
      this.D3HT = new Label();
      this.settingbutton = new Label();
      this.updatebutton = new Label();
      this.updaterecbutton = new Label();
      this.closebutton = new Label();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.notifyIcon1 = new NotifyIcon(this.components);
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.oPENToolStripMenuItem = new ToolStripMenuItem();
      this.eXITToolStripMenuItem = new ToolStripMenuItem();
      this.timer2 = new System.Windows.Forms.Timer(this.components);
      ((ISupportInitialize) this.D0PIC).BeginInit();
      ((ISupportInitialize) this.D0WD).BeginInit();
      ((ISupportInitialize) this.D1PIC).BeginInit();
      ((ISupportInitialize) this.D2PIC).BeginInit();
      ((ISupportInitialize) this.D3PIC).BeginInit();
      ((ISupportInitialize) this.D1WD).BeginInit();
      ((ISupportInitialize) this.D2WD).BeginInit();
      ((ISupportInitialize) this.D3WD).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.lb_HOMEWEATHER.BackColor = Color.Black;
      this.lb_HOMEWEATHER.Cursor = Cursors.Hand;
      this.lb_HOMEWEATHER.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_HOMEWEATHER.ForeColor = Color.White;
      this.lb_HOMEWEATHER.Image = (Image) Resources.tab_aHome;
      this.lb_HOMEWEATHER.Location = new Point(0, 0);
      this.lb_HOMEWEATHER.Name = "lb_HOMEWEATHER";
      this.lb_HOMEWEATHER.Size = new Size(143, 32);
      this.lb_HOMEWEATHER.TabIndex = 1;
      this.lb_HOMEWEATHER.Text = "HOME WEATHER";
      this.lb_HOMEWEATHER.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_HOMEWEATHER.Click += new EventHandler(this.lb_HOMEWEATHER_Click);
      this.lb_CITY1.BackColor = Color.Black;
      this.lb_CITY1.Cursor = Cursors.Hand;
      this.lb_CITY1.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_CITY1.ForeColor = Color.DarkGray;
      this.lb_CITY1.Image = (Image) Resources.tab_dCITY1;
      this.lb_CITY1.Location = new Point(143, 0);
      this.lb_CITY1.Name = "lb_CITY1";
      this.lb_CITY1.Size = new Size(75, 32);
      this.lb_CITY1.TabIndex = 2;
      this.lb_CITY1.Text = "CITY1";
      this.lb_CITY1.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_CITY1.Click += new EventHandler(this.lb_CITY1_Click);
      this.lb_CITY2.BackColor = Color.Black;
      this.lb_CITY2.Cursor = Cursors.Hand;
      this.lb_CITY2.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_CITY2.ForeColor = Color.DarkGray;
      this.lb_CITY2.Image = (Image) Resources.tab_dCITY2;
      this.lb_CITY2.Location = new Point(218, 0);
      this.lb_CITY2.Name = "lb_CITY2";
      this.lb_CITY2.Size = new Size(75, 32);
      this.lb_CITY2.TabIndex = 3;
      this.lb_CITY2.Text = "CITY2";
      this.lb_CITY2.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_CITY2.Click += new EventHandler(this.lb_CITY2_Click);
      this.lb_CITY3.BackColor = Color.Black;
      this.lb_CITY3.Cursor = Cursors.Hand;
      this.lb_CITY3.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_CITY3.ForeColor = Color.DarkGray;
      this.lb_CITY3.Image = (Image) Resources.tab_dCITY3;
      this.lb_CITY3.Location = new Point(293, 0);
      this.lb_CITY3.Name = "lb_CITY3";
      this.lb_CITY3.Size = new Size(75, 32);
      this.lb_CITY3.TabIndex = 4;
      this.lb_CITY3.Text = "CITY3";
      this.lb_CITY3.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_CITY3.Click += new EventHandler(this.lb_CITY3_Click);
      this.lb_CITY4.BackColor = Color.Black;
      this.lb_CITY4.Cursor = Cursors.Hand;
      this.lb_CITY4.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_CITY4.ForeColor = Color.DarkGray;
      this.lb_CITY4.Image = (Image) Resources.tab_dCITY4;
      this.lb_CITY4.Location = new Point(368, 0);
      this.lb_CITY4.Name = "lb_CITY4";
      this.lb_CITY4.Size = new Size(75, 32);
      this.lb_CITY4.TabIndex = 5;
      this.lb_CITY4.Text = "CITY4";
      this.lb_CITY4.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_CITY4.Click += new EventHandler(this.lb_CITY4_Click);
      this.lb_Time.BackColor = Color.Transparent;
      this.lb_Time.CausesValidation = false;
      this.lb_Time.Font = new Font("Arial", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_Time.ForeColor = Color.White;
      this.lb_Time.Location = new Point(8, 44);
      this.lb_Time.Name = "lb_Time";
      this.lb_Time.Size = new Size(92, 20);
      this.lb_Time.TabIndex = 6;
      this.lb_Time.Text = "--:--";
      this.lb_Time.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_Time.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_Time.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_Date.BackColor = Color.Transparent;
      this.lb_Date.CausesValidation = false;
      this.lb_Date.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_Date.ForeColor = Color.White;
      this.lb_Date.Location = new Point(74, 44);
      this.lb_Date.Name = "lb_Date";
      this.lb_Date.Size = new Size(52, 20);
      this.lb_Date.TabIndex = 7;
      this.lb_Date.Text = "(--/--)";
      this.lb_Date.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_Date.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_Date.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.tb_Title.BackColor = Color.Transparent;
      this.tb_Title.CausesValidation = false;
      this.tb_Title.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tb_Title.ForeColor = Color.White;
      this.tb_Title.Location = new Point(140, 48);
      this.tb_Title.Name = "tb_Title";
      this.tb_Title.Size = new Size(160, 20);
      this.tb_Title.TabIndex = 8;
      this.tb_Title.Text = "4 - DAY FORECAST";
      this.tb_Title.TextAlign = ContentAlignment.TopCenter;
      this.tb_Title.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.tb_Title.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_CityName.BackColor = Color.Transparent;
      this.lb_CityName.CausesValidation = false;
      this.lb_CityName.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_CityName.ForeColor = Color.White;
      this.lb_CityName.Location = new Point(8, 64);
      this.lb_CityName.Name = "lb_CityName";
      this.lb_CityName.Size = new Size(124, 48);
      this.lb_CityName.TabIndex = 9;
      this.lb_CityName.Text = "CityName";
      this.lb_CityName.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_CityName.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_Lastupdate.BackColor = Color.Transparent;
      this.lb_Lastupdate.CausesValidation = false;
      this.lb_Lastupdate.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_Lastupdate.ForeColor = Color.White;
      this.lb_Lastupdate.Location = new Point(70, 72);
      this.lb_Lastupdate.Name = "lb_Lastupdate";
      this.lb_Lastupdate.Size = new Size(300, 16);
      this.lb_Lastupdate.TabIndex = 10;
      this.lb_Lastupdate.Text = "Last Update -";
      this.lb_Lastupdate.TextAlign = ContentAlignment.TopCenter;
      this.lb_Lastupdate.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_Lastupdate.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.poweredby.BackColor = Color.Transparent;
      this.poweredby.CausesValidation = false;
      this.poweredby.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.poweredby.ForeColor = Color.White;
      this.poweredby.Image = (Image) Resources.poweredby;
      this.poweredby.Location = new Point(340, 32);
      this.poweredby.Name = "poweredby";
      this.poweredby.Size = new Size(100, 24);
      this.poweredby.TabIndex = 11;
      this.poweredby.TextAlign = ContentAlignment.TopCenter;
      this.poweredby.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.poweredby.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_Sunrise.BackColor = Color.Transparent;
      this.lb_Sunrise.CausesValidation = false;
      this.lb_Sunrise.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_Sunrise.ForeColor = Color.White;
      this.lb_Sunrise.Location = new Point(384, 60);
      this.lb_Sunrise.Name = "lb_Sunrise";
      this.lb_Sunrise.Size = new Size(52, 18);
      this.lb_Sunrise.TabIndex = 12;
      this.lb_Sunrise.Text = "Sunrise";
      this.lb_Sunrise.TextAlign = ContentAlignment.BottomRight;
      this.lb_Sunrise.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_Sunrise.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_Sunset.BackColor = Color.Transparent;
      this.lb_Sunset.CausesValidation = false;
      this.lb_Sunset.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_Sunset.ForeColor = Color.White;
      this.lb_Sunset.Location = new Point(384, 76);
      this.lb_Sunset.Name = "lb_Sunset";
      this.lb_Sunset.Size = new Size(52, 18);
      this.lb_Sunset.TabIndex = 13;
      this.lb_Sunset.Text = "Sunset";
      this.lb_Sunset.TextAlign = ContentAlignment.BottomRight;
      this.lb_Sunset.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_Sunset.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.sunrise.BackColor = Color.Transparent;
      this.sunrise.CausesValidation = false;
      this.sunrise.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.sunrise.ForeColor = Color.White;
      this.sunrise.Location = new Point(308, 60);
      this.sunrise.Name = "sunrise";
      this.sunrise.Size = new Size(80, 20);
      this.sunrise.TabIndex = 14;
      this.sunrise.Text = "--:--";
      this.sunrise.TextAlign = ContentAlignment.BottomRight;
      this.sunrise.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.sunrise.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.sunset.BackColor = Color.Transparent;
      this.sunset.CausesValidation = false;
      this.sunset.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.sunset.ForeColor = Color.White;
      this.sunset.Location = new Point(308, 76);
      this.sunset.Name = "sunset";
      this.sunset.Size = new Size(80, 20);
      this.sunset.TabIndex = 15;
      this.sunset.Text = "--:--";
      this.sunset.TextAlign = ContentAlignment.BottomRight;
      this.sunset.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.sunset.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0DAY.BackColor = Color.Transparent;
      this.D0DAY.CausesValidation = false;
      this.D0DAY.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0DAY.ForeColor = Color.White;
      this.D0DAY.Location = new Point(102, 112);
      this.D0DAY.Name = "D0DAY";
      this.D0DAY.Size = new Size(70, 20);
      this.D0DAY.TabIndex = 16;
      this.D0DAY.Text = "TODAY";
      this.D0DAY.TextAlign = ContentAlignment.TopCenter;
      this.D0DAY.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0DAY.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1DAY.BackColor = Color.Transparent;
      this.D1DAY.CausesValidation = false;
      this.D1DAY.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1DAY.ForeColor = Color.White;
      this.D1DAY.Location = new Point(187, 112);
      this.D1DAY.Name = "D1DAY";
      this.D1DAY.Size = new Size(70, 20);
      this.D1DAY.TabIndex = 17;
      this.D1DAY.Text = "2ND";
      this.D1DAY.TextAlign = ContentAlignment.TopCenter;
      this.D1DAY.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1DAY.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2DAY.BackColor = Color.Transparent;
      this.D2DAY.CausesValidation = false;
      this.D2DAY.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2DAY.ForeColor = Color.White;
      this.D2DAY.Location = new Point(272, 112);
      this.D2DAY.Name = "D2DAY";
      this.D2DAY.Size = new Size(70, 20);
      this.D2DAY.TabIndex = 18;
      this.D2DAY.Text = "3RD";
      this.D2DAY.TextAlign = ContentAlignment.TopCenter;
      this.D2DAY.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2DAY.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3DAY.BackColor = Color.Transparent;
      this.D3DAY.CausesValidation = false;
      this.D3DAY.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3DAY.ForeColor = Color.White;
      this.D3DAY.Location = new Point(357, 112);
      this.D3DAY.Name = "D3DAY";
      this.D3DAY.Size = new Size(70, 20);
      this.D3DAY.TabIndex = 19;
      this.D3DAY.Text = "4TH";
      this.D3DAY.TextAlign = ContentAlignment.TopCenter;
      this.D3DAY.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3DAY.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0TEXT.BackColor = Color.Transparent;
      this.D0TEXT.CausesValidation = false;
      this.D0TEXT.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0TEXT.ForeColor = Color.White;
      this.D0TEXT.Location = new Point(102, 200);
      this.D0TEXT.Name = "D0TEXT";
      this.D0TEXT.Size = new Size(70, 32);
      this.D0TEXT.TabIndex = 20;
      this.D0TEXT.Text = "1st Forecast";
      this.D0TEXT.TextAlign = ContentAlignment.TopCenter;
      this.D0TEXT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0TEXT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1TEXT.BackColor = Color.Transparent;
      this.D1TEXT.CausesValidation = false;
      this.D1TEXT.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1TEXT.ForeColor = Color.White;
      this.D1TEXT.Location = new Point(187, 200);
      this.D1TEXT.Name = "D1TEXT";
      this.D1TEXT.Size = new Size(70, 32);
      this.D1TEXT.TabIndex = 21;
      this.D1TEXT.Text = "2nd Forecast";
      this.D1TEXT.TextAlign = ContentAlignment.TopCenter;
      this.D1TEXT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1TEXT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2TEXT.BackColor = Color.Transparent;
      this.D2TEXT.CausesValidation = false;
      this.D2TEXT.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2TEXT.ForeColor = Color.White;
      this.D2TEXT.Location = new Point(272, 200);
      this.D2TEXT.Name = "D2TEXT";
      this.D2TEXT.Size = new Size(70, 32);
      this.D2TEXT.TabIndex = 22;
      this.D2TEXT.Text = "3rd Forecast";
      this.D2TEXT.TextAlign = ContentAlignment.TopCenter;
      this.D2TEXT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2TEXT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3TEXT.BackColor = Color.Transparent;
      this.D3TEXT.CausesValidation = false;
      this.D3TEXT.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3TEXT.ForeColor = Color.White;
      this.D3TEXT.Location = new Point(357, 200);
      this.D3TEXT.Name = "D3TEXT";
      this.D3TEXT.Size = new Size(70, 32);
      this.D3TEXT.TabIndex = 23;
      this.D3TEXT.Text = "4th Forecast";
      this.D3TEXT.TextAlign = ContentAlignment.TopCenter;
      this.D3TEXT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3TEXT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_HIGH.BackColor = Color.Transparent;
      this.lb_HIGH.CausesValidation = false;
      this.lb_HIGH.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_HIGH.ForeColor = Color.White;
      this.lb_HIGH.Location = new Point(8, 240);
      this.lb_HIGH.Name = "lb_HIGH";
      this.lb_HIGH.Size = new Size(88, 24);
      this.lb_HIGH.TabIndex = 24;
      this.lb_HIGH.Text = "HIGH";
      this.lb_HIGH.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_HIGH.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_HIGH.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_LOW.BackColor = Color.Transparent;
      this.lb_LOW.CausesValidation = false;
      this.lb_LOW.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_LOW.ForeColor = Color.White;
      this.lb_LOW.Location = new Point(8, 268);
      this.lb_LOW.Name = "lb_LOW";
      this.lb_LOW.Size = new Size(88, 24);
      this.lb_LOW.TabIndex = 25;
      this.lb_LOW.Text = "LOW";
      this.lb_LOW.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_LOW.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_LOW.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_HUMIDIY.BackColor = Color.Transparent;
      this.lb_HUMIDIY.CausesValidation = false;
      this.lb_HUMIDIY.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_HUMIDIY.ForeColor = Color.White;
      this.lb_HUMIDIY.Location = new Point(8, 296);
      this.lb_HUMIDIY.Name = "lb_HUMIDIY";
      this.lb_HUMIDIY.Size = new Size(88, 24);
      this.lb_HUMIDIY.TabIndex = 26;
      this.lb_HUMIDIY.Text = "HUMIDIY";
      this.lb_HUMIDIY.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_HUMIDIY.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_HUMIDIY.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_uvindex.BackColor = Color.Transparent;
      this.lb_uvindex.CausesValidation = false;
      this.lb_uvindex.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_uvindex.ForeColor = Color.White;
      this.lb_uvindex.Location = new Point(8, 324);
      this.lb_uvindex.Name = "lb_uvindex";
      this.lb_uvindex.Size = new Size(88, 24);
      this.lb_uvindex.TabIndex = 27;
      this.lb_uvindex.Text = "UV INDEX";
      this.lb_uvindex.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_uvindex.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_uvindex.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_precipitation.BackColor = Color.Transparent;
      this.lb_precipitation.CausesValidation = false;
      this.lb_precipitation.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_precipitation.ForeColor = Color.White;
      this.lb_precipitation.Location = new Point(8, 352);
      this.lb_precipitation.Name = "lb_precipitation";
      this.lb_precipitation.Size = new Size(88, 24);
      this.lb_precipitation.TabIndex = 28;
      this.lb_precipitation.Text = "PRECIPITATION";
      this.lb_precipitation.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_precipitation.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_precipitation.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_wind.BackColor = Color.Transparent;
      this.lb_wind.CausesValidation = false;
      this.lb_wind.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_wind.ForeColor = Color.White;
      this.lb_wind.Location = new Point(8, 380);
      this.lb_wind.Name = "lb_wind";
      this.lb_wind.Size = new Size(88, 24);
      this.lb_wind.TabIndex = 29;
      this.lb_wind.Text = "WIND";
      this.lb_wind.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_wind.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_wind.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_direction.BackColor = Color.Transparent;
      this.lb_direction.CausesValidation = false;
      this.lb_direction.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_direction.ForeColor = Color.White;
      this.lb_direction.Location = new Point(8, 432);
      this.lb_direction.Name = "lb_direction";
      this.lb_direction.Size = new Size(88, 24);
      this.lb_direction.TabIndex = 30;
      this.lb_direction.Text = "DIRECTION";
      this.lb_direction.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_direction.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_direction.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0HT.BackColor = Color.Transparent;
      this.D0HT.CausesValidation = false;
      this.D0HT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0HT.ForeColor = Color.White;
      this.D0HT.Location = new Point(102, 240);
      this.D0HT.Name = "D0HT";
      this.D0HT.Size = new Size(70, 24);
      this.D0HT.TabIndex = 31;
      this.D0HT.Text = "1st HT";
      this.D0HT.TextAlign = ContentAlignment.MiddleCenter;
      this.D0HT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0HT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0LT.BackColor = Color.Transparent;
      this.D0LT.CausesValidation = false;
      this.D0LT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0LT.ForeColor = Color.White;
      this.D0LT.Location = new Point(102, 268);
      this.D0LT.Name = "D0LT";
      this.D0LT.Size = new Size(70, 24);
      this.D0LT.TabIndex = 32;
      this.D0LT.Text = "1st LT";
      this.D0LT.TextAlign = ContentAlignment.MiddleCenter;
      this.D0LT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0LT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0HUM.BackColor = Color.Transparent;
      this.D0HUM.CausesValidation = false;
      this.D0HUM.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0HUM.ForeColor = Color.White;
      this.D0HUM.Location = new Point(102, 296);
      this.D0HUM.Name = "D0HUM";
      this.D0HUM.Size = new Size(70, 24);
      this.D0HUM.TabIndex = 33;
      this.D0HUM.Text = "1st H";
      this.D0HUM.TextAlign = ContentAlignment.MiddleCenter;
      this.D0HUM.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0HUM.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0PRE.BackColor = Color.Transparent;
      this.D0PRE.CausesValidation = false;
      this.D0PRE.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0PRE.ForeColor = Color.White;
      this.D0PRE.Location = new Point(102, 352);
      this.D0PRE.Name = "D0PRE";
      this.D0PRE.Size = new Size(70, 24);
      this.D0PRE.TabIndex = 34;
      this.D0PRE.Text = "1st PREC";
      this.D0PRE.TextAlign = ContentAlignment.MiddleCenter;
      this.D0PRE.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0PRE.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0UV.BackColor = Color.Transparent;
      this.D0UV.CausesValidation = false;
      this.D0UV.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0UV.ForeColor = Color.White;
      this.D0UV.Location = new Point(102, 324);
      this.D0UV.Name = "D0UV";
      this.D0UV.Size = new Size(70, 24);
      this.D0UV.TabIndex = 35;
      this.D0UV.Text = "1st UV";
      this.D0UV.TextAlign = ContentAlignment.MiddleCenter;
      this.D0UV.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0UV.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0WS.BackColor = Color.Transparent;
      this.D0WS.CausesValidation = false;
      this.D0WS.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0WS.ForeColor = Color.White;
      this.D0WS.Location = new Point(102, 380);
      this.D0WS.Name = "D0WS";
      this.D0WS.Size = new Size(70, 24);
      this.D0WS.TabIndex = 36;
      this.D0WS.Text = "1st WS";
      this.D0WS.TextAlign = ContentAlignment.MiddleCenter;
      this.D0WS.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0WS.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0WDN.BackColor = Color.Transparent;
      this.D0WDN.CausesValidation = false;
      this.D0WDN.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D0WDN.ForeColor = Color.White;
      this.D0WDN.Location = new Point(102, 404);
      this.D0WDN.Name = "D0WDN";
      this.D0WDN.Size = new Size(66, 10);
      this.D0WDN.TabIndex = 37;
      this.D0WDN.Text = "1st WDN";
      this.D0WDN.TextAlign = ContentAlignment.TopRight;
      this.D0WDN.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D0WDN.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D0PIC.BackColor = Color.Transparent;
      this.D0PIC.Location = new Point(107, 140);
      this.D0PIC.Name = "D0PIC";
      this.D0PIC.Size = new Size(60, 60);
      this.D0PIC.TabIndex = 38;
      this.D0PIC.TabStop = false;
      this.D0PIC.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D0PIC.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D0WD.BackColor = Color.Transparent;
      this.D0WD.Location = new Point(102, 416);
      this.D0WD.Name = "D0WD";
      this.D0WD.Size = new Size(70, 70);
      this.D0WD.TabIndex = 39;
      this.D0WD.TabStop = false;
      this.D0WD.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D0WD.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D1PIC.BackColor = Color.Transparent;
      this.D1PIC.Location = new Point(192, 140);
      this.D1PIC.Name = "D1PIC";
      this.D1PIC.Size = new Size(60, 60);
      this.D1PIC.TabIndex = 40;
      this.D1PIC.TabStop = false;
      this.D1PIC.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D1PIC.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D2PIC.BackColor = Color.Transparent;
      this.D2PIC.Location = new Point(277, 140);
      this.D2PIC.Name = "D2PIC";
      this.D2PIC.Size = new Size(60, 60);
      this.D2PIC.TabIndex = 41;
      this.D2PIC.TabStop = false;
      this.D2PIC.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D2PIC.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D3PIC.BackColor = Color.Transparent;
      this.D3PIC.Location = new Point(362, 140);
      this.D3PIC.Name = "D3PIC";
      this.D3PIC.Size = new Size(60, 60);
      this.D3PIC.TabIndex = 42;
      this.D3PIC.TabStop = false;
      this.D3PIC.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D3PIC.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D1WD.BackColor = Color.Transparent;
      this.D1WD.Location = new Point(187, 416);
      this.D1WD.Name = "D1WD";
      this.D1WD.Size = new Size(70, 70);
      this.D1WD.TabIndex = 50;
      this.D1WD.TabStop = false;
      this.D1WD.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D1WD.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D1WDN.BackColor = Color.Transparent;
      this.D1WDN.CausesValidation = false;
      this.D1WDN.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1WDN.ForeColor = Color.White;
      this.D1WDN.Location = new Point(187, 404);
      this.D1WDN.Name = "D1WDN";
      this.D1WDN.Size = new Size(66, 10);
      this.D1WDN.TabIndex = 49;
      this.D1WDN.Text = "2nd WDN";
      this.D1WDN.TextAlign = ContentAlignment.TopRight;
      this.D1WDN.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1WDN.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1WS.BackColor = Color.Transparent;
      this.D1WS.CausesValidation = false;
      this.D1WS.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1WS.ForeColor = Color.White;
      this.D1WS.Location = new Point(187, 380);
      this.D1WS.Name = "D1WS";
      this.D1WS.Size = new Size(70, 24);
      this.D1WS.TabIndex = 48;
      this.D1WS.Text = "2nd WS";
      this.D1WS.TextAlign = ContentAlignment.MiddleCenter;
      this.D1WS.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1WS.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1UV.BackColor = Color.Transparent;
      this.D1UV.CausesValidation = false;
      this.D1UV.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1UV.ForeColor = Color.White;
      this.D1UV.Location = new Point(187, 324);
      this.D1UV.Name = "D1UV";
      this.D1UV.Size = new Size(70, 24);
      this.D1UV.TabIndex = 47;
      this.D1UV.Text = "2nd UV";
      this.D1UV.TextAlign = ContentAlignment.MiddleCenter;
      this.D1UV.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1UV.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1PRE.BackColor = Color.Transparent;
      this.D1PRE.CausesValidation = false;
      this.D1PRE.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1PRE.ForeColor = Color.White;
      this.D1PRE.Location = new Point(187, 352);
      this.D1PRE.Name = "D1PRE";
      this.D1PRE.Size = new Size(70, 24);
      this.D1PRE.TabIndex = 46;
      this.D1PRE.Text = "2nd PREC";
      this.D1PRE.TextAlign = ContentAlignment.MiddleCenter;
      this.D1PRE.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1PRE.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1HUM.BackColor = Color.Transparent;
      this.D1HUM.CausesValidation = false;
      this.D1HUM.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1HUM.ForeColor = Color.White;
      this.D1HUM.Location = new Point(187, 296);
      this.D1HUM.Name = "D1HUM";
      this.D1HUM.Size = new Size(70, 24);
      this.D1HUM.TabIndex = 45;
      this.D1HUM.Text = "2nd H";
      this.D1HUM.TextAlign = ContentAlignment.MiddleCenter;
      this.D1HUM.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1HUM.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1LT.BackColor = Color.Transparent;
      this.D1LT.CausesValidation = false;
      this.D1LT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1LT.ForeColor = Color.White;
      this.D1LT.Location = new Point(187, 268);
      this.D1LT.Name = "D1LT";
      this.D1LT.Size = new Size(70, 24);
      this.D1LT.TabIndex = 44;
      this.D1LT.Text = "2nd LT";
      this.D1LT.TextAlign = ContentAlignment.MiddleCenter;
      this.D1LT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1LT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D1HT.BackColor = Color.Transparent;
      this.D1HT.CausesValidation = false;
      this.D1HT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D1HT.ForeColor = Color.White;
      this.D1HT.Location = new Point(187, 240);
      this.D1HT.Name = "D1HT";
      this.D1HT.Size = new Size(70, 24);
      this.D1HT.TabIndex = 43;
      this.D1HT.Text = "2nd HT";
      this.D1HT.TextAlign = ContentAlignment.MiddleCenter;
      this.D1HT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D1HT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2WD.BackColor = Color.Transparent;
      this.D2WD.Location = new Point(272, 416);
      this.D2WD.Name = "D2WD";
      this.D2WD.Size = new Size(70, 70);
      this.D2WD.TabIndex = 58;
      this.D2WD.TabStop = false;
      this.D2WD.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D2WD.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D2WDN.BackColor = Color.Transparent;
      this.D2WDN.CausesValidation = false;
      this.D2WDN.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2WDN.ForeColor = Color.White;
      this.D2WDN.Location = new Point(272, 404);
      this.D2WDN.Name = "D2WDN";
      this.D2WDN.Size = new Size(66, 10);
      this.D2WDN.TabIndex = 57;
      this.D2WDN.Text = "3rd WDN";
      this.D2WDN.TextAlign = ContentAlignment.TopRight;
      this.D2WDN.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2WDN.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2WS.BackColor = Color.Transparent;
      this.D2WS.CausesValidation = false;
      this.D2WS.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2WS.ForeColor = Color.White;
      this.D2WS.Location = new Point(272, 380);
      this.D2WS.Name = "D2WS";
      this.D2WS.Size = new Size(70, 24);
      this.D2WS.TabIndex = 56;
      this.D2WS.Text = "3rd WS";
      this.D2WS.TextAlign = ContentAlignment.MiddleCenter;
      this.D2WS.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2WS.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2UV.BackColor = Color.Transparent;
      this.D2UV.CausesValidation = false;
      this.D2UV.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2UV.ForeColor = Color.White;
      this.D2UV.Location = new Point(272, 324);
      this.D2UV.Name = "D2UV";
      this.D2UV.Size = new Size(70, 24);
      this.D2UV.TabIndex = 55;
      this.D2UV.Text = "3rd UV";
      this.D2UV.TextAlign = ContentAlignment.MiddleCenter;
      this.D2UV.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2UV.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2PRE.BackColor = Color.Transparent;
      this.D2PRE.CausesValidation = false;
      this.D2PRE.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2PRE.ForeColor = Color.White;
      this.D2PRE.Location = new Point(272, 352);
      this.D2PRE.Name = "D2PRE";
      this.D2PRE.Size = new Size(70, 24);
      this.D2PRE.TabIndex = 54;
      this.D2PRE.Text = "3rd PREC";
      this.D2PRE.TextAlign = ContentAlignment.MiddleCenter;
      this.D2PRE.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2PRE.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2HUM.BackColor = Color.Transparent;
      this.D2HUM.CausesValidation = false;
      this.D2HUM.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2HUM.ForeColor = Color.White;
      this.D2HUM.Location = new Point(272, 296);
      this.D2HUM.Name = "D2HUM";
      this.D2HUM.Size = new Size(70, 24);
      this.D2HUM.TabIndex = 53;
      this.D2HUM.Text = "3rd H";
      this.D2HUM.TextAlign = ContentAlignment.MiddleCenter;
      this.D2HUM.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2HUM.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2LT.BackColor = Color.Transparent;
      this.D2LT.CausesValidation = false;
      this.D2LT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2LT.ForeColor = Color.White;
      this.D2LT.Location = new Point(272, 268);
      this.D2LT.Name = "D2LT";
      this.D2LT.Size = new Size(70, 24);
      this.D2LT.TabIndex = 52;
      this.D2LT.Text = "3rd LT";
      this.D2LT.TextAlign = ContentAlignment.MiddleCenter;
      this.D2LT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2LT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D2HT.BackColor = Color.Transparent;
      this.D2HT.CausesValidation = false;
      this.D2HT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D2HT.ForeColor = Color.White;
      this.D2HT.Location = new Point(272, 240);
      this.D2HT.Name = "D2HT";
      this.D2HT.Size = new Size(70, 24);
      this.D2HT.TabIndex = 51;
      this.D2HT.Text = "3rd HT";
      this.D2HT.TextAlign = ContentAlignment.MiddleCenter;
      this.D2HT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D2HT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3WD.BackColor = Color.Transparent;
      this.D3WD.Location = new Point(357, 416);
      this.D3WD.Name = "D3WD";
      this.D3WD.Size = new Size(70, 70);
      this.D3WD.TabIndex = 66;
      this.D3WD.TabStop = false;
      this.D3WD.MouseMove += new MouseEventHandler(this.pic_mousemove);
      this.D3WD.MouseDown += new MouseEventHandler(this.pic_mousedown);
      this.D3WDN.BackColor = Color.Transparent;
      this.D3WDN.CausesValidation = false;
      this.D3WDN.Font = new Font("Arial", 6.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3WDN.ForeColor = Color.White;
      this.D3WDN.Location = new Point(357, 404);
      this.D3WDN.Name = "D3WDN";
      this.D3WDN.Size = new Size(66, 10);
      this.D3WDN.TabIndex = 65;
      this.D3WDN.Text = "4th WDN";
      this.D3WDN.TextAlign = ContentAlignment.TopRight;
      this.D3WDN.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3WDN.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3WS.BackColor = Color.Transparent;
      this.D3WS.CausesValidation = false;
      this.D3WS.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3WS.ForeColor = Color.White;
      this.D3WS.Location = new Point(357, 380);
      this.D3WS.Name = "D3WS";
      this.D3WS.Size = new Size(70, 24);
      this.D3WS.TabIndex = 64;
      this.D3WS.Text = "4th WS";
      this.D3WS.TextAlign = ContentAlignment.MiddleCenter;
      this.D3WS.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3WS.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3UV.BackColor = Color.Transparent;
      this.D3UV.CausesValidation = false;
      this.D3UV.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3UV.ForeColor = Color.White;
      this.D3UV.Location = new Point(357, 324);
      this.D3UV.Name = "D3UV";
      this.D3UV.Size = new Size(70, 24);
      this.D3UV.TabIndex = 63;
      this.D3UV.Text = "4th UV";
      this.D3UV.TextAlign = ContentAlignment.MiddleCenter;
      this.D3UV.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3UV.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3PRE.BackColor = Color.Transparent;
      this.D3PRE.CausesValidation = false;
      this.D3PRE.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3PRE.ForeColor = Color.White;
      this.D3PRE.Location = new Point(357, 352);
      this.D3PRE.Name = "D3PRE";
      this.D3PRE.Size = new Size(70, 24);
      this.D3PRE.TabIndex = 62;
      this.D3PRE.Text = "4th PREC";
      this.D3PRE.TextAlign = ContentAlignment.MiddleCenter;
      this.D3PRE.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3PRE.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3HUM.BackColor = Color.Transparent;
      this.D3HUM.CausesValidation = false;
      this.D3HUM.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3HUM.ForeColor = Color.White;
      this.D3HUM.Location = new Point(357, 296);
      this.D3HUM.Name = "D3HUM";
      this.D3HUM.Size = new Size(70, 24);
      this.D3HUM.TabIndex = 61;
      this.D3HUM.Text = "4th H";
      this.D3HUM.TextAlign = ContentAlignment.MiddleCenter;
      this.D3HUM.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3HUM.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3LT.BackColor = Color.Transparent;
      this.D3LT.CausesValidation = false;
      this.D3LT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3LT.ForeColor = Color.White;
      this.D3LT.Location = new Point(357, 268);
      this.D3LT.Name = "D3LT";
      this.D3LT.Size = new Size(70, 24);
      this.D3LT.TabIndex = 60;
      this.D3LT.Text = "4th LT";
      this.D3LT.TextAlign = ContentAlignment.MiddleCenter;
      this.D3LT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3LT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.D3HT.BackColor = Color.Transparent;
      this.D3HT.CausesValidation = false;
      this.D3HT.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.D3HT.ForeColor = Color.White;
      this.D3HT.Location = new Point(357, 240);
      this.D3HT.Name = "D3HT";
      this.D3HT.Size = new Size(70, 24);
      this.D3HT.TabIndex = 59;
      this.D3HT.Text = "4th HT";
      this.D3HT.TextAlign = ContentAlignment.MiddleCenter;
      this.D3HT.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.D3HT.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.settingbutton.BackColor = Color.Transparent;
      this.settingbutton.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.settingbutton.ForeColor = Color.DarkGray;
      this.settingbutton.Image = (Image) Resources.button2;
      this.settingbutton.Location = new Point(11, 492);
      this.settingbutton.Name = "settingbutton";
      this.settingbutton.Size = new Size(84, 40);
      this.settingbutton.TabIndex = 67;
      this.settingbutton.Text = "SETTING...";
      this.settingbutton.TextAlign = ContentAlignment.MiddleCenter;
      this.settingbutton.MouseLeave += new EventHandler(this.button_mouseleave);
      this.settingbutton.Click += new EventHandler(this.settingbutton_Click);
      this.settingbutton.MouseEnter += new EventHandler(this.button_mouseenter);
      this.updatebutton.BackColor = Color.Transparent;
      this.updatebutton.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.updatebutton.ForeColor = Color.DarkGray;
      this.updatebutton.Image = (Image) Resources.button2;
      this.updatebutton.Location = new Point(122, 492);
      this.updatebutton.Name = "updatebutton";
      this.updatebutton.Size = new Size(84, 40);
      this.updatebutton.TabIndex = 68;
      this.updatebutton.Text = "UPDATE FORECAST";
      this.updatebutton.TextAlign = ContentAlignment.MiddleCenter;
      this.updatebutton.MouseLeave += new EventHandler(this.button_mouseleave);
      this.updatebutton.Click += new EventHandler(this.updatebutton_Click);
      this.updatebutton.MouseEnter += new EventHandler(this.button_mouseenter);
      this.updaterecbutton.BackColor = Color.Transparent;
      this.updaterecbutton.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.updaterecbutton.ForeColor = Color.DarkGray;
      this.updaterecbutton.Image = (Image) Resources.button2;
      this.updaterecbutton.Location = new Point(234, 492);
      this.updaterecbutton.Name = "updaterecbutton";
      this.updaterecbutton.Size = new Size(84, 40);
      this.updaterecbutton.TabIndex = 69;
      this.updaterecbutton.Text = "SEND TO RECEIVER";
      this.updaterecbutton.TextAlign = ContentAlignment.MiddleCenter;
      this.updaterecbutton.MouseLeave += new EventHandler(this.button_mouseleave);
      this.updaterecbutton.Click += new EventHandler(this.updaterecbutton_Click);
      this.updaterecbutton.MouseEnter += new EventHandler(this.button_mouseenter);
      this.closebutton.BackColor = Color.Transparent;
      this.closebutton.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.closebutton.ForeColor = Color.DarkGray;
      this.closebutton.Image = (Image) Resources.button2;
      this.closebutton.Location = new Point(347, 492);
      this.closebutton.Name = "closebutton";
      this.closebutton.Size = new Size(84, 40);
      this.closebutton.TabIndex = 70;
      this.closebutton.Text = "CLOSE";
      this.closebutton.TextAlign = ContentAlignment.MiddleCenter;
      this.closebutton.MouseLeave += new EventHandler(this.button_mouseleave);
      this.closebutton.Click += new EventHandler(this.closebutton_Click);
      this.closebutton.MouseEnter += new EventHandler(this.button_mouseenter2);
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
      this.notifyIcon1.Icon = (Icon) componentResourceManager.GetObject("notifyIcon1.Icon");
      this.notifyIcon1.Text = "4-Day Forecast";
      this.notifyIcon1.Visible = true;
      this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(this.notify_mousedoubleclick);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.oPENToolStripMenuItem,
        (ToolStripItem) this.eXITToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(111, 48);
      this.oPENToolStripMenuItem.Name = "oPENToolStripMenuItem";
      this.oPENToolStripMenuItem.Size = new Size(110, 22);
      this.oPENToolStripMenuItem.Text = "OPEN";
      this.oPENToolStripMenuItem.Click += new EventHandler(this.oPENToolStripMenuItem_Click);
      this.eXITToolStripMenuItem.Name = "eXITToolStripMenuItem";
      this.eXITToolStripMenuItem.Size = new Size(110, 22);
      this.eXITToolStripMenuItem.Text = "EXIT";
      this.eXITToolStripMenuItem.Click += new EventHandler(this.eXITToolStripMenuItem_Click);
      this.timer2.Tick += new EventHandler(this.timer2_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.BackgroundImage = (Image) Resources.Background;
      this.ClientSize = new Size(443, 538);
      this.Controls.Add((Control) this.lb_CityName);
      this.Controls.Add((Control) this.closebutton);
      this.Controls.Add((Control) this.updaterecbutton);
      this.Controls.Add((Control) this.updatebutton);
      this.Controls.Add((Control) this.settingbutton);
      this.Controls.Add((Control) this.D3WD);
      this.Controls.Add((Control) this.D3WDN);
      this.Controls.Add((Control) this.D3WS);
      this.Controls.Add((Control) this.D3UV);
      this.Controls.Add((Control) this.D3PRE);
      this.Controls.Add((Control) this.D3HUM);
      this.Controls.Add((Control) this.D3LT);
      this.Controls.Add((Control) this.D3HT);
      this.Controls.Add((Control) this.D2WD);
      this.Controls.Add((Control) this.D2WDN);
      this.Controls.Add((Control) this.D2WS);
      this.Controls.Add((Control) this.D2UV);
      this.Controls.Add((Control) this.D2PRE);
      this.Controls.Add((Control) this.D2HUM);
      this.Controls.Add((Control) this.D2LT);
      this.Controls.Add((Control) this.D2HT);
      this.Controls.Add((Control) this.D1WD);
      this.Controls.Add((Control) this.D1WDN);
      this.Controls.Add((Control) this.D1WS);
      this.Controls.Add((Control) this.D1UV);
      this.Controls.Add((Control) this.D1PRE);
      this.Controls.Add((Control) this.D1HUM);
      this.Controls.Add((Control) this.D1LT);
      this.Controls.Add((Control) this.D1HT);
      this.Controls.Add((Control) this.D3PIC);
      this.Controls.Add((Control) this.D2PIC);
      this.Controls.Add((Control) this.D1PIC);
      this.Controls.Add((Control) this.D0WD);
      this.Controls.Add((Control) this.D0PIC);
      this.Controls.Add((Control) this.D0WDN);
      this.Controls.Add((Control) this.D0WS);
      this.Controls.Add((Control) this.D0UV);
      this.Controls.Add((Control) this.D0PRE);
      this.Controls.Add((Control) this.D0HUM);
      this.Controls.Add((Control) this.D0LT);
      this.Controls.Add((Control) this.D0HT);
      this.Controls.Add((Control) this.lb_direction);
      this.Controls.Add((Control) this.lb_wind);
      this.Controls.Add((Control) this.lb_precipitation);
      this.Controls.Add((Control) this.lb_uvindex);
      this.Controls.Add((Control) this.lb_HUMIDIY);
      this.Controls.Add((Control) this.lb_LOW);
      this.Controls.Add((Control) this.lb_HIGH);
      this.Controls.Add((Control) this.D3TEXT);
      this.Controls.Add((Control) this.D2TEXT);
      this.Controls.Add((Control) this.D1TEXT);
      this.Controls.Add((Control) this.D0TEXT);
      this.Controls.Add((Control) this.D3DAY);
      this.Controls.Add((Control) this.D2DAY);
      this.Controls.Add((Control) this.D1DAY);
      this.Controls.Add((Control) this.D0DAY);
      this.Controls.Add((Control) this.sunset);
      this.Controls.Add((Control) this.sunrise);
      this.Controls.Add((Control) this.lb_Sunset);
      this.Controls.Add((Control) this.lb_Sunrise);
      this.Controls.Add((Control) this.poweredby);
      this.Controls.Add((Control) this.lb_Lastupdate);
      this.Controls.Add((Control) this.tb_Title);
      this.Controls.Add((Control) this.lb_HOMEWEATHER);
      this.Controls.Add((Control) this.lb_Date);
      this.Controls.Add((Control) this.lb_Time);
      this.Controls.Add((Control) this.lb_CITY1);
      this.Controls.Add((Control) this.lb_CITY4);
      this.Controls.Add((Control) this.lb_CITY3);
      this.Controls.Add((Control) this.lb_CITY2);
      this.Cursor = Cursors.AppStarting;
      this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "Main";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "4-Day Forecast";
      this.TransparencyKey = Color.Black;
      this.Load += new EventHandler(this.Form1_Load);
      this.MouseDown += new MouseEventHandler(this.Main_MouseDown);
      this.MouseMove += new MouseEventHandler(this.Main_MouseMove);
      ((ISupportInitialize) this.D0PIC).EndInit();
      ((ISupportInitialize) this.D0WD).EndInit();
      ((ISupportInitialize) this.D1PIC).EndInit();
      ((ISupportInitialize) this.D2PIC).EndInit();
      ((ISupportInitialize) this.D3PIC).EndInit();
      ((ISupportInitialize) this.D1WD).EndInit();
      ((ISupportInitialize) this.D2WD).EndInit();
      ((ISupportInitialize) this.D3WD).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private Control GetControlByName(string name)
    {
      return this.controlHashtable[(object) name] as Control;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.controlHashtable = new Hashtable();
      foreach (Control control in (ArrangedElementCollection) this.Controls)
        this.controlHashtable.Add((object) control.Name, (object) control);
      if (System.IO.File.Exists(this.DataPath + "pcdata.bin"))
        this.loadsave();
      else
        this.savepc();
      DateTime dateTime;
      if (this.func_getutctime(5, false))
      {
        dateTime = DateTime.UtcNow;
        this.LastUpdateTime = dateTime.Add(this.pc_utc_timediff);
        this.networkok = true;
      }
      else
        this.networkok = false;
      if (System.IO.File.Exists(this.DataPath + "Lastupdate.bin"))
      {
        StreamReader streamReader = new StreamReader(this.DataPath + "Lastupdate.bin");
        this.LastUpdateTime = Convert.ToDateTime(streamReader.ReadLine(), (IFormatProvider) this.us_culture);
        streamReader.Close();
      }
      if (Program.Startup)
        this.WindowState = FormWindowState.Minimized;
      if (this.networkok)
        this.func_getcityver(5, false);
      if (this.networkok)
      {
        this.updateweather(false);
      }
      else
      {
        dateTime = DateTime.UtcNow;
        dateTime = dateTime.Add(this.pc_utc_timediff);
        this.NextUpdateTime = dateTime.AddMinutes(5.0);
      }
      this.changeformat();
      this.displaytime();
      this.displayinfo();
      if (this.networkok && this.func_apver(5, false) && (new ErrorBox(3).ShowDialog() == DialogResult.OK && !this.func_getapexe()) && (new ErrorBox(6).ShowDialog() == DialogResult.OK && !this.func_getapzip()))
      {
        int num = (int) new ErrorBox(7).ShowDialog();
      }
      this.Show();
      if (!(this.cdata[0].cityid == "") || !(this.cdata[1].cityid == "") || (!(this.cdata[2].cityid == "") || !(this.cdata[3].cityid == "")) || !(this.cdata[4].cityid == ""))
        this.updatedevice = !Program.Startup ? 2 : 1;
      else
        this.Cursor = Cursors.Default;
      this.timer1.Start();
      this.timer2.Start();
    }

    private bool syncclock(int retrytime, bool prompt_error)
    {
      DateTime now = DateTime.Now;
      int num1 = 0;
      if (this.usbobj.IsConnected)
      {
        while (true)
        {
          while (num1 <= retrytime && !this.exitfg)
          {
            this.usbfunc = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
            this.usbfunc.Start((object) usbjob.usb_lock);
            while (this.usbfunc.IsAlive && this.usbobj.IsConnected)
            {
              Application.DoEvents();
              Thread.Sleep(10);
            }
            if (this.usbobj.locksuccess)
            {
              this.usbobj.citytimediff = this.cdata[0].citytimediff;
              this.usbobj.DSTEN = this.cdata[0].DSTEN;
              this.usbobj.citytimeoffset = this.cdata[0].citytimeoffset;
              this.usbobj.pc_utc_timediff = this.pc_utc_timediff;
              this.usbobj.retrytime = retrytime;
              this.usbobj.retrycount = retrytime;
              this.usbfunc = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
              this.usbfunc.Priority = ThreadPriority.AboveNormal;
              this.usbfunc.Start((object) usbjob.Synclock);
              while (this.usbfunc.IsAlive && this.usbobj.IsConnected)
              {
                Application.DoEvents();
                Thread.Sleep(10);
              }
              if (this.usbobj.getsuccess())
              {
                int num2 = 0;
                while (num2 <= retrytime && !this.exitfg)
                {
                  this.usbfunc = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
                  this.usbfunc.Start((object) usbjob.usb_lock);
                  while (this.usbfunc.IsAlive && this.usbobj.IsConnected)
                  {
                    Application.DoEvents();
                    Thread.Sleep(10);
                  }
                  if (this.usbobj.locksuccess)
                  {
                    this.usbobj.retrytime = retrytime;
                    this.usbobj.retrycount = retrytime;
                    this.usbfunc = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
                    this.usbfunc.Priority = ThreadPriority.AboveNormal;
                    this.usbfunc.Start((object) usbjob.SendData);
                    while (this.usbfunc.IsAlive && this.usbobj.IsConnected)
                    {
                      Application.DoEvents();
                      Thread.Sleep(10);
                    }
                    if (this.usbobj.getsuccess())
                      return true;
                    ++num2;
                  }
                }
                if (prompt_error && (!this.exitfg && !this.usbobj.gotinterrupt && new ErrorBox(9).ShowDialog() == DialogResult.OK))
                {
                  num1 = 0;
                  retrytime = 6;
                }
                else
                {
                  if (!prompt_error)
                    this.NextUpdateTime = !this.networkok ? DateTime.UtcNow.Add(this.pc_utc_timediff).AddMinutes(5.0) : DateTime.UtcNow.Add(this.pc_utc_timediff).AddMinutes(15.0);
                  return false;
                }
              }
              else
                ++num1;
            }
          }
          if (prompt_error && (!this.exitfg && !this.usbobj.gotinterrupt && new ErrorBox(9).ShowDialog() == DialogResult.OK))
          {
            num1 = 0;
            retrytime = 6;
          }
          else
            break;
        }
        if (!prompt_error)
          this.NextUpdateTime = !this.networkok ? DateTime.UtcNow.Add(this.pc_utc_timediff).AddMinutes(5.0) : DateTime.UtcNow.Add(this.pc_utc_timediff).AddMinutes(15.0);
      }
      return false;
    }

    private void lb_HOMEWEATHER_Click(object sender, EventArgs e)
    {
      this.set_tab(0);
    }

    private void lb_CITY1_Click(object sender, EventArgs e)
    {
      this.set_tab(1);
    }

    private void lb_CITY2_Click(object sender, EventArgs e)
    {
      this.set_tab(2);
    }

    private void lb_CITY3_Click(object sender, EventArgs e)
    {
      this.set_tab(3);
    }

    private void lb_CITY4_Click(object sender, EventArgs e)
    {
      this.set_tab(4);
    }

    private void closebutton_Click(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
      this.Hide();
    }

    private void button_mouseenter(object sender, EventArgs e)
    {
      if (this.updatedevice == 0)
      {
        ((Control) sender).ForeColor = Color.White;
        this.Cursor = Cursors.Hand;
      }
      else
        ((Control) sender).ForeColor = Color.DarkGray;
    }

    private void button_mouseenter2(object sender, EventArgs e)
    {
      ((Control) sender).ForeColor = Color.White;
      this.Cursor = Cursors.Hand;
    }

    private void button_mouseleave(object sender, EventArgs e)
    {
      ((Control) sender).ForeColor = Color.DarkGray;
      if (this.updatedevice == 0)
        this.Cursor = Cursors.Default;
      else
        this.Cursor = Cursors.AppStarting;
    }

    private void oPENToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.Created)
        return;
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    private void Main_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left || this.mouse_offset.X == 5000 && this.mouse_offset.Y == 5000)
        return;
      Point mousePosition = Control.MousePosition;
      mousePosition.Offset(this.mouse_offset.X, this.mouse_offset.Y);
      this.Location = mousePosition;
    }

    private void Main_MouseDown(object sender, MouseEventArgs e)
    {
      this.mouse_offset = new Point(-e.X, -e.Y);
    }

    private void set_tab(int index)
    {
      if (index == 0)
      {
        this.lb_HOMEWEATHER.Image = (Image) Resources.tab_aHome;
        this.lb_HOMEWEATHER.ForeColor = Color.White;
        this.settingbutton.Text = "SETTING...";
        this.updaterecbutton.Visible = true;
      }
      else
      {
        this.lb_HOMEWEATHER.Image = (Image) Resources.tab_dHome;
        this.lb_HOMEWEATHER.ForeColor = Color.DarkGray;
        this.settingbutton.Text = "SET LOCATION...";
        this.updaterecbutton.Visible = false;
      }
      if (index == 1)
      {
        this.lb_CITY1.Image = (Image) Resources.tab_aCITY1;
        this.lb_CITY1.ForeColor = Color.White;
      }
      else
      {
        this.lb_CITY1.Image = (Image) Resources.tab_dCITY1;
        this.lb_CITY1.ForeColor = Color.DarkGray;
      }
      if (index == 2)
      {
        this.lb_CITY2.Image = (Image) Resources.tab_aCITY2;
        this.lb_CITY2.ForeColor = Color.White;
      }
      else
      {
        this.lb_CITY2.Image = (Image) Resources.tab_dCITY2;
        this.lb_CITY2.ForeColor = Color.DarkGray;
      }
      if (index == 3)
      {
        this.lb_CITY3.Image = (Image) Resources.tab_aCITY3;
        this.lb_CITY3.ForeColor = Color.White;
      }
      else
      {
        this.lb_CITY3.Image = (Image) Resources.tab_dCITY3;
        this.lb_CITY3.ForeColor = Color.DarkGray;
      }
      if (index == 4)
      {
        this.lb_CITY4.Image = (Image) Resources.tab_aCITY4;
        this.lb_CITY4.ForeColor = Color.White;
      }
      else
      {
        this.lb_CITY4.Image = (Image) Resources.tab_dCITY4;
        this.lb_CITY4.ForeColor = Color.DarkGray;
      }
      this.curcity = index;
      this.displaytime();
      this.displayinfo();
    }

    private void label_mousedown(object sender, MouseEventArgs e)
    {
      Point point;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Point& local = @point;
      Point location = ((Control) sender).Location;
      int x = location.X;
      location = ((Control) sender).Location;
      int y = location.Y;
      // ISSUE: explicit reference operation
      ^local = new Point(x, y);
      this.mouse_offset = new Point(-e.X - point.X, -e.Y - point.Y);
    }

    private void label_mousemove(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left || this.mouse_offset.X == 5000 && this.mouse_offset.Y == 5000)
        return;
      Point mousePosition = Control.MousePosition;
      mousePosition.Offset(this.mouse_offset.X, this.mouse_offset.Y);
      this.Location = mousePosition;
    }

    private void pic_mousedown(object sender, MouseEventArgs e)
    {
      Point point;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Point& local = @point;
      Point location = ((Control) sender).Location;
      int x = location.X;
      location = ((Control) sender).Location;
      int y = location.Y;
      // ISSUE: explicit reference operation
      ^local = new Point(x, y);
      this.mouse_offset = new Point(-e.X - point.X, -e.Y - point.Y);
    }

    private void pic_mousemove(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left || this.mouse_offset.X == 5000 && this.mouse_offset.Y == 5000)
        return;
      Point mousePosition = Control.MousePosition;
      mousePosition.Offset(this.mouse_offset.X, this.mouse_offset.Y);
      this.Location = mousePosition;
    }

    private string getHTTP(string addr, int buffersize, int timeout)
    {
      string str = "";
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(addr);
        httpWebRequest.Timeout = timeout;
        HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        byte[] buffer = new byte[buffersize];
        responseStream.Read(buffer, 0, buffersize);
        int num = 0;
        str = "";
        while (num < buffersize)
          str += (string) (object) (char) buffer[num++];
        responseStream.Close();
      }
      catch
      {
        str = "";
      }
      finally
      {
      }
      return str;
    }

    private bool func_getapexe()
    {
      string str = this.DataPath + this.newapver.Substring(0, this.newapver.IndexOf(";"));
      if (System.IO.File.Exists(str))
        System.IO.File.Delete(str);
      if (!this.getfile(this.ip[0] + "getapexe2.php?model=NIS01_INSTALL_1940", str, 999, 9999, 5))
        return false;
      new Process()
      {
        StartInfo = {
          FileName = str
        }
      }.Start();
      this.exitfg = true;
      return true;
    }

    private bool func_getapzip()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.FileName = this.newapver.Substring(this.newapver.IndexOf(";") + 1);
      saveFileDialog.Filter = "Zip Files (*.zip)|*.zip|All Files (*.*)|*.*";
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        string fileName = saveFileDialog.FileName;
        saveFileDialog.Dispose();
        if (System.IO.File.Exists(fileName))
          System.IO.File.Delete(fileName);
        if (!this.getfile(this.ip[0] + "getapzip2.php?model=NIS01_INSTALL_1940", fileName, 999, 9999, 5))
          return false;
        new Process()
        {
          StartInfo = {
            FileName = fileName
          }
        }.Start();
        this.exitfg = true;
        return true;
      }
      saveFileDialog.Dispose();
      return false;
    }

    private bool getfile(string addr, string file, int buffersize, int timeout, int retrytime)
    {
      for (int index = 0; index < retrytime; ++index)
      {
        if (this.getfile(addr, file, buffersize, timeout))
          return true;
      }
      return false;
    }

    private bool getfile(string addr, string file, int buffersize, int timeout)
    {
      FileStream fileStream = new FileStream(file, FileMode.Create);
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(addr);
        httpWebRequest.Timeout = timeout;
        HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        byte[] buffer = new byte[buffersize];
        int count;
        do
        {
          count = responseStream.Read(buffer, 0, buffersize);
          fileStream.Write(buffer, 0, count);
        }
        while (count > 0);
        responseStream.Close();
      }
      catch
      {
        return false;
      }
      finally
      {
        fileStream.Close();
      }
      return true;
    }

    private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Thread thread1 = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
      thread1.Start((object) usbjob.usb_lock);
      while (thread1.IsAlive && this.usbobj.IsConnected)
      {
        Application.DoEvents();
        Thread.Sleep(10);
      }
      if (this.usbobj.locksuccess)
      {
        if (new ErrorBox(2).ShowDialog() == DialogResult.OK)
        {
          this.exitfg = true;
        }
        else
        {
          Thread thread2 = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
          thread2.Start((object) usbjob.reset);
          thread2.Join();
        }
      }
      else
      {
        int num = (int) new ErrorBox(8).ShowDialog();
      }
    }

    private void displaytime()
    {
      DateTime dateTime1 = DateTime.UtcNow;
      dateTime1 = dateTime1.Add(this.pc_utc_timediff);
      this.last_displaytime0 = dateTime1.AddHours(this.cdata[0].citytimediff + (double) this.cdata[0].DSTEN + this.cdata[0].citytimeoffset);
      if (this.cdata[this.curcity].cityid == "")
      {
        DateTime dateTime2 = DateTime.UtcNow;
        dateTime2 = dateTime2.Add(this.pc_utc_timediff);
        this.last_displaytime = dateTime2.AddHours(this.cdata[this.curcity].citytimediff + (double) this.cdata[this.curcity].DSTEN + this.cdata[this.curcity].citytimeoffset);
        this.lb_Time.Text = "--:--";
        this.lb_Date.Text = "";
        this.lb_Lastupdate.Text = "";
        this.D1DAY.Text = this.daytostring(this.last_displaytime.AddDays(1.0).DayOfWeek.GetHashCode());
        this.D2DAY.Text = this.daytostring(this.last_displaytime.AddDays(2.0).DayOfWeek.GetHashCode());
        this.D3DAY.Text = this.daytostring(this.last_displaytime.AddDays(3.0).DayOfWeek.GetHashCode());
      }
      else
      {
        DateTime dateTime2 = DateTime.UtcNow;
        dateTime2 = dateTime2.Add(this.pc_utc_timediff);
        this.last_displaytime = dateTime2.AddHours(this.cdata[this.curcity].citytimediff + (double) this.cdata[this.curcity].DSTEN + this.cdata[this.curcity].citytimeoffset);
        DateTime dateTime3 = this.LastUpdateTime.AddHours(this.cdata[0].citytimediff + (double) this.cdata[0].DSTEN + this.cdata[0].citytimeoffset);
        this.lb_Time.Text = this.last_displaytime.ToString("t", (IFormatProvider) this.timeformat);
        if (this.time12hr)
        {
          if (this.last_displaytime.Hour == 0 || this.last_displaytime.Hour > 9 && this.last_displaytime.Hour < 13 || this.last_displaytime.Hour > 21)
            this.lb_Date.Location = new Point(94, 44);
          else
            this.lb_Date.Location = new Point(84, 44);
        }
        else if (this.last_displaytime.Hour < 10)
          this.lb_Date.Location = new Point(64, 44);
        else
          this.lb_Date.Location = new Point(74, 44);
        this.lb_Date.Text = this.last_displaytime.ToString("m", (IFormatProvider) this.timeformat);
        if (this.last_displaytime.Date != dateTime3.Date)
          this.lb_Lastupdate.Text = "Last Update - " + dateTime3.ToString("d", (IFormatProvider) this.timeformat) + " " + dateTime3.ToString("t", (IFormatProvider) this.timeformat);
        else
          this.lb_Lastupdate.Text = "Last Update - Today " + dateTime3.ToString("t", (IFormatProvider) this.timeformat);
        Label label1 = this.D1DAY;
        DateTime dateTime4 = this.last_displaytime.AddDays(1.0);
        string str1 = this.daytostring(dateTime4.DayOfWeek.GetHashCode());
        label1.Text = str1;
        Label label2 = this.D2DAY;
        dateTime4 = this.last_displaytime.AddDays(2.0);
        string str2 = this.daytostring(dateTime4.DayOfWeek.GetHashCode());
        label2.Text = str2;
        this.D3DAY.Text = this.daytostring(this.last_displaytime.AddDays(3.0).DayOfWeek.GetHashCode());
      }
    }

    private void displayinfo()
    {
      DateTime dateTime1 = new DateTime(this.cdata[this.curcity].weatherbank[0].year, this.cdata[this.curcity].weatherbank[0].month, this.cdata[this.curcity].weatherbank[0].day);
      DateTime dateTime2 = DateTime.UtcNow;
      dateTime2 = dateTime2.Add(this.pc_utc_timediff);
      int days = (dateTime2.AddHours(this.cdata[this.curcity].citytimediff + (double) this.cdata[this.curcity].DSTEN + this.cdata[this.curcity].citytimeoffset) - dateTime1).Days;
      this.lb_CityName.Text = this.cdata[this.curcity].cityname;
      this.displayinfo_day(days, 0);
      this.displayinfo_day(days, 1);
      this.displayinfo_day(days, 2);
      this.displayinfo_day(days, 3);
    }

    private void displayinfo_day(int daypass, int day)
    {
      if (daypass >= 10 - day || daypass < -day)
      {
        this.GetControlByName("D" + (object) day + "HT").Text = "---";
        this.GetControlByName("D" + (object) day + "LT").Text = "---";
        this.GetControlByName("D" + (object) day + "HUM").Text = "---";
        this.GetControlByName("D" + (object) day + "PRE").Text = "---";
        this.GetControlByName("D" + (object) day + "WS").Text = "---";
        this.GetControlByName("D" + (object) day + "UV").Text = "---";
        ((PictureBox) this.GetControlByName("D" + (object) day + "PIC")).Image = (Image) null;
        ((PictureBox) this.GetControlByName("D" + (object) day + "WD")).Image = (Image) null;
        this.GetControlByName("D" + (object) day + "TEXT").Text = "";
        this.GetControlByName("D" + (object) day + "WDN").Text = "";
        if (day != 0)
          return;
        this.sunset.Text = "--:--";
        this.sunrise.Text = "--:--";
      }
      else
      {
        this.GetControlByName("D" + (object) day + "TEXT").Text = this.forecasttext((int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[0]);
        this.GetControlByName("D" + (object) day + "WDN").Text = this.wdtostring(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[9]);
        this.GetControlByName("D" + (object) day + "HT").Text = this.fahtocel(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[1], this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[2]);
        this.GetControlByName("D" + (object) day + "LT").Text = this.fahtocel(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[3], this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[4]);
        this.GetControlByName("D" + (object) day + "HUM").Text = Convert.ToString(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[5]) + "%";
        this.GetControlByName("D" + (object) day + "PRE").Text = Convert.ToString(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[6]) + "%";
        this.GetControlByName("D" + (object) day + "WS").Text = this.wstostring(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[7], this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[8]);
        if ((int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[10] == (int) byte.MaxValue)
          this.GetControlByName("D" + (object) day + "UV").Text = "---";
        else
          this.GetControlByName("D" + (object) day + "UV").Text = Convert.ToString(this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[10]);
        ((PictureBox) this.GetControlByName("D" + (object) day + "PIC")).Image = this.weatherimage((int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[0]);
        if (this.GetControlByName("D" + (object) day + "WDN").Text != "")
          ((PictureBox) this.GetControlByName("D" + (object) day + "WD")).Image = (Image) Resources.ResourceManager.GetObject(this.GetControlByName("D" + (object) day + "WDN").Text);
        else
          ((PictureBox) this.GetControlByName("D" + (object) day + "WD")).Image = (Image) null;
        if (day == 0)
        {
          if ((int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[13] == (int) byte.MaxValue && (int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[14] == (int) byte.MaxValue)
            this.sunrise.Text = "--:--";
          else
            this.sunrise.Text = new DateTime(2000, 1, 1, (int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[13], (int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[14], 0, 0).ToString("t", (IFormatProvider) this.timeformat);
          if ((int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[15] == (int) byte.MaxValue && (int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[16] == (int) byte.MaxValue)
            this.sunset.Text = "--:--";
          else
            this.sunset.Text = new DateTime(2000, 1, 1, (int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[15], (int) this.cdata[this.curcity].weatherbank[daypass + day].forecastbytes[16], 0, 0).ToString("t", (IFormatProvider) this.timeformat);
        }
      }
    }

    private Image weatherimage(int weather)
    {
      string str = "_";
      if (weather == 1)
        weather = 0;
      return (Image) Resources.ResourceManager.GetObject(weather + 1 <= 9 ? str + "0" + Convert.ToString(weather + 1, 10) : str + Convert.ToString(weather + 1, 10));
    }

    private string daytostring(int day)
    {
      switch (day)
      {
        case 0:
          return "SUN";
        case 1:
          return "MON";
        case 2:
          return "TUE";
        case 3:
          return "WED";
        case 4:
          return "THUR";
        case 5:
          return "FRI";
        case 6:
          return "SAT";
        default:
          return "NON";
      }
    }

    private byte wdtointeger(string wd)
    {
      switch (wd)
      {
        case "N":
          return (byte) 0;
        case "NNE":
          return (byte) 1;
        case "NE":
          return (byte) 2;
        case "ENE":
          return (byte) 3;
        case "E":
          return (byte) 4;
        case "ESE":
          return (byte) 5;
        case "SE":
          return (byte) 6;
        case "SSE":
          return (byte) 7;
        case "S":
          return (byte) 8;
        case "SSW":
          return (byte) 9;
        case "SW":
          return (byte) 10;
        case "WSW":
          return (byte) 11;
        case "W":
          return (byte) 12;
        case "WNW":
          return (byte) 13;
        case "NW":
          return (byte) 14;
        case "NNW":
          return (byte) 15;
        default:
          return (byte) 16;
      }
    }

    private string wdtostring(byte wd)
    {
      switch (wd)
      {
        case (byte) 0:
          return "N";
        case (byte) 1:
          return "NNE";
        case (byte) 2:
          return "NE";
        case (byte) 3:
          return "ENE";
        case (byte) 4:
          return "E";
        case (byte) 5:
          return "ESE";
        case (byte) 6:
          return "SE";
        case (byte) 7:
          return "SSE";
        case (byte) 8:
          return "S";
        case (byte) 9:
          return "SSW";
        case (byte) 10:
          return "SW";
        case (byte) 11:
          return "WSW";
        case (byte) 12:
          return "W";
        case (byte) 13:
          return "WNW";
        case (byte) 14:
          return "NW";
        case (byte) 15:
          return "NNW";
        default:
          return "";
      }
    }

    private byte icontobyte(string icon)
    {
      icon = icon.Remove(0, 1);
      switch (icon)
      {
        case "000":
        case "110":
        case "111":
        case "112":
        case "120":
        case "121":
        case "122":
        case "130":
        case "131":
        case "132":
        case "140":
        case "141":
        case "142":
          return (byte) 0;
        case "100":
          return (byte) 1;
        case "200":
          return (byte) 2;
        case "210":
          return (byte) 3;
        case "211":
          return (byte) 4;
        case "212":
          return (byte) 5;
        case "220":
          return (byte) 6;
        case "221":
          return (byte) 7;
        case "222":
          return (byte) 8;
        case "240":
        case "241":
        case "242":
          return (byte) 9;
        case "300":
          return (byte) 10;
        case "310":
          return (byte) 11;
        case "311":
          return (byte) 12;
        case "312":
          return (byte) 13;
        case "320":
        case "230":
          return (byte) 14;
        case "321":
        case "231":
          return (byte) 15;
        case "322":
        case "232":
          return (byte) 16;
        case "340":
        case "341":
        case "342":
          return (byte) 17;
        case "400":
          return (byte) 18;
        case "410":
          return (byte) 19;
        case "411":
          return (byte) 20;
        case "412":
          return (byte) 21;
        case "420":
          return (byte) 22;
        case "421":
          return (byte) 23;
        case "422":
          return (byte) 24;
        case "430":
        case "330":
          return (byte) 25;
        case "431":
        case "331":
          return (byte) 26;
        case "432":
        case "332":
          return (byte) 27;
        case "440":
        case "441":
        case "442":
          return (byte) 28;
        default:
          return (byte) 0;
      }
    }

    private int timetoint(string timestr)
    {
      if (timestr.Length < 4)
        return -1;
      int num1 = 1;
      char ch1 = timestr[0];
      if ((int) ch1 > 57 || (int) ch1 < 48)
        return -1;
      int num2 = (int) ch1 - 48;
      char ch2 = timestr[1];
      if ((int) ch2 <= 57 && (int) ch2 >= 48)
      {
        num2 = num2 * 10 + (int) ch2 - 48;
      }
      else
      {
        if ((int) ch2 != 58)
          return -1;
        num1 = 0;
      }
      if (num1 == 1 && (int) timestr[2] != 58 || num2 > 23 || timestr.Length > 4 + num1)
        return -1;
      char ch3 = timestr[2 + num1];
      if ((int) ch3 > 57 || (int) ch3 < 48)
        return -1;
      int num3 = (int) ch3 - 48;
      char ch4 = timestr[3 + num1];
      if ((int) ch4 > 57 || (int) ch4 < 48)
        return -1;
      int num4 = num3 * 10 + (int) ch4 - 48;
      if (num4 > 60)
        return -1;
      return num2 * 100 + num4;
    }

    private string forecasttext(int forecast)
    {
      switch (forecast)
      {
        case 0:
          return "Clear";
        case 1:
          return "Clear";
        case 2:
          return "Partly cloudy";
        case 3:
          return "Partly cloudy with light rain";
        case 4:
          return "Partly cloudy with light sleet";
        case 5:
          return "Partly cloudy with light snow";
        case 6:
          return "Partly cloudy with showers";
        case 7:
          return "Partly cloudy with sleet showers";
        case 8:
          return "Partly cloudy with snow showers";
        case 9:
          return "Partly cloudy with thunderstorms";
        case 10:
          return "Mostly cloudy";
        case 11:
          return "Mostly cloudy with light rain";
        case 12:
          return "Mostly cloudy with light sleet";
        case 13:
          return "Mostly cloudy with light snow";
        case 14:
          return "Mostly cloudy with showers";
        case 15:
          return "Mostly cloudy with sleet showers";
        case 16:
          return "Mostly cloudy with snow showers";
        case 17:
          return "Mostly cloudy with thunderstorms";
        case 18:
          return "Overcast";
        case 19:
          return "Overcast with light rain";
        case 20:
          return "Overcast with light sleet";
        case 21:
          return "Overcast with light snow";
        case 22:
          return "Overcast with showers";
        case 23:
          return "Overcast with sleet showers";
        case 24:
          return "Overcast with snow showers";
        case 25:
          return "Overcast with rain";
        case 26:
          return "Overcast with sleet";
        case 27:
          return "Overcast with snow";
        case 28:
          return "Overcast with thunderstorms";
        default:
          return "Wrong number";
      }
    }

    private string fahtocel(byte highbyte, byte lowbyte)
    {
      double num = (double) ((int) highbyte * 256 + (int) lowbyte);
      if (num > 1220.0)
        num -= 65536.0;
      if (this.CEL)
        num = this.Round45((this.Round45(num / 10.0) * 10.0 - 320.0) / 1.8);
      string str = "" + Convert.ToString((int) this.Round45(num / 10.0), 10);
      return !this.CEL ? str + "°F" : str + "°C";
    }

    private double Round45(double round)
    {
      if (round - Math.Floor(round) < 0.5)
        return Math.Floor(round);
      return Math.Ceiling(round);
    }

    private string wstostring(byte hi, byte low)
    {
      int num = (int) hi * 256 + (int) low;
      switch (this.windspeedfor)
      {
        case 0:
          return Convert.ToString((int) this.Round45((double) num * 3.6 / 10.0), 10) + " km/h";
        case 1:
          return Convert.ToString((int) this.Round45(0.2778 * (double) (int) this.Round45((double) num * 3.6 / 10.0)), 10) + " m/s";
        case 2:
          return Convert.ToString((int) this.Round45((double) (int) this.Round45((double) num * 3.6 / 10.0) * 0.54), 10) + " knots";
        case 3:
          return Convert.ToString((int) this.Round45((double) (int) this.Round45((double) num * 3.6 / 10.0) * 0.6214), 10) + " mph";
        default:
          return "N/A";
      }
    }

    private void changeformat()
    {
      DateTime dateTime1 = DateTime.UtcNow;
      dateTime1 = dateTime1.Add(this.pc_utc_timediff);
      DateTime dateTime2 = dateTime1.AddHours(this.cdata[0].citytimediff + (double) this.cdata[0].DSTEN + this.cdata[0].citytimeoffset);
      this.timeformat.ShortTimePattern = !this.time12hr ? "H:mm" : "h:mmtt";
      if (this.daymonth)
      {
        this.timeformat.MonthDayPattern = "(d/M)";
        this.timeformat.ShortDatePattern = "d/M/yy";
      }
      else
      {
        this.timeformat.MonthDayPattern = "(M/d)";
        this.timeformat.ShortDatePattern = "M/d/yy";
      }
      this.checkDSTall(dateTime2.AddHours(-this.cdata[this.curcity].citytimeoffset));
    }

    private bool updateweather(bool prompterror)
    {
      int num1 = 0;
      DateTime dateTime;
      for (int citynum = 0; citynum < 5; ++citynum)
      {
        string weather = this.getweatherdata(this.cdata[citynum].cityid, 3, prompterror);
        if (this.cdata[citynum].cityid != "")
        {
          if (weather == "" || !this.pluginweather(weather, citynum, true))
          {
            if (prompterror && this.WindowState != FormWindowState.Minimized)
            {
              int num2 = (int) new ErrorBox(4).ShowDialog();
            }
            return false;
          }
        }
        else
        {
          ++num1;
          if (num1 == 5)
          {
            dateTime = DateTime.UtcNow;
            dateTime = dateTime.Add(this.pc_utc_timediff);
            this.NextUpdateTime = dateTime.AddHours(5.0);
          }
        }
      }
      dateTime = DateTime.UtcNow;
      this.LastUpdateTime = dateTime.Add(this.pc_utc_timediff);
      this.savepc();
      this.save4bit();
      return true;
    }

    private void save4bit()
    {
      byte[] buffer = new byte[8192];
      DateTime dateTime1 = DateTime.UtcNow.Add(this.pc_utc_timediff);
      buffer[10] = (byte) 196;
      buffer[17] = (byte) 1;
      buffer[20] = (byte) 25;
      buffer[21] = (byte) 5;
      buffer[22] = (byte) 10;
      buffer[23] = (byte) 99;
      buffer[24] = (byte) 12;
      buffer[25] = (byte) 31;
      buffer[26] = (byte) 23;
      buffer[27] = (byte) 59;
      buffer[28] = (byte) (dateTime1.AddHours(this.cdata[0].citytimediff + this.cdata[0].citytimeoffset + (double) this.cdata[0].DSTEN).Year - 2000);
      buffer[29] = (byte) dateTime1.AddHours(this.cdata[0].citytimediff + this.cdata[0].citytimeoffset + (double) this.cdata[0].DSTEN).Month;
      buffer[30] = (byte) dateTime1.AddHours(this.cdata[0].citytimediff + this.cdata[0].citytimeoffset + (double) this.cdata[0].DSTEN).Day;
      int num1 = 0;
      for (int index1 = 0; index1 < 5; ++index1)
      {
        DateTime dateTime2 = dateTime1.AddHours(this.cdata[index1].citytimediff + this.cdata[index1].citytimeoffset + (double) this.cdata[index1].DSTEN);
        byte num2 = (byte) (dateTime2.Year - 2000);
        dateTime2 = dateTime1.AddHours(this.cdata[index1].citytimediff + this.cdata[index1].citytimeoffset + (double) this.cdata[index1].DSTEN);
        byte num3 = (byte) dateTime2.Month;
        dateTime2 = dateTime1.AddHours(this.cdata[index1].citytimediff + this.cdata[index1].citytimeoffset + (double) this.cdata[index1].DSTEN);
        byte num4 = (byte) dateTime2.Day;
        int num5 = 0;
        for (int index2 = 0; index2 < 10; ++index2)
        {
          if ((int) num2 < (int) (byte) (this.cdata[index1].weatherbank[index2].year - 2000))
          {
            for (int index3 = 0; index3 < 25; ++index3)
            {
              if (index3 == 9 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 63;
              else if (index3 == 1 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 3 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 17 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 4;
              else if (index3 == 2 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 4 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 18 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 246;
              else if (index3 == 7 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 19 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 21 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue)
              {
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 2;
              }
              else
              {
                int num6 = index3 == 8 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 20 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue ? 0 : (index3 != 22 || (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] != (int) byte.MaxValue ? 1 : ((int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] != (int) byte.MaxValue ? 1 : 0));
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = num6 != 0 ? this.cdata[index1].weatherbank[index2].forecastbytes[index3] : (byte) 188;
              }
            }
            ++num5;
          }
          else if ((int) num2 == (int) (byte) (this.cdata[index1].weatherbank[index2].year - 2000) && (int) num3 < (int) (byte) this.cdata[index1].weatherbank[index2].month)
          {
            for (int index3 = 0; index3 < 25; ++index3)
            {
              if (index3 == 9 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 63;
              else if (index3 == 1 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 3 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 17 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 4;
              else if (index3 == 2 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 4 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 18 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 246;
              else if (index3 == 7 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 19 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 21 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue)
              {
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 2;
              }
              else
              {
                int num6 = index3 == 8 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 20 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue ? 0 : (index3 != 22 || (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] != (int) byte.MaxValue ? 1 : ((int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] != (int) byte.MaxValue ? 1 : 0));
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = num6 != 0 ? this.cdata[index1].weatherbank[index2].forecastbytes[index3] : (byte) 188;
              }
            }
            ++num5;
          }
          else if ((int) num2 == (int) (byte) (this.cdata[index1].weatherbank[index2].year - 2000) && (int) num3 == (int) (byte) this.cdata[index1].weatherbank[index2].month && (int) num4 <= (int) (byte) this.cdata[index1].weatherbank[index2].day)
          {
            for (int index3 = 0; index3 < 25; ++index3)
            {
              if (index3 == 9 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 63;
              else if (index3 == 1 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 3 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 17 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 4;
              else if (index3 == 2 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 4 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 18 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue)
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 246;
              else if (index3 == 7 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 19 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue || index3 == 21 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 + 1] == (int) byte.MaxValue)
              {
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = (byte) 2;
              }
              else
              {
                int num6 = index3 == 8 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue || index3 == 20 && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] == (int) byte.MaxValue && (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] == (int) byte.MaxValue ? 0 : (index3 != 22 || (int) this.cdata[index1].weatherbank[index2].forecastbytes[index3] != (int) byte.MaxValue ? 1 : ((int) this.cdata[index1].weatherbank[index2].forecastbytes[index3 - 1] != (int) byte.MaxValue ? 1 : 0));
                buffer[index1 * 250 + num5 * 25 + 34 + index3] = num6 != 0 ? this.cdata[index1].weatherbank[index2].forecastbytes[index3] : (byte) 188;
              }
            }
            ++num5;
          }
        }
        for (; num5 < 10; ++num5)
        {
          for (int index2 = 0; index2 < 25; ++index2)
          {
            if (index2 == 9)
              buffer[index1 * 250 + num5 * 25 + 34 + index2] = (byte) 63;
            else if (index2 == 1 || index2 == 3 || index2 == 17)
              buffer[index1 * 250 + num5 * 25 + 34 + index2] = (byte) 4;
            else if (index2 == 2 || index2 == 4 || index2 == 18)
              buffer[index1 * 250 + num5 * 25 + 34 + index2] = (byte) 246;
            else if (index2 == 7 || index2 == 19 || index2 == 21)
            {
              buffer[index1 * 250 + num5 * 25 + 34 + index2] = (byte) 2;
            }
            else
            {
              int num6 = index2 == 8 || index2 == 20 ? 0 : (index2 != 22 ? 1 : 0);
              buffer[index1 * 250 + num5 * 25 + 34 + index2] = num6 != 0 ? byte.MaxValue : (byte) 188;
            }
          }
        }
        buffer[1284] = (byte) num1;
        int length = this.cdata[index1].cityname.Length;
        if (length > num1)
          num1 = length;
      }
      for (int index1 = 0; index1 < 5; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          buffer[1285 + index1 * num1 + index2] = index2 < this.cdata[index1].cityname.Length ? (byte) this.cdata[index1].cityname[index2] : (byte) 0;
      }
      if (this.cdata[0].DSTvalid)
      {
        buffer[1285 + num1 * 5] = this.cdata[0].DSTEN;
        buffer[1286 + num1 * 5] = (byte) (this.cdata[0].NextDSTtime.Year - 2000);
        buffer[1287 + num1 * 5] = (byte) this.cdata[0].NextDSTtime.Month;
        buffer[1288 + num1 * 5] = (byte) this.cdata[0].NextDSTtime.Day;
        buffer[1289 + num1 * 5] = (byte) this.cdata[0].NextDSTtime.Hour;
      }
      else
      {
        buffer[1285 + num1 * 5] = byte.MaxValue;
        buffer[1286 + num1 * 5] = byte.MaxValue;
        buffer[1287 + num1 * 5] = byte.MaxValue;
        buffer[1288 + num1 * 5] = byte.MaxValue;
        buffer[1289 + num1 * 5] = byte.MaxValue;
      }
      for (int index = 1; index < 5; ++index)
      {
        buffer[1288 + num1 * 5 + index * 2] = this.cdata[index].DSTEN;
        double num2 = this.cdata[index].citytimediff - this.cdata[0].citytimediff + this.cdata[index].citytimeoffset - this.cdata[0].citytimeoffset;
        byte num3 = (byte) 0;
        if (num2 < 0.0)
          num3 = (byte) sbyte.MinValue;
        double num4 = Math.Abs(num2);
        byte num5 = (byte) ((double) ((int) num3 + (int) (byte) ((uint) (int) num4 * 4U)) + (num4 - (double) (int) num4) / 0.25);
        buffer[1289 + num1 * 5 + index * 2] = num5;
      }
      for (int index = 1; index < 5; ++index)
      {
        buffer[1295 + num1 * 5 + index * 3] = (byte) (this.cdata[index].weatherbank[0].year - 2000);
        buffer[1296 + num1 * 5 + index * 3] = (byte) this.cdata[index].weatherbank[0].month;
        buffer[1297 + num1 * 5 + index * 3] = (byte) this.cdata[index].weatherbank[0].day;
      }
      int count = 1310 + num1 * 5;
      buffer[18] = (byte) (count / 256);
      buffer[19] = (byte) (count % 256);
      if (System.IO.File.Exists(this.DataPath + "mcudata.bin"))
      {
        try
        {
          System.IO.File.Delete(this.DataPath + "mcudata.bin");
        }
        catch
        {
        }
        finally
        {
        }
      }
      FileStream fileStream = (FileStream) null;
      try
      {
        fileStream = new FileStream(this.DataPath + "mcudata.bin", FileMode.OpenOrCreate);
        fileStream.Write(buffer, 0, count);
      }
      catch
      {
      }
      finally
      {
        if (null != fileStream)
          fileStream.Close();
      }
    }

    private string getweatherdata(string cityid, int retrytime, bool prompterrer)
    {
      string input = "";
      string key_seed = "ansen's weather";
      char[] chArray = new char[1];
      for (int index = 0; index < retrytime; ++index)
      {
        if (cityid != "")
        {
          input = this.getHTTP(this.ip[0] + "getweather.php?city=" + cityid, 5000, 5000).Trim(chArray);
          if (input != "")
          {
            input = Crypto.decrypt3des(input, key_seed);
            break;
          }
        }
      }
      return input;
    }

    private bool func_apver(int retrytime, bool prompterror)
    {
      char[] chArray = new char[2]
      {
        char.MinValue,
        '\n'
      };
      for (int index1 = 0; index1 < retrytime; ++index1)
      {
        string str = this.getHTTP(this.ip[0] + "getapver2.php?model=NIS01_INSTALL_1940", 1000, 5000).Trim(chArray);
        if (str != "")
        {
          this.newap = true;
          this.newapver = str;
          for (int index2 = 0; index2 < this.apver.GetLength(0); ++index2)
          {
            if (this.apver[index2] == str)
              this.newap = false;
          }
          return this.newap;
        }
      }
      return false;
    }

    private bool func_getcityver(int retrytime, bool prompterror)
    {
      char[] chArray = new char[1];
      for (int index1 = 0; index1 < retrytime; ++index1)
      {
        string str1 = this.getHTTP(this.ip[0] + "getcityver2.php", 1000, 5000).Trim(chArray);
        if (str1 != "" && this.citylistver != str1)
        {
          if (this.getfile(this.ip[0] + "getcity2.php", this.DataPath + "temp.dat", 999, 5000, 5))
          {
            this.citylistver = str1;
            byte[] buffer = new byte[8000000];
            FileStream fileStream1 = new FileStream(this.DataPath + "temp.dat", FileMode.OpenOrCreate);
            fileStream1.Read(buffer, 0, 8000000);
            fileStream1.Close();
            int index2 = 0;
            while (true)
            {
              while ((int) buffer[index2] != 64)
                ++index2;
              int index3 = index2 + 1;
              if ((int) buffer[index3] != 35)
              {
                string str2 = "";
                while ((int) buffer[index3] != 10 && (int) buffer[index3] != 13)
                  str2 += (string) (object) (char) buffer[index3++];
                int index4 = index3 + 1;
                while ((int) buffer[index4] != 110 && (int) buffer[index4] != 13 && (int) buffer[index4] != 10)
                  ++index4;
                index2 = index4 + 1;
                int offset = index2;
                FileStream fileStream2 = new FileStream(this.DataPath + "db\\" + str2, FileMode.Create);
                while ((int) buffer[index2] != 64)
                  ++index2;
                fileStream2.Write(buffer, offset, index2 - offset);
                fileStream2.Close();
              }
              else
                break;
            }
            System.IO.File.Delete("temp.dat");
          }
          return true;
        }
      }
      return false;
    }

    private bool func_getutctime(int retrytime, bool prompterror)
    {
      int length = 0;
      TimeSpan[] array = new TimeSpan[retrytime];
      DateTime utcNow1 = DateTime.UtcNow;
      DateTime utcNow2 = DateTime.UtcNow;
      DateTime dateTime1 = DateTime.UtcNow;
      for (int index1 = 0; index1 < retrytime; ++index1)
      {
        string http = this.getHTTP(this.ip[0] + "gettime2.php", 999, 5000);
        if (http != "")
        {
          string[] strArray = http.Split(' ');
          for (int index2 = 0; index2 < strArray.GetLength(0); ++index2)
            strArray[index2] = strArray[index2].Trim();
          try
          {
            DateTime dateTime2 = new DateTime((long) (((double) (Convert.ToInt64(strArray[1], 10) + 62135596800L) + Convert.ToDouble(strArray[0])) * 10000000.0));
            if (index1 == 0)
            {
              utcNow1 = DateTime.UtcNow;
              dateTime1 = dateTime2;
            }
            else
            {
              array[length] = dateTime2 - dateTime1;
              utcNow1 = DateTime.UtcNow;
              dateTime1 = dateTime2;
            }
            ++length;
          }
          catch
          {
          }
        }
      }
      if (length < 3)
      {
        if (!prompterror)
          this.NextUpdateTime = !this.networkok ? DateTime.UtcNow.Add(this.pc_utc_timediff).AddMinutes(5.0) : DateTime.UtcNow.Add(this.pc_utc_timediff).AddMinutes(15.0);
        else if (this.WindowState != FormWindowState.Minimized)
        {
          int num = (int) new ErrorBox(4).ShowDialog();
        }
        return false;
      }
      Array.Sort<TimeSpan>(array, 0, length);
      TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, 0);
      for (int index = 0; index < length; ++index)
        timeSpan += array[index];
      this.pc_utc_timediff = dateTime1.AddMilliseconds(timeSpan.TotalMilliseconds / (double) length / 2.0) - utcNow1;
      return true;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (this.Created)
      {
        if (this.exitfg)
        {
          this.Hide();
          this.savepc();
          this.ShowInTaskbar = false;
          this.notifyIcon1.Visible = false;
          this.notifyIcon1.Dispose();
          Application.Exit();
        }
        else
          this.notifyIcon1.Visible = true;
        if (this.WindowState == FormWindowState.Minimized)
          this.Hide();
      }
      if (!this.usbobj.IsConnected)
      {
        this.timer1.Enabled = false;
        ErrorBox errorBox = new ErrorBox(1);
        while (this.usbfunc != null && this.usbfunc.IsAlive)
        {
          this.usbobj.job = 0;
          this.usbfunc.Abort();
        }
        do
          ;
        while (errorBox.ShowDialog() == DialogResult.OK && !this.usbobj.IsConnected);
        if (errorBox.DialogResult == DialogResult.Cancel)
          this.exitfg = true;
        this.timer1.Interval = 100;
        this.timer1.Enabled = true;
      }
      DateTime dateTime1 = DateTime.UtcNow.Add(this.pc_utc_timediff);
      DateTime dateTime2 = dateTime1.AddHours(this.cdata[0].citytimediff + (double) this.cdata[0].DSTEN + this.cdata[0].citytimeoffset);
      DateTime date1 = dateTime2.Date;
      dateTime1 = this.last_displaytime0.AddMinutes(1.0);
      DateTime date2 = dateTime1.Date;
      if (date1 > date2 || dateTime2.Date < this.last_displaytime0.Date)
      {
        if (this.func_getutctime(5, false))
          this.updateweather(false);
        if (dateTime2.Minute % 15 == 0)
          this.checkDSTall(dateTime2.AddHours(-this.cdata[this.curcity].citytimeoffset));
        this.displaytime();
        this.displayinfo();
      }
      if (dateTime2 >= this.NextUpdateTime.AddHours(this.cdata[0].citytimediff + (double) this.cdata[0].DSTEN + this.cdata[0].citytimeoffset))
      {
        this.Cursor = Cursors.AppStarting;
        if (this.func_getutctime(5, false))
        {
          this.updateweather(false);
          if (dateTime2.Minute % 15 == 0)
            this.checkDSTall(dateTime2.AddHours(-this.cdata[this.curcity].citytimeoffset));
          this.displaytime();
          this.displayinfo();
          if (this.updatedevice == 0)
            this.updatedevice = 3;
        }
      }
      if (this.last_displaytime0.Minute == dateTime2.Minute && this.last_displaytime0.Hour == dateTime2.Hour && !(this.last_displaytime0.Date != dateTime2.Date))
        return;
      if (dateTime2.Minute % 15 == 0)
        this.checkDSTall(dateTime2.AddHours(-this.cdata[this.curcity].citytimeoffset));
      if (this.last_displaytime0.Hour != dateTime2.Hour)
      {
        this.displaytime();
        this.displayinfo();
      }
      else
        this.displaytime();
    }

    private void loadsave()
    {
      FileStream fileStream = new FileStream(this.DataPath + "pcdata.bin", FileMode.OpenOrCreate);
      byte[] buffer = new byte[1000000];
      fileStream.Read(buffer, 0, 1000000);
      fileStream.Close();
      string input = "";
      for (int index = 0; (int) buffer[index] != 0; ++index)
        input += (string) (object) (char) buffer[index];
      string key_seed = "ansen's weather";
      string str1 = Crypto.decrypt3des(input, key_seed);
      int index1 = 0;
      while ((int) str1[index1] != 42)
        ++index1;
      int index2 = index1 + 1;
      this.CEL = (int) str1[index2] == 49;
      int index3 = index2 + 2;
      this.windspeedfor = (int) str1[index3] - 48;
      int index4 = index3 + 2;
      this.time12hr = (int) str1[index4] == 49;
      int index5 = index4 + 2;
      this.daymonth = (int) str1[index5] == 49;
      int index6 = index5 + 2;
      string str2 = "";
      while ((int) str1[index6] != 42)
        str2 += (string) (object) str1[index6++];
      this.cdata[0].citytimeoffset = Convert.ToDouble(str2);
      int index7 = index6 + 1;
      string str3 = "";
      while ((int) str1[index7] != 42)
        str3 += (string) (object) str1[index7++];
      int index8 = index7 + 1;
      string str4 = "";
      while ((int) str1[index8] != 45)
        str4 += (string) (object) str1[index8++];
      int index9 = index8 + 1;
      string str5 = "";
      while ((int) str1[index9] != 45)
        str5 += (string) (object) str1[index9++];
      int index10 = index9 + 1;
      string str6 = "";
      while ((int) str1[index10] != 45)
        str6 += (string) (object) str1[index10++];
      int index11 = index10 + 1;
      string str7 = "";
      while ((int) str1[index11] != 45)
        str7 += (string) (object) str1[index11++];
      int index12 = index11 + 1;
      string str8 = "";
      while ((int) str1[index12] != 37)
        str8 += (string) (object) str1[index12++];
      this.NextUpdateTime = new DateTime((int) Convert.ToInt16(str4, 10), (int) Convert.ToInt16(str5, 10), (int) Convert.ToInt16(str6, 10), (int) Convert.ToInt16(str7, 10), (int) Convert.ToInt16(str8, 10), 0);
      for (int index13 = 0; index13 < 5; ++index13)
      {
        while ((int) str1[index12] != 37)
          ++index12;
        int index14 = index12 + 1;
        string str9 = "";
        while ((int) str1[index14] != 42)
          str9 += (string) (object) str1[index14++];
        this.cdata[index13].cityname = str9;
        int index15 = index14 + 1;
        string str10 = "";
        while ((int) str1[index15] != 42)
          str10 += (string) (object) str1[index15++];
        this.cdata[index13].statename = str10;
        int index16 = index15 + 1;
        string str11 = "";
        while ((int) str1[index16] != 42)
          str11 += (string) (object) str1[index16++];
        this.cdata[index13].countryname = str11;
        int index17 = index16 + 1;
        string str12 = "";
        while ((int) str1[index17] != 42)
          str12 += (string) (object) str1[index17++];
        this.cdata[index13].cityid = str12;
        int index18 = index17 + 1;
        string str13 = "";
        while ((int) str1[index18] != 42)
          str13 += (string) (object) str1[index18++];
        this.cdata[index13].cur_con = (int) Convert.ToInt16(str13, 10);
        int index19 = index18 + 1;
        string str14 = "";
        while ((int) str1[index19] != 42)
          str14 += (string) (object) str1[index19++];
        this.cdata[index13].cur_country = (int) Convert.ToInt16(str14, 10);
        int index20 = index19 + 1;
        string str15 = "";
        while ((int) str1[index20] != 42)
          str15 += (string) (object) str1[index20++];
        this.cdata[index13].cur_state = (int) Convert.ToInt16(str15, 10);
        int index21 = index20 + 1;
        string str16 = "";
        while ((int) str1[index21] != 42)
          str16 += (string) (object) str1[index21++];
        this.cdata[index13].cur_city = (int) Convert.ToInt16(str16, 10);
        int index22 = index21 + 1;
        string str17 = "";
        while ((int) str1[index22] != 42)
          str17 += (string) (object) str1[index22++];
        this.cdata[index13].citytimediff = Convert.ToDouble(str17);
        int index23 = index22 + 1;
        string str18 = "";
        while ((int) str1[index23] != 35)
          str18 += (string) (object) str1[index23++];
        this.cdata[index13].cityDSTmode = str18;
        index12 = index23 + 1;
        for (int index24 = 0; index24 < 10; ++index24)
        {
          string str19 = "";
          while ((int) str1[index12] != 45)
            str19 += (string) (object) str1[index12++];
          this.cdata[index13].weatherbank[index24].year = (int) Convert.ToInt16(str19, 10);
          int index25 = index12 + 1;
          string str20 = "";
          while ((int) str1[index25] != 45)
            str20 += (string) (object) str1[index25++];
          this.cdata[index13].weatherbank[index24].month = (int) Convert.ToInt16(str20, 10);
          int index26 = index25 + 1;
          string str21 = "";
          while ((int) str1[index26] != 42)
            str21 += (string) (object) str1[index26++];
          this.cdata[index13].weatherbank[index24].day = (int) Convert.ToInt16(str21, 10);
          index12 = index26 + 1;
          for (int index27 = 0; index27 < 25; ++index27)
          {
            string str22 = "";
            while ((int) str1[index12] != 42 && (int) str1[index12] != 35 && (int) str1[index12] != 64)
              str22 += (string) (object) str1[index12++];
            this.cdata[index13].weatherbank[index24].forecastbytes[index27] = Convert.ToByte(str22, 10);
            if ((int) str1[index12] != 64)
              ++index12;
            else
              break;
          }
        }
      }
    }

    private void savepc()
    {
      string str1 = "" + (object) '@';
      string str2 = (string) (!this.CEL ? (object) (str1 + (object) '*' + (string) (object) '0') : (object) (str1 + (object) '*' + (string) (object) '1')) + (object) '*' + Convert.ToString(this.windspeedfor, 10);
      string str3 = !this.time12hr ? str2 + (object) '*' + (string) (object) '0' : str2 + (object) '*' + (string) (object) '1';
      string input = (string) (!this.daymonth ? (object) (str3 + (object) '*' + (string) (object) '0') : (object) (str3 + (object) '*' + (string) (object) '1')) + (object) '*' + Convert.ToString(this.cdata[0].citytimeoffset) + (object) '*' + (object) '*' + Convert.ToString(this.NextUpdateTime.Year, 10) + (string) (object) '-' + Convert.ToString(this.NextUpdateTime.Month, 10) + (string) (object) '-' + Convert.ToString(this.NextUpdateTime.Day, 10) + (string) (object) '-' + Convert.ToString(this.NextUpdateTime.Hour, 10) + (string) (object) '-' + Convert.ToString(this.NextUpdateTime.Second, 10);
      for (int index1 = 0; index1 < 5; ++index1)
      {
        string str4 = input + (object) '%' + this.cdata[index1].cityname + (string) (object) '*' + this.cdata[index1].statename + (string) (object) '*' + this.cdata[index1].countryname + (string) (object) '*' + this.cdata[index1].cityid + (string) (object) '*' + (string) (object) this.cdata[index1].cur_con + (string) (object) '*' + (string) (object) this.cdata[index1].cur_country + (string) (object) '*' + (string) (object) this.cdata[index1].cur_state + (string) (object) '*' + (string) (object) this.cdata[index1].cur_city + (string) (object) '*' + Convert.ToString(this.cdata[index1].citytimediff) + (string) (object) '*' + this.cdata[index1].cityDSTmode;
        for (int index2 = 0; index2 < 10; ++index2)
        {
          str4 = str4 + (object) '#' + Convert.ToString(this.cdata[index1].weatherbank[index2].year, 10) + (string) (object) '-' + Convert.ToString(this.cdata[index1].weatherbank[index2].month, 10) + (string) (object) '-' + Convert.ToString(this.cdata[index1].weatherbank[index2].day, 10);
          for (int index3 = 0; index3 < 25; ++index3)
            str4 = str4 + (object) '*' + Convert.ToString(this.cdata[index1].weatherbank[index2].forecastbytes[index3], 10);
        }
        input = str4 + (object) '@';
      }
      string key_seed = "ansen's weather";
      string str5 = Crypto.encrypt3des(input, key_seed);
      if (System.IO.File.Exists(this.DataPath + "pcdata.bin"))
        System.IO.File.Delete(this.DataPath + "pcdata.bin");
      FileStream fileStream = new FileStream(this.DataPath + "pcdata.bin", FileMode.OpenOrCreate);
      int length = str5.Length;
      byte[] buffer = new byte[length];
      for (int index = 0; index < length; ++index)
        buffer[index] = (byte) str5[index];
      fileStream.Write(buffer, 0, length);
      fileStream.Close();
    }

    private void settingbutton_Click(object sender, EventArgs e)
    {
      bool flag = false;
      if (this.updatedevice != 0)
        return;
      if (this.curcity == 0)
      {
        OptionDialog optionDialog = new OptionDialog();
        optionDialog.CEL = this.CEL;
        optionDialog.time12hr = this.time12hr;
        optionDialog.daymonth = this.daymonth;
        optionDialog.windspeedfor = this.windspeedfor;
        optionDialog.cdata = (citydata) this.cdata[this.curcity].Clone();
        optionDialog.pc_utc_timediff = this.pc_utc_timediff;
        optionDialog.DataPath = this.DataPath;
        if (optionDialog.ShowDialog() != DialogResult.OK)
          return;
        while (this.updatedevice != 0)
        {
          Thread.Sleep(10);
          Application.DoEvents();
        }
        this.CEL = optionDialog.CEL;
        this.time12hr = optionDialog.time12hr;
        this.daymonth = optionDialog.daymonth;
        this.windspeedfor = optionDialog.windspeedfor;
        if (this.cdata[this.curcity].citytimeoffset != optionDialog.cdata.citytimeoffset)
        {
          flag = true;
          this.cdata[this.curcity].citytimeoffset = optionDialog.cdata.citytimeoffset;
        }
        if (this.cdata[this.curcity].cityid != optionDialog.cdata.cityid)
        {
          flag = true;
          string weather = this.getweatherdata(optionDialog.cdata.cityid, 3, true);
          if (optionDialog.cdata.cityid != "" && (weather != "" && this.pluginweather(weather, this.curcity, false)))
            citydata.copy_wo_data(optionDialog.cdata, ref this.cdata[this.curcity]);
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          citydata& cdata = @this.cdata[this.curcity];
          DateTime dateTime = DateTime.UtcNow;
          dateTime = dateTime.Add(this.pc_utc_timediff);
          DateTime localtime = dateTime.AddHours(this.cdata[this.curcity].citytimediff + (double) this.cdata[this.curcity].DSTEN);
          string DataPath = this.DataPath;
          Main.checkDST(cdata, localtime, DataPath);
        }
        if (flag)
        {
          this.Cursor = Cursors.AppStarting;
          if (this.func_getutctime(5, true))
            this.updateweather(true);
          this.changeformat();
          this.displaytime();
          this.displayinfo();
          if (this.updatedevice == 0)
            this.updatedevice = 3;
        }
        else
        {
          this.changeformat();
          this.displaytime();
          this.displayinfo();
        }
      }
      else
      {
        SelectCity selectCity = new SelectCity();
        selectCity.cdata = (citydata) this.cdata[this.curcity].Clone();
        selectCity.DataPath = this.DataPath;
        if (selectCity.ShowDialog() == DialogResult.OK)
        {
          while (this.updatedevice != 0)
          {
            Thread.Sleep(10);
            Application.DoEvents();
          }
          if (this.cdata[this.curcity].citytimeoffset != selectCity.cdata.citytimeoffset)
          {
            flag = true;
            this.cdata[this.curcity].citytimeoffset = selectCity.cdata.citytimeoffset;
          }
          if (this.cdata[this.curcity].cityid != selectCity.cdata.cityid)
          {
            flag = true;
            string weather = this.getweatherdata(selectCity.cdata.cityid, 3, true);
            if (selectCity.cdata.cityid != "" && (weather != "" && this.pluginweather(weather, this.curcity, false)))
              citydata.copy_wo_data(selectCity.cdata, ref this.cdata[this.curcity]);
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            citydata& cdata = @this.cdata[this.curcity];
            DateTime dateTime = DateTime.UtcNow;
            dateTime = dateTime.Add(this.pc_utc_timediff);
            DateTime localtime = dateTime.AddHours(this.cdata[this.curcity].citytimediff + (double) this.cdata[this.curcity].DSTEN);
            string DataPath = this.DataPath;
            Main.checkDST(cdata, localtime, DataPath);
          }
          if (flag)
          {
            this.Cursor = Cursors.AppStarting;
            if (this.func_getutctime(5, true))
              this.updateweather(true);
            this.changeformat();
            this.displaytime();
            this.displayinfo();
            if (this.updatedevice == 0)
              this.updatedevice = 3;
          }
          else
          {
            this.changeformat();
            this.displaytime();
            this.displayinfo();
          }
        }
      }
    }

    private bool pluginweather(string weather, int citynum, bool getNextUpdate)
    {
      if (weather.Length == 0)
        return false;
      weathernode[] weathernodeArray = new weathernode[10];
      int index1 = 0;
      int index2 = 0;
      string[,] strArray = new string[10, 13];
      string str1 = "";
      string str2 = "";
      for (int index3 = 0; index3 < weathernodeArray.GetLength(0); ++index3)
        weathernodeArray[index3] = new weathernode();
      for (int index3 = 0; index3 < 9 && index3 < weather.Length; ++index3)
        str2 += (string) (object) weather[index3];
      if (str2 != "forecast:")
        return false;
      for (int index3 = 10; index3 < 19; ++index3)
        str1 += (string) (object) weather[index3];
      for (; index1 < 10; ++index1)
      {
        byte[] numArray = new byte[25];
        while ((int) weather[index2] != 35)
          ++index2;
        int index3 = index2 + 1;
        string str3 = "";
        while ((int) weather[index3] != 45)
          str3 += (string) (object) weather[index3++];
        weathernodeArray[index1].year = (int) Convert.ToInt16(str3, 10);
        int index4 = index3 + 1;
        string str4 = "";
        while ((int) weather[index4] != 45)
          str4 += (string) (object) weather[index4++];
        int index5 = index4 + 1;
        weathernodeArray[index1].month = (int) Convert.ToInt16(str4, 10);
        string str5 = "";
        while ((int) weather[index5] != 42)
          str5 += (string) (object) weather[index5++];
        weathernodeArray[index1].day = (int) Convert.ToInt16(str5, 10);
        while ((int) weather[index5] != 42)
          ++index5;
        index2 = index5 + 1;
        for (int index6 = 0; index6 < 13; ++index6)
        {
          strArray[index1, index6] = "";
          while ((int) weather[index2] != 42 && (int) weather[index2] != 35 && (int) weather[index2] != 64)
            strArray[index1, index6] = strArray[index1, index6] + (object) weather[index2++];
          if ((int) weather[index2] != 35 && (int) weather[index2] != 64)
            ++index2;
        }
        int num1 = (int) (Convert.ToDouble(strArray[index1, 1]) * 10.0) + 65536;
        numArray[1] = (byte) (num1 / 256);
        numArray[2] = (byte) (num1 % 256);
        int num2 = (int) (Convert.ToDouble(strArray[index1, 0]) * 10.0) + 65536;
        numArray[3] = (byte) (num2 / 256);
        numArray[4] = (byte) (num2 % 256);
        int num3 = (int) (Convert.ToDouble(strArray[index1, 2]) * 10.0) + 65536;
        numArray[17] = (byte) (num3 / 256);
        numArray[18] = (byte) (num3 % 256);
        numArray[9] = this.wdtointeger(strArray[index1, 3]);
        int num4 = (int) (Convert.ToDouble(strArray[index1, 4]) * 10.0) + 65536;
        numArray[21] = (byte) (num4 / 256);
        numArray[22] = (byte) (num4 % 256);
        int num5 = (int) (Convert.ToDouble(strArray[index1, 5]) * 10.0) + 65536;
        numArray[19] = (byte) (num5 / 256);
        numArray[20] = (byte) (num5 % 256);
        int num6 = (int) (Convert.ToDouble(strArray[index1, 6]) * 10.0) + 65536;
        numArray[7] = (byte) (num6 / 256);
        numArray[8] = (byte) (num6 % 256);
        numArray[0] = this.icontobyte(strArray[index1, 7]);
        numArray[6] = Convert.ToByte(strArray[index1, 8], 10);
        int num7 = (int) (Convert.ToDouble(strArray[index1, 9]) * 10.0) + 65536;
        numArray[11] = (byte) (num7 / 256);
        numArray[12] = (byte) (num7 % 256);
        numArray[24] = Convert.ToByte(strArray[index1, 10], 10);
        numArray[23] = Convert.ToByte(strArray[index1, 11], 10);
        numArray[5] = Convert.ToByte(strArray[index1, 12], 10);
        for (int index6 = 0; index6 < 25; ++index6)
          weathernodeArray[index1].forecastbytes[index6] = numArray[index6];
      }
      int index7 = 0;
      string str6 = "";
      while ((int) weather[index2] != 117)
        ++index2;
      if (str6 + (object) weather[index2] + (string) (object) weather[index2 + 1] + (string) (object) weather[index2 + 2] + (string) (object) weather[index2 + 3] != "uvi:")
        return false;
      int num8 = index2 + 5;
      string str7 = "";
      for (int index3 = num8; index3 < num8 + 9; ++index3)
        str7 += (string) (object) weather[index3];
      DateTime dateTime1;
      TimeSpan timeSpan;
      while (index7 < 10)
      {
        int index3 = num8;
        if ((int) weather[index3] != 64)
        {
          while ((int) weather[index3] != 35)
            ++index3;
          int index4 = index3 + 1;
          string str3 = "";
          while ((int) weather[index4] != 45)
            str3 += (string) (object) weather[index4++];
          int index5 = index4 + 1;
          string str4 = "";
          while ((int) weather[index5] != 45)
            str4 += (string) (object) weather[index5++];
          int index6 = index5 + 1;
          string str5 = "";
          while ((int) weather[index6] != 42)
            str5 += (string) (object) weather[index6++];
          DateTime dateTime2 = new DateTime((int) Convert.ToInt16(str3, 10), (int) Convert.ToInt16(str4, 10), (int) Convert.ToInt16(str5));
          dateTime1 = new DateTime(weathernodeArray[index7].year, weathernodeArray[index7].month, weathernodeArray[index7].day);
          timeSpan = dateTime1 - dateTime2;
          if (timeSpan.Days > 0)
          {
            while ((int) weather[index6] != 35 && (int) weather[index6] != 64)
              ++index6;
            num8 = index6;
          }
          else if (timeSpan.Days < 0)
          {
            weathernodeArray[index7++].forecastbytes[10] = byte.MaxValue;
          }
          else
          {
            while ((int) weather[index6] != 42)
              ++index6;
            int index8 = index6 + 1;
            string str8 = "";
            while ((int) weather[index8] != 42 && (int) weather[index8] != 35 && (int) weather[index8] != 64)
              str8 += (string) (object) weather[index8++];
            weathernodeArray[index7].forecastbytes[10] = (byte) this.Round45(Convert.ToDouble(str8));
            ++index7;
            num8 = index8;
          }
        }
        else
          break;
      }
      while (index7 < 10)
        weathernodeArray[index7++].forecastbytes[10] = byte.MaxValue;
      int index9 = num8 + 1 + 1;
      int index10 = 0;
      if ("" + (object) weather[index9] + (string) (object) weather[index9 + 1] + (string) (object) weather[index9 + 2] + (string) (object) weather[index9 + 3] != "sun:")
        return false;
      int index11 = index9 + 5;
      string str9 = "";
      for (int index3 = index11; index3 < index11 + 9; ++index3)
        str9 += (string) (object) weather[index3];
      while (index10 < 10)
      {
        int index3 = index11;
        if ((int) weather[index3] != 64)
        {
          while ((int) weather[index3] != 35)
            ++index3;
          int index4 = index3 + 1;
          string str3 = "";
          while ((int) weather[index4] != 45)
            str3 += (string) (object) weather[index4++];
          int index5 = index4 + 1;
          string str4 = "";
          while ((int) weather[index5] != 45)
            str4 += (string) (object) weather[index5++];
          int index6 = index5 + 1;
          string str5 = "";
          while ((int) weather[index6] != 42)
            str5 += (string) (object) weather[index6++];
          DateTime dateTime2 = new DateTime((int) Convert.ToInt16(str3, 10), (int) Convert.ToInt16(str4, 10), (int) Convert.ToInt16(str5));
          dateTime1 = new DateTime(weathernodeArray[index10].year, weathernodeArray[index10].month, weathernodeArray[index10].day);
          timeSpan = dateTime1 - dateTime2;
          if (timeSpan.TotalDays > 0.0)
          {
            while ((int) weather[index6] != 35 && (int) weather[index6] != 64)
              ++index6;
            index11 = index6;
          }
          else if (timeSpan.TotalDays < 0.0)
          {
            weathernodeArray[index10].forecastbytes[13] = byte.MaxValue;
            weathernodeArray[index10].forecastbytes[14] = byte.MaxValue;
            weathernodeArray[index10].forecastbytes[15] = byte.MaxValue;
            weathernodeArray[index10].forecastbytes[16] = byte.MaxValue;
            ++index10;
          }
          else
          {
            while ((int) weather[index6] != 42)
              ++index6;
            int index8 = index6 + 1;
            string timestr1 = "";
            while ((int) weather[index8] != 42 && (int) weather[index8] != 35 && (int) weather[index8] != 64)
              timestr1 += (string) (object) weather[index8++];
            int index12 = index8 + 1;
            string timestr2 = "";
            while ((int) weather[index12] != 42 && (int) weather[index12] != 35 && (int) weather[index12] != 64)
              timestr2 += (string) (object) weather[index12++];
            int num1 = this.timetoint(timestr1);
            int num2 = num1 / 100;
            if (num1 == -1)
            {
              weathernodeArray[index10].forecastbytes[13] = byte.MaxValue;
              weathernodeArray[index10].forecastbytes[14] = byte.MaxValue;
            }
            else
            {
              weathernodeArray[index10].forecastbytes[13] = (byte) num2;
              int num3 = num1 % 100;
              weathernodeArray[index10].forecastbytes[14] = (byte) num3;
            }
            int num4 = this.timetoint(timestr2);
            int num5 = num4 / 100;
            if (num4 == -1)
            {
              weathernodeArray[index10].forecastbytes[15] = byte.MaxValue;
              weathernodeArray[index10].forecastbytes[16] = byte.MaxValue;
            }
            else
            {
              weathernodeArray[index10].forecastbytes[15] = (byte) num5;
              int num3 = num4 % 100;
              weathernodeArray[index10].forecastbytes[16] = (byte) num3;
            }
            ++index10;
            index11 = index12;
          }
        }
        else
          break;
      }
      for (; index10 < 10; ++index10)
      {
        weathernodeArray[index10].forecastbytes[13] = byte.MaxValue;
        weathernodeArray[index10].forecastbytes[14] = byte.MaxValue;
        weathernodeArray[index10].forecastbytes[15] = byte.MaxValue;
        weathernodeArray[index10].forecastbytes[16] = byte.MaxValue;
      }
      while ((int) weather[index11] != 64)
        ++index11;
      int index13 = index11 + 1;
      while ((int) weather[index13] != 10)
        ++index13;
      int index14 = index13 + 1;
      string str10 = "";
      while ((int) weather[index14] != 58)
        str10 += (string) (object) weather[index14++];
      int index15 = index14 + 1;
      if (str10 != "nextupdate")
        return false;
      string str11 = "";
      while (index15 < weather.Length && (int) weather[index15] != 0)
        str11 += (string) (object) weather[index15++];
      if (getNextUpdate)
      {
        byte[] data = new byte[16];
        this.Gen.GetBytes(data);
        this.Gen.GetBytes(data);
        this.NextUpdateTime = DateTime.UtcNow.Add(this.pc_utc_timediff).AddHours((double) (3 + (Convert.ToInt32(data[0]) + Convert.ToInt32(data[1]) * 16 + Convert.ToInt32(data[2]) * 16 + Convert.ToInt32(data[3]) * 16) % 3)).AddMinutes((double) ((Convert.ToInt32(data[4]) + Convert.ToInt32(data[5]) * 16 + Convert.ToInt32(data[6]) * 16 + Convert.ToInt32(data[7]) * 16) % 60)).AddSeconds((double) ((Convert.ToInt32(data[8]) + Convert.ToInt32(data[9]) * 16 + Convert.ToInt32(data[10]) * 16 + Convert.ToInt32(data[11]) * 16) % 60));
      }
      for (int index3 = 0; index3 < 10; ++index3)
      {
        this.cdata[citynum].weatherbank[index3].year = weathernodeArray[index3].year;
        this.cdata[citynum].weatherbank[index3].month = weathernodeArray[index3].month;
        this.cdata[citynum].weatherbank[index3].day = weathernodeArray[index3].day;
        for (index15 = 0; index15 < 25; ++index15)
          this.cdata[citynum].weatherbank[index3].forecastbytes[index15] = weathernodeArray[index3].forecastbytes[index15];
      }
      string str12 = "";
      while (index15 < weather.Length && (int) weather[index15] != 58)
        str12 += (string) (object) weather[index15++];
      int num9 = index15 + 1;
      return str12 != "pollen" ? true : true;
    }

    private void notify_mousedoubleclick(object sender, MouseEventArgs e)
    {
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    public void checkDSTall(DateTime localtime)
    {
      for (int index = 0; index < 5; ++index)
        Main.checkDST(ref this.cdata[index], localtime, this.DataPath);
    }

    public static void checkDST(ref citydata cdata, DateTime localtime, string DataPath)
    {
      cdata.DSTvalid = true;
      FileStream fileStream;
      if (cdata.cityDSTmode == "X")
      {
        cdata.DSTEN = (byte) 0;
        cdata.DSTvalid = false;
        fileStream = (FileStream) null;
      }
      else if (cdata.cityDSTmode == "" || cdata.cityDSTmode == null)
      {
        cdata.DSTEN = (byte) 0;
        fileStream = new FileStream(DataPath + "db\\DSTUS.txt", FileMode.OpenOrCreate);
      }
      else
        fileStream = new FileStream(DataPath + "db\\" + cdata.cityDSTmode + ".txt", FileMode.OpenOrCreate);
      if (fileStream == null)
        return;
      byte[] buffer = new byte[1000000];
      fileStream.Read(buffer, 0, 1000000);
      fileStream.Close();
      int index1 = 0;
      string str1 = "";
      while ((int) buffer[index1] != 0)
        str1 += (string) (object) (char) buffer[index1++];
      int index2 = 0;
      int index3;
      int year1;
      while (true)
      {
        string str2 = "";
        while ((int) str1[index2] != 9)
        {
          if ((int) str1[index2] != 32)
            str2 += (string) (object) str1[index2++];
          else
            ++index2;
        }
        index3 = index2 + 1;
        year1 = (int) Convert.ToInt16(str2, 10);
        if (localtime.Year != year1)
        {
          while ((int) str1[index3] != 10)
            ++index3;
          index2 = index3 + 1;
        }
        else
          break;
      }
      int num;
      if (cdata.cityDSTmode == "DSTAU3" && localtime.Year > 2008)
      {
        if (localtime.Year == 2009)
        {
          while ((int) str1[index3] == 9)
            ++index3;
          string str2 = "";
          while ((int) str1[index3] != 9)
          {
            if ((int) str1[index3] != 32)
              str2 += (string) (object) str1[index3++];
            else
              ++index3;
          }
          int index4 = index3 + 1;
          int month = (int) Convert.ToInt16(str2, 10);
          string str3 = "";
          while ((int) str1[index4] != 9)
          {
            if ((int) str1[index4] != 32)
              str3 += (string) (object) str1[index4++];
            else
              ++index4;
          }
          int index5 = index4 + 1;
          int day = (int) Convert.ToInt16(str3, 10);
          string str4 = "";
          while ((int) str1[index5] != 58)
          {
            if ((int) str1[index5] != 32)
              str4 += (string) (object) str1[index5++];
            else
              ++index5;
          }
          int index6 = index5 + 1;
          int hour = (int) Convert.ToInt16(str4, 10);
          string str5 = "";
          while ((int) str1[index6] != 9 && (int) str1[index6] != 13 && (int) str1[index6] != 10 && (int) str1[index6] != 0)
          {
            if ((int) str1[index6] != 32)
              str5 += (string) (object) str1[index6++];
            else
              ++index6;
          }
          num = index6 + 1;
          int minute = (int) Convert.ToInt16(str5, 10);
          DateTime dateTime = new DateTime(year1, month, day, hour, minute, 0);
          cdata.DSTEN = (byte) 1;
          if ((localtime - dateTime).TotalDays >= 0.0)
          {
            cdata.DSTEN = (byte) 0;
            cdata.DSTvalid = false;
          }
          else
            cdata.NextDSTtime = dateTime;
        }
        else
          cdata.DSTvalid = false;
      }
      else
      {
        DateTime[] dateTimeArray = new DateTime[3];
        int index4 = 0;
        while ((int) str1[index3] != 10)
        {
          string str2 = "";
          while ((int) str1[index3] != 9)
          {
            if ((int) str1[index3] != 32)
              str2 += (string) (object) str1[index3++];
            else
              ++index3;
          }
          int index5 = index3 + 1;
          int month = (int) Convert.ToInt16(str2, 10);
          string str3 = "";
          while ((int) str1[index5] != 9)
          {
            if ((int) str1[index5] != 32)
              str3 += (string) (object) str1[index5++];
            else
              ++index5;
          }
          int index6 = index5 + 1;
          int day = (int) Convert.ToInt16(str3, 10);
          string str4 = "";
          while ((int) str1[index6] != 58)
          {
            if ((int) str1[index6] != 32)
              str4 += (string) (object) str1[index6++];
            else
              ++index6;
          }
          int index7 = index6 + 1;
          int hour = (int) Convert.ToInt16(str4, 10);
          string str5 = "";
          while ((int) str1[index7] != 9 && (int) str1[index7] != 13 && (int) str1[index7] != 10 && (int) str1[index7] != 0)
          {
            if ((int) str1[index7] != 32)
              str5 += (string) (object) str1[index7++];
            else
              ++index7;
          }
          index3 = index7 + 1;
          int minute = (int) Convert.ToInt16(str5, 10);
          dateTimeArray[index4] = new DateTime(year1, month, day, hour, minute, 0);
          ++index4;
          if (index4 == 2)
          {
            while ((int) str1[index3] != 10)
              ++index3;
          }
        }
        int index8 = index3 + 1;
        string str6 = "";
        while ((int) str1[index8] != 9)
        {
          if ((int) str1[index8] != 32)
            str6 += (string) (object) str1[index8++];
          else
            ++index8;
        }
        int index9 = index8 + 1;
        int year2 = (int) Convert.ToInt16(str6, 10);
        if (cdata.cur_con == 0 && cdata.cur_country == 0 && (!(cdata.cityDSTmode == "DSTAU3") || year2 != 2008))
        {
          for (int index5 = 0; index5 < 3; ++index5)
          {
            while ((int) str1[index9] != 9)
              ++index9;
            ++index9;
            while ((int) str1[index9] == 9)
              ++index9;
          }
        }
        string str7 = "";
        while ((int) str1[index9] != 9)
        {
          if ((int) str1[index9] != 32)
            str7 += (string) (object) str1[index9++];
          else
            ++index9;
        }
        int index10 = index9 + 1;
        int month1 = (int) Convert.ToInt16(str7, 10);
        string str8 = "";
        while ((int) str1[index10] != 9)
        {
          if ((int) str1[index10] != 32)
            str8 += (string) (object) str1[index10++];
          else
            ++index10;
        }
        int index11 = index10 + 1;
        int day1 = (int) Convert.ToInt16(str8, 10);
        string str9 = "";
        while ((int) str1[index11] != 58)
        {
          if ((int) str1[index11] != 32)
            str9 += (string) (object) str1[index11++];
          else
            ++index11;
        }
        int index12 = index11 + 1;
        int hour1 = (int) Convert.ToInt16(str9, 10);
        string str10 = "";
        while ((int) str1[index12] != 9 && (int) str1[index12] != 13 && (int) str1[index12] != 10 && (int) str1[index12] != 0)
        {
          if ((int) str1[index12] != 32)
            str10 += (string) (object) str1[index12++];
          else
            ++index12;
        }
        num = index12 + 1;
        int minute1 = (int) Convert.ToInt16(str10, 10);
        dateTimeArray[index4] = new DateTime(year2, month1, day1, hour1, minute1, 0);
        if (cdata.cur_con == 0 && cdata.cur_country == 0)
        {
          cdata.DSTEN = (byte) 1;
          TimeSpan timeSpan = localtime - dateTimeArray[1];
          if (timeSpan.TotalDays >= 0.0)
          {
            cdata.DSTEN = (byte) 0;
            timeSpan = localtime - dateTimeArray[0];
            if (timeSpan.TotalDays >= 0.0)
            {
              cdata.DSTEN = (byte) 1;
              cdata.NextDSTtime = dateTimeArray[2];
            }
            else
            {
              cdata.DSTEN = (byte) 0;
              cdata.NextDSTtime = dateTimeArray[0];
            }
          }
          else
            cdata.NextDSTtime = dateTimeArray[1];
        }
        else
        {
          cdata.DSTEN = (byte) 0;
          TimeSpan timeSpan = localtime - dateTimeArray[0];
          if (timeSpan.TotalDays >= 0.0)
          {
            cdata.DSTEN = (byte) 1;
            timeSpan = localtime - dateTimeArray[1];
            if (timeSpan.TotalDays >= 0.0)
            {
              cdata.DSTEN = (byte) 0;
              cdata.NextDSTtime = dateTimeArray[2];
            }
            else
            {
              cdata.DSTEN = (byte) 1;
              cdata.NextDSTtime = dateTimeArray[1];
            }
          }
          else
            cdata.NextDSTtime = dateTimeArray[0];
        }
      }
    }

    private void updatebutton_Click(object sender, EventArgs e)
    {
      if (this.updatedevice != 0)
        return;
      this.Cursor = Cursors.AppStarting;
      if (!this.func_getutctime(5, true))
        return;
      this.updateweather(true);
      this.displaytime();
      this.displayinfo();
      if (this.updatedevice == 0)
        this.updatedevice = 2;
    }

    private void updaterecbutton_Click(object sender, EventArgs e)
    {
      if (this.updatedevice != 0 || this.updatedevice != 0)
        return;
      this.updatedevice = 2;
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
      this.timer2.Stop();
      if (this.usbobj.IsConnected)
      {
        if (this.updatedevice != 0)
          this.Cursor = Cursors.AppStarting;
        if (this.updatedevice == 1)
        {
          this.syncclock(60, true);
          this.Cursor = Cursors.Default;
        }
        else if (this.updatedevice == 2)
        {
          this.syncclock(6, true);
          this.Cursor = Cursors.Default;
        }
        else if (this.updatedevice == 3)
        {
          this.syncclock(6, false);
          this.Cursor = Cursors.Default;
        }
        this.updatedevice = 0;
        Thread thread1 = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
        thread1.Start((object) usbjob.usb_lock);
        while (thread1.IsAlive && this.usbobj.IsConnected)
        {
          Application.DoEvents();
          Thread.Sleep(10);
        }
        if (this.usbobj.locksuccess)
        {
          if (this.usbobj.gotinterrupt && this.usbobj.interruptcommand == 81)
            this.updatedevice = 2;
          this.usbobj.gotinterrupt = false;
          Thread thread2 = new Thread(new ParameterizedThreadStart(this.usbobj.dofunc));
          thread2.Start((object) usbjob.Usbcheck);
          while (thread2.IsAlive && this.usbobj.IsConnected)
          {
            Application.DoEvents();
            Thread.Sleep(10);
          }
          this.usbobj.getsuccess();
        }
      }
      this.timer2.Interval = 500;
      this.timer2.Start();
    }
  }
}
