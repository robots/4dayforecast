// Decompiled with JetBrains decompiler
// Type: Forecast.ErrorBox
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Forecast
{
  public class ErrorBox : Form
  {
    private IContainer components = (IContainer) null;
    private Label lb_message;
    private Button bt_2;
    private Button bt_1;
    private Button bt_3;

    public ErrorBox(int type)
    {
      this.InitializeComponent();
      switch (type)
      {
        case 1:
          this.lb_message.Text = "USB disconnected.\nPlease reconnect and click RETRY.";
          this.bt_1.Text = "RETRY";
          this.bt_2.Visible = false;
          this.bt_3.Text = "EXIT";
          break;
        case 2:
          this.lb_message.Text = "Are you sure you want to exit the program?";
          this.bt_1.Text = "YES";
          this.bt_2.Visible = false;
          this.bt_3.Text = "NO";
          break;
        case 3:
          this.lb_message.Text = "A new version of 4-Day Forecast program is available, would you like to download the file now?";
          this.bt_1.Text = "YES";
          this.bt_2.Visible = false;
          this.bt_3.Text = "NO";
          break;
        case 4:
          this.lb_message.Text = "Cannot connect to server. Please try again later";
          this.bt_1.Visible = false;
          this.bt_2.Text = "OK";
          this.bt_3.Visible = false;
          break;
        case 5:
          this.lb_message.Text = "This program is running on your computer.";
          this.bt_1.Visible = false;
          this.bt_2.Text = "OK";
          this.bt_3.Visible = false;
          break;
        case 6:
          this.lb_message.Text = "Unable to download the EXE file.\nNow try to downalod in Zip file format.";
          this.bt_1.Visible = false;
          this.bt_2.Text = "OK";
          this.bt_3.Visible = false;
          break;
        case 7:
          this.lb_message.Text = "Unable to download the Zip file.\nPlease check your internet settings and download next time.";
          this.bt_1.Visible = false;
          this.bt_2.Text = "OK";
          this.bt_3.Visible = false;
          break;
        case 8:
          this.lb_message.Text = "Loading...";
          this.bt_1.Visible = false;
          this.bt_2.Text = "OK";
          this.bt_3.Visible = false;
          break;
        case 9:
          this.lb_message.Text = "No response from the USB transmitter. Disconnect the transmitter from the USB port, wait 10 seconds and reconnect again. Then Click [RETRY]";
          this.bt_1.Text = "RETRY";
          this.bt_2.Visible = false;
          this.bt_3.Text = "EXIT";
          break;
        default:
          this.lb_message.Text = "Error.";
          this.bt_1.Visible = false;
          this.bt_2.Text = "OK";
          this.bt_3.Visible = false;
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lb_message = new Label();
      this.bt_2 = new Button();
      this.bt_1 = new Button();
      this.bt_3 = new Button();
      this.SuspendLayout();
      this.lb_message.Font = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lb_message.Location = new Point(0, 10);
      this.lb_message.Name = "lb_message";
      this.lb_message.Size = new Size(408, 72);
      this.lb_message.TabIndex = 0;
      this.lb_message.Text = "Message";
      this.lb_message.TextAlign = ContentAlignment.TopCenter;
      this.bt_2.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.bt_2.Location = new Point(168, 96);
      this.bt_2.Margin = new Padding(3, 4, 3, 4);
      this.bt_2.Name = "bt_2";
      this.bt_2.Size = new Size(80, 23);
      this.bt_2.TabIndex = 3;
      this.bt_2.Text = "OK";
      this.bt_2.UseVisualStyleBackColor = true;
      this.bt_2.Click += new EventHandler(this.bt_1_Click);
      this.bt_1.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.bt_1.Location = new Point(104, 96);
      this.bt_1.Margin = new Padding(3, 4, 3, 4);
      this.bt_1.Name = "bt_1";
      this.bt_1.Size = new Size(80, 23);
      this.bt_1.TabIndex = 1;
      this.bt_1.Text = "RETRY";
      this.bt_1.UseVisualStyleBackColor = true;
      this.bt_1.Click += new EventHandler(this.bt_2_Click);
      this.bt_3.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.bt_3.Location = new Point(232, 96);
      this.bt_3.Margin = new Padding(3, 4, 3, 4);
      this.bt_3.Name = "bt_3";
      this.bt_3.Size = new Size(80, 23);
      this.bt_3.TabIndex = 3;
      this.bt_3.Text = "EXIT";
      this.bt_3.UseVisualStyleBackColor = true;
      this.bt_3.Click += new EventHandler(this.bt_3_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(410, 128);
      this.Controls.Add((Control) this.bt_2);
      this.Controls.Add((Control) this.bt_3);
      this.Controls.Add((Control) this.bt_1);
      this.Controls.Add((Control) this.lb_message);
      this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Margin = new Padding(3, 4, 3, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ErrorBox";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "4-Day Forecast";
      this.TopMost = true;
      this.ResumeLayout(false);
    }

    private void bt_1_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void bt_2_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    private void bt_3_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
