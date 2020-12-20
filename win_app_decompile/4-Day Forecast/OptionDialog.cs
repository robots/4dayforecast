// Decompiled with JetBrains decompiler
// Type: Forecast.OptionDialog
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using Forecast.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace Forecast
{
  public class OptionDialog : Form
  {
    private IContainer components = (IContainer) null;
    private DateTimeFormatInfo timeformat = new DateTimeFormatInfo();
    private Label lb_title;
    private Label lb_cityname;
    private Label lb_citytime;
    private Label lb_citydate;
    private Label label1;
    private Label lb_timeoffset;
    private Label lb_temperature;
    private Label lb_windspeed;
    private Label lb_timeformat;
    private Label lb_calendarformat;
    private ComboBox cb_timeoffset;
    private ComboBox cb_temp;
    private ComboBox cb_wind;
    private ComboBox cb_time;
    private ComboBox cb_cal;
    private Label lb_setloc;
    private Label lb_save;
    private Label lb_cancel;
    private Timer timer1;
    private Bitmap renderBmp;
    private Point mouse_offset;
    public bool _time12hr;
    public bool _daymonth;
    public bool _CEL;
    public int _windspeedfor;
    public string _DataPath;
    public citydata _cdata;
    public TimeSpan _pc_utc_timediff;
    private DateTime last_displaytime;

    public bool time12hr
    {
      get
      {
        return this._time12hr;
      }
      set
      {
        this._time12hr = value;
      }
    }

    public bool daymonth
    {
      get
      {
        return this._daymonth;
      }
      set
      {
        this._daymonth = value;
      }
    }

    public bool CEL
    {
      get
      {
        return this._CEL;
      }
      set
      {
        this._CEL = value;
      }
    }

    public int windspeedfor
    {
      get
      {
        return this._windspeedfor;
      }
      set
      {
        this._windspeedfor = value;
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

    public citydata cdata
    {
      get
      {
        return this._cdata;
      }
      set
      {
        this._cdata = value;
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
        TextureBrush textureBrush = new TextureBrush(image, new Rectangle(0, 0, this.Width, this.Height));
        Graphics graphics = Graphics.FromImage((Image) this.renderBmp);
        Color color = Color.FromArgb((int) byte.MaxValue, 50, 50, 50);
        this.TransparencyKey = color;
        Brush brush = (Brush) new SolidBrush(color);
        graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
        graphics.FillPie((Brush) textureBrush, 0, 0, 40, 40, 180, 90);
        graphics.FillPie((Brush) textureBrush, this.Width - 40, 0, 40, 40, 270, 90);
        graphics.FillPie((Brush) textureBrush, 0, this.Height - 40, 40, 40, 90, 90);
        graphics.FillPie((Brush) textureBrush, this.Width - 40, this.Height - 40, 40, 40, 0, 90);
        graphics.FillRectangle((Brush) textureBrush, 20, 0, this.Width - 40, this.Height);
        graphics.FillRectangle((Brush) textureBrush, 0, 20, 20, this.Height - 40);
        graphics.FillRectangle((Brush) textureBrush, this.Width - 20, 20, 20, this.Height - 40);
        graphics.Dispose();
        base.BackgroundImage = (Image) this.renderBmp;
      }
    }

    public OptionDialog()
    {
      this.InitializeComponent();
      this.TransparencyKey = Color.FromArgb((int) byte.MaxValue, 50, 50, 50);
      this.cb_temp.Items.Clear();
      this.cb_temp.Items.Add((object) "°C");
      this.cb_temp.Items.Add((object) "°F");
      this.cb_wind.Items.Clear();
      this.cb_wind.Items.Add((object) "km/h");
      this.cb_wind.Items.Add((object) "m/s");
      this.cb_wind.Items.Add((object) "knots");
      this.cb_wind.Items.Add((object) "mph");
      this.cb_time.Items.Clear();
      this.cb_time.Items.Add((object) "24 hour");
      this.cb_time.Items.Add((object) "12 hour");
      this.cb_cal.Items.Clear();
      this.cb_cal.Items.Add((object) "Day/Month");
      this.cb_cal.Items.Add((object) "Month/Day");
      this.cb_timeoffset.Items.Clear();
      this.cb_timeoffset.Items.Add((object) "0 Hour");
      this.cb_timeoffset.Items.Add((object) "+0.5 Hour");
      this.cb_timeoffset.Items.Add((object) "+1 Hour");
      this.cb_timeoffset.Items.Add((object) "+1.5 Hours");
      this.cb_timeoffset.Items.Add((object) "+2 Hours");
      this.cb_timeoffset.Items.Add((object) "-0.5 Hour");
      this.cb_timeoffset.Items.Add((object) "-1 Hour");
      this.cb_timeoffset.Items.Add((object) "-1.5 Hours");
      this.cb_timeoffset.Items.Add((object) "-2 Hours");
      this.BackgroundImage = (Image) Resources.option;
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
      this.lb_title = new Label();
      this.lb_cityname = new Label();
      this.lb_citytime = new Label();
      this.lb_citydate = new Label();
      this.label1 = new Label();
      this.lb_timeoffset = new Label();
      this.lb_temperature = new Label();
      this.lb_windspeed = new Label();
      this.lb_timeformat = new Label();
      this.lb_calendarformat = new Label();
      this.cb_timeoffset = new ComboBox();
      this.cb_temp = new ComboBox();
      this.cb_wind = new ComboBox();
      this.cb_time = new ComboBox();
      this.cb_cal = new ComboBox();
      this.lb_setloc = new Label();
      this.lb_save = new Label();
      this.lb_cancel = new Label();
      this.timer1 = new Timer(this.components);
      this.SuspendLayout();
      this.lb_title.BackColor = Color.Transparent;
      this.lb_title.Font = new Font("Arial", 14.25f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.lb_title.ForeColor = Color.White;
      this.lb_title.Location = new Point(8, 8);
      this.lb_title.Name = "lb_title";
      this.lb_title.Size = new Size(176, 40);
      this.lb_title.TabIndex = 0;
      this.lb_title.Text = "HOME LOCATION";
      this.lb_title.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_title.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_title.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_cityname.BackColor = Color.Transparent;
      this.lb_cityname.Font = new Font("Arial", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_cityname.ForeColor = Color.White;
      this.lb_cityname.Location = new Point(8, 48);
      this.lb_cityname.Name = "lb_cityname";
      this.lb_cityname.Size = new Size(296, 88);
      this.lb_cityname.TabIndex = 1;
      this.lb_cityname.Text = "cityname";
      this.lb_cityname.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_cityname.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_cityname.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_citytime.BackColor = Color.Transparent;
      this.lb_citytime.Font = new Font("Arial", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_citytime.ForeColor = Color.White;
      this.lb_citytime.Location = new Point(8, 136);
      this.lb_citytime.Name = "lb_citytime";
      this.lb_citytime.Size = new Size(296, 20);
      this.lb_citytime.TabIndex = 2;
      this.lb_citytime.Text = "citytime";
      this.lb_citytime.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_citytime.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_citytime.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_citydate.BackColor = Color.Transparent;
      this.lb_citydate.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_citydate.ForeColor = Color.White;
      this.lb_citydate.Location = new Point(8, 160);
      this.lb_citydate.Name = "lb_citydate";
      this.lb_citydate.Size = new Size(296, 20);
      this.lb_citydate.TabIndex = 3;
      this.lb_citydate.Text = "citydate";
      this.lb_citydate.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_citydate.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_citydate.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(223, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(169, 24);
      this.label1.TabIndex = 4;
      this.label1.Text = "Version 1.04";
      this.label1.TextAlign = ContentAlignment.TopRight;
      this.label1.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.label1.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_timeoffset.BackColor = Color.Transparent;
      this.lb_timeoffset.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_timeoffset.ForeColor = Color.White;
      this.lb_timeoffset.Location = new Point(8, 188);
      this.lb_timeoffset.Name = "lb_timeoffset";
      this.lb_timeoffset.Size = new Size(80, 24);
      this.lb_timeoffset.TabIndex = 5;
      this.lb_timeoffset.Text = "Time offset";
      this.lb_timeoffset.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_timeoffset.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_timeoffset.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_temperature.BackColor = Color.Transparent;
      this.lb_temperature.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_temperature.ForeColor = Color.White;
      this.lb_temperature.Location = new Point(8, 224);
      this.lb_temperature.Name = "lb_temperature";
      this.lb_temperature.Size = new Size(80, 24);
      this.lb_temperature.TabIndex = 6;
      this.lb_temperature.Text = "Temperature";
      this.lb_temperature.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_temperature.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_temperature.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_windspeed.BackColor = Color.Transparent;
      this.lb_windspeed.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_windspeed.ForeColor = Color.White;
      this.lb_windspeed.Location = new Point(8, 256);
      this.lb_windspeed.Name = "lb_windspeed";
      this.lb_windspeed.Size = new Size(80, 24);
      this.lb_windspeed.TabIndex = 7;
      this.lb_windspeed.Text = "Wind speed";
      this.lb_windspeed.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_windspeed.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_windspeed.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_timeformat.BackColor = Color.Transparent;
      this.lb_timeformat.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_timeformat.ForeColor = Color.White;
      this.lb_timeformat.Location = new Point(8, 288);
      this.lb_timeformat.Name = "lb_timeformat";
      this.lb_timeformat.Size = new Size(80, 24);
      this.lb_timeformat.TabIndex = 8;
      this.lb_timeformat.Text = "Time format";
      this.lb_timeformat.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_timeformat.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_timeformat.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.lb_calendarformat.BackColor = Color.Transparent;
      this.lb_calendarformat.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_calendarformat.ForeColor = Color.White;
      this.lb_calendarformat.Location = new Point(8, 320);
      this.lb_calendarformat.Name = "lb_calendarformat";
      this.lb_calendarformat.Size = new Size(96, 24);
      this.lb_calendarformat.TabIndex = 9;
      this.lb_calendarformat.Text = "Calendar format";
      this.lb_calendarformat.TextAlign = ContentAlignment.MiddleLeft;
      this.lb_calendarformat.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.lb_calendarformat.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.cb_timeoffset.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cb_timeoffset.BackColor = Color.Black;
      this.cb_timeoffset.FlatStyle = FlatStyle.Flat;
      this.cb_timeoffset.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cb_timeoffset.ForeColor = Color.White;
      this.cb_timeoffset.FormattingEnabled = true;
      this.cb_timeoffset.Location = new Point(248, 192);
      this.cb_timeoffset.MaxDropDownItems = 9;
      this.cb_timeoffset.Name = "cb_timeoffset";
      this.cb_timeoffset.Size = new Size(144, 22);
      this.cb_timeoffset.TabIndex = 3;
      this.cb_timeoffset.SelectedIndexChanged += new EventHandler(this.cb_timeoffset_SelectedIndexChanged);
      this.cb_timeoffset.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_timeoffset.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_temp.BackColor = Color.Black;
      this.cb_temp.FlatStyle = FlatStyle.Flat;
      this.cb_temp.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cb_temp.ForeColor = Color.White;
      this.cb_temp.FormattingEnabled = true;
      this.cb_temp.Location = new Point(248, 224);
      this.cb_temp.MaxDropDownItems = 9;
      this.cb_temp.Name = "cb_temp";
      this.cb_temp.Size = new Size(144, 22);
      this.cb_temp.TabIndex = 4;
      this.cb_temp.SelectedIndexChanged += new EventHandler(this.cb_temp_SelectedIndexChanged);
      this.cb_temp.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_temp.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_wind.BackColor = Color.Black;
      this.cb_wind.FlatStyle = FlatStyle.Flat;
      this.cb_wind.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cb_wind.ForeColor = Color.White;
      this.cb_wind.FormattingEnabled = true;
      this.cb_wind.Location = new Point(248, 256);
      this.cb_wind.MaxDropDownItems = 9;
      this.cb_wind.Name = "cb_wind";
      this.cb_wind.Size = new Size(144, 22);
      this.cb_wind.TabIndex = 5;
      this.cb_wind.SelectedIndexChanged += new EventHandler(this.cb_wind_SelectedIndexChanged);
      this.cb_wind.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_wind.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_time.BackColor = Color.Black;
      this.cb_time.FlatStyle = FlatStyle.Flat;
      this.cb_time.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cb_time.ForeColor = Color.White;
      this.cb_time.FormattingEnabled = true;
      this.cb_time.Location = new Point(248, 288);
      this.cb_time.MaxDropDownItems = 9;
      this.cb_time.Name = "cb_time";
      this.cb_time.Size = new Size(144, 22);
      this.cb_time.TabIndex = 6;
      this.cb_time.SelectedIndexChanged += new EventHandler(this.cb_time_SelectedIndexChanged);
      this.cb_time.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_time.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_cal.BackColor = Color.Black;
      this.cb_cal.FlatStyle = FlatStyle.Flat;
      this.cb_cal.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.cb_cal.ForeColor = Color.White;
      this.cb_cal.FormattingEnabled = true;
      this.cb_cal.Location = new Point(248, 320);
      this.cb_cal.MaxDropDownItems = 9;
      this.cb_cal.Name = "cb_cal";
      this.cb_cal.Size = new Size(144, 22);
      this.cb_cal.TabIndex = 7;
      this.cb_cal.SelectedIndexChanged += new EventHandler(this.cb_cal_SelectedIndexChanged);
      this.cb_cal.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_cal.DropDown += new EventHandler(this.cb_dropdown);
      this.lb_setloc.BackColor = Color.Transparent;
      this.lb_setloc.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_setloc.ForeColor = Color.DarkGray;
      this.lb_setloc.Image = (Image) Resources.button21;
      this.lb_setloc.Location = new Point(304, 48);
      this.lb_setloc.Name = "lb_setloc";
      this.lb_setloc.Size = new Size(84, 40);
      this.lb_setloc.TabIndex = 2;
      this.lb_setloc.Text = "SET LOCATION...";
      this.lb_setloc.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_setloc.MouseLeave += new EventHandler(this.button_mouseleave);
      this.lb_setloc.Click += new EventHandler(this.lb_setloc_Click);
      this.lb_setloc.MouseEnter += new EventHandler(this.button_mouseenter);
      this.lb_save.BackColor = Color.Transparent;
      this.lb_save.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_save.ForeColor = Color.DarkGray;
      this.lb_save.Image = (Image) Resources.button21;
      this.lb_save.Location = new Point(200, 352);
      this.lb_save.Name = "lb_save";
      this.lb_save.Size = new Size(84, 40);
      this.lb_save.TabIndex = 8;
      this.lb_save.Text = "SAVE";
      this.lb_save.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_save.MouseLeave += new EventHandler(this.button_mouseleave);
      this.lb_save.Click += new EventHandler(this.lb_save_Click);
      this.lb_save.MouseEnter += new EventHandler(this.button_mouseenter);
      this.lb_cancel.BackColor = Color.Transparent;
      this.lb_cancel.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lb_cancel.ForeColor = Color.DarkGray;
      this.lb_cancel.Image = (Image) Resources.button21;
      this.lb_cancel.Location = new Point(304, 352);
      this.lb_cancel.Name = "lb_cancel";
      this.lb_cancel.Size = new Size(84, 40);
      this.lb_cancel.TabIndex = 1;
      this.lb_cancel.Text = "CANCEL";
      this.lb_cancel.TextAlign = ContentAlignment.MiddleCenter;
      this.lb_cancel.MouseLeave += new EventHandler(this.button_mouseleave);
      this.lb_cancel.Click += new EventHandler(this.lb_cancel_Click);
      this.lb_cancel.MouseEnter += new EventHandler(this.button_mouseenter);
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.BackgroundImage = (Image) Resources.option;
      this.ClientSize = new Size(400, 400);
      this.Controls.Add((Control) this.lb_cancel);
      this.Controls.Add((Control) this.lb_save);
      this.Controls.Add((Control) this.lb_setloc);
      this.Controls.Add((Control) this.cb_cal);
      this.Controls.Add((Control) this.cb_time);
      this.Controls.Add((Control) this.cb_wind);
      this.Controls.Add((Control) this.cb_temp);
      this.Controls.Add((Control) this.cb_timeoffset);
      this.Controls.Add((Control) this.lb_calendarformat);
      this.Controls.Add((Control) this.lb_timeformat);
      this.Controls.Add((Control) this.lb_windspeed);
      this.Controls.Add((Control) this.lb_temperature);
      this.Controls.Add((Control) this.lb_timeoffset);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lb_citydate);
      this.Controls.Add((Control) this.lb_citytime);
      this.Controls.Add((Control) this.lb_cityname);
      this.Controls.Add((Control) this.lb_title);
      this.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.ForeColor = Color.Black;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(4);
      this.Name = "OptionDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "OptionDialog";
      this.Load += new EventHandler(this.OptionDialog_Load);
      this.MouseDown += new MouseEventHandler(this.Main_MouseDown);
      this.MouseMove += new MouseEventHandler(this.Main_MouseMove);
      this.ResumeLayout(false);
    }

    private void cb_cal_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._daymonth = this.cb_cal.SelectedIndex == 0;
      this.changeformat();
      this.displaytime();
    }

    private void displaytime()
    {
      if (this._cdata.cityid == "")
      {
        this.lb_cityname.Text = "";
        this.lb_citytime.Text = "--:--";
        this.lb_citydate.Text = "";
      }
      else
      {
        if (this._cdata.statename == null)
          this.lb_cityname.Text = this._cdata.cityname + ", " + this._cdata.countryname;
        else if (this._cdata.statename.Length == 0)
          this.lb_cityname.Text = this._cdata.cityname + ", " + this._cdata.countryname;
        else
          this.lb_cityname.Text = this._cdata.cityname + ", " + this._cdata.statename + ", " + this._cdata.countryname;
        DateTime dateTime = DateTime.UtcNow;
        dateTime = dateTime.Add(this._pc_utc_timediff);
        this.last_displaytime = dateTime.AddHours(this._cdata.citytimediff + (double) this._cdata.DSTEN + this._cdata.citytimeoffset);
        this.lb_citytime.Text = this.last_displaytime.ToString("t", (IFormatProvider) this.timeformat);
        this.lb_citydate.Text = this.last_displaytime.ToString("d", (IFormatProvider) this.timeformat);
      }
    }

    private void button_mouseenter(object sender, EventArgs e)
    {
      ((Control) sender).ForeColor = Color.White;
    }

    private void button_mouseleave(object sender, EventArgs e)
    {
      ((Control) sender).ForeColor = Color.DarkGray;
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

    private void lb_cancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void cb_keydown(object sender, KeyEventArgs e)
    {
      e.SuppressKeyPress = true;
    }

    private void OptionDialog_Load(object sender, EventArgs e)
    {
      if (this._CEL)
        this.cb_temp.SelectedIndex = 0;
      else
        this.cb_temp.SelectedIndex = 1;
      this.cb_wind.SelectedIndex = this._windspeedfor;
      if (this._time12hr)
        this.cb_time.SelectedIndex = 1;
      else
        this.cb_time.SelectedIndex = 0;
      if (this._daymonth)
        this.cb_cal.SelectedIndex = 0;
      else
        this.cb_cal.SelectedIndex = 1;
      this.setoffset();
      this.changeformat();
      this.displaytime();
      this.timer1.Start();
    }

    private void setoffset()
    {
      switch ((int) (this._cdata.citytimeoffset * 10.0))
      {
        case 10:
          this.cb_timeoffset.SelectedItem = (object) "+1 Hour";
          break;
        case 15:
          this.cb_timeoffset.SelectedItem = (object) "+1.5 Hours";
          break;
        case 20:
          this.cb_timeoffset.SelectedItem = (object) "+2 Hours";
          break;
        case 0:
          this.cb_timeoffset.SelectedItem = (object) "0 Hour";
          break;
        case 5:
          this.cb_timeoffset.SelectedItem = (object) "+0.5 Hour";
          break;
        case -10:
          this.cb_timeoffset.SelectedItem = (object) "-1 Hour";
          break;
        case -5:
          this.cb_timeoffset.SelectedItem = (object) "-0.5 Hour";
          break;
        case -20:
          this.cb_timeoffset.SelectedItem = (object) "-2 Hours";
          break;
        case -15:
          this.cb_timeoffset.SelectedItem = (object) "-1.5 Hours";
          break;
        default:
          this.cb_timeoffset.SelectedItem = (object) "0 Hour";
          break;
      }
    }

    private void changeformat()
    {
      this.timeformat.AMDesignator = "am";
      this.timeformat.PMDesignator = "pm";
      this.timeformat.ShortTimePattern = !this._time12hr ? "H:mm" : "h:mmtt";
      if (this._daymonth)
      {
        this.timeformat.MonthDayPattern = "(d/M)";
        this.timeformat.ShortDatePattern = "d / M / yyyy";
      }
      else
      {
        this.timeformat.MonthDayPattern = "(M/d)";
        this.timeformat.ShortDatePattern = "M / d / yyyy";
      }
    }

    private void cb_time_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._time12hr = this.cb_time.SelectedIndex != 0;
      this.changeformat();
      this.displaytime();
    }

    private void cb_wind_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._windspeedfor = this.cb_wind.SelectedIndex;
    }

    private void cb_temp_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cb_temp.SelectedIndex == 0)
        this._CEL = true;
      else
        this._CEL = false;
    }

    private void cb_timeoffset_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.cb_timeoffset.SelectedIndex)
      {
        case 0:
          this._cdata.citytimeoffset = 0.0;
          break;
        case 1:
          this._cdata.citytimeoffset = 0.5;
          break;
        case 2:
          this._cdata.citytimeoffset = 1.0;
          break;
        case 3:
          this._cdata.citytimeoffset = 1.5;
          break;
        case 4:
          this._cdata.citytimeoffset = 2.0;
          break;
        case 5:
          this._cdata.citytimeoffset = -0.5;
          break;
        case 6:
          this._cdata.citytimeoffset = -1.0;
          break;
        case 7:
          this._cdata.citytimeoffset = -1.5;
          break;
        case 8:
          this._cdata.citytimeoffset = -2.0;
          break;
        default:
          this._cdata.citytimeoffset = 0.0;
          break;
      }
      this.changeformat();
      this.displaytime();
    }

    private void lb_setloc_Click(object sender, EventArgs e)
    {
      SelectCity selectCity = new SelectCity();
      selectCity.cdata = (citydata) this._cdata.Clone();
      selectCity.DataPath = this._DataPath;
      if (selectCity.ShowDialog() == DialogResult.OK)
      {
        this._cdata = (citydata) selectCity.cdata.Clone();
        Main.checkDST(ref this._cdata, DateTime.UtcNow.Add(this._pc_utc_timediff).AddHours(this._cdata.citytimediff), this._DataPath);
        Main.checkDST(ref this._cdata, DateTime.UtcNow.Add(this._pc_utc_timediff).AddHours(this._cdata.citytimediff + (double) this._cdata.DSTEN), this._DataPath);
      }
      this.setoffset();
      this.changeformat();
      this.displaytime();
    }

    private void cb_dropdown(object sender, EventArgs e)
    {
      this.mouse_offset.X = 5000;
      this.mouse_offset.Y = 5000;
    }

    private void lb_save_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      DateTime dateTime1 = DateTime.UtcNow;
      dateTime1 = dateTime1.Add(this.pc_utc_timediff);
      DateTime dateTime2 = dateTime1.AddHours(this._cdata.citytimediff + (double) this._cdata.DSTEN + this._cdata.citytimeoffset);
      if (this.last_displaytime.Minute == dateTime2.Minute && this.last_displaytime.Hour == dateTime2.Hour && !(this.last_displaytime.Date != dateTime2.Date))
        return;
      if (dateTime2.Minute % 15 == 0)
        Main.checkDST(ref this._cdata, DateTime.UtcNow.Add(this._pc_utc_timediff).AddHours(this._cdata.citytimediff + (double) this._cdata.DSTEN), this._DataPath);
      this.displaytime();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      Graphics graphics = e.Graphics;
      Brush brush = (Brush) new SolidBrush(Color.FromArgb((int) byte.MaxValue, 50, 50, 50));
      graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
      TextureBrush textureBrush = new TextureBrush((Image) this.renderBmp, new Rectangle(0, 0, this.Width, this.Height));
      graphics.FillPie((Brush) textureBrush, 0, 0, 40, 40, 180, 90);
      graphics.FillPie((Brush) textureBrush, this.Width - 40, 0, 40, 40, 270, 90);
      graphics.FillPie((Brush) textureBrush, 0, this.Height - 40, 40, 40, 90, 90);
      graphics.FillPie((Brush) textureBrush, this.Width - 40, this.Height - 40, 40, 40, 0, 90);
      graphics.FillRectangle((Brush) textureBrush, 20, 0, this.Width - 40, this.Height);
      graphics.FillRectangle((Brush) textureBrush, 0, 20, 20, this.Height - 40);
      graphics.FillRectangle((Brush) textureBrush, this.Width - 20, 20, 20, this.Height - 40);
      OptionDialog.horizontalRule horizontalRule = new OptionDialog.horizontalRule(e, 8, 216, 384);
      base.OnPaint(e);
    }

    public class horizontalRule : Control
    {
      public horizontalRule(PaintEventArgs e, int x, int y, int width)
      {
        if (width < 0)
          width = 0;
        Graphics graphics = e.Graphics;
        Pen pen1 = new Pen(Color.FromKnownColor(KnownColor.ControlDark), 1f);
        Pen pen2 = new Pen(Color.WhiteSmoke, 1f);
        graphics.DrawLine(pen1, x, y, x + width, y);
        graphics.DrawLine(pen2, x, y + 1, x + width, y + 1);
      }
    }
  }
}
