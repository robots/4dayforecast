// Decompiled with JetBrains decompiler
// Type: Forecast.SelectCity
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using Forecast.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Forecast
{
  public class SelectCity : Form
  {
    private IContainer components = (IContainer) null;
    private Bitmap renderBmp;
    private Point mouse_offset;
    private citydata _cdata;
    private string _DataPath;
    private string[] sortnamelist;
    private bool ready;
    private SelectCity.countrynode[][] citydb;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private ComboBox cb_continent;
    private ComboBox cb_country;
    private ComboBox cb_state;
    private ComboBox cb_city;
    private Label bt_save;
    private Label label6;

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

    public SelectCity()
    {
      this.InitializeComponent();
      this.BackgroundImage = (Image) Resources.option;
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

    private void cb_keydown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete)
      {
        if (((ComboBox) sender).DropDownStyle != ComboBoxStyle.DropDownList && ((ComboBox) sender).Items.Count > 0)
        {
          ((ComboBox) sender).DropDownStyle = ComboBoxStyle.DropDownList;
          while (((ComboBox) sender).DropDownStyle != ComboBoxStyle.DropDownList)
            Application.DoEvents();
          for (int index = 0; index < ((ComboBox) sender).Items.Count; ++index)
          {
            if (((ComboBox) sender).Items[index].ToString().StartsWith(Convert.ToString((object) e.KeyCode).ToUpper(), true, (CultureInfo) null))
            {
              ((ListControl) sender).SelectedIndex = index;
              ((Control) sender).Text = ((ComboBox) sender).Items[index].ToString();
              ((ComboBox) sender).DroppedDown = true;
              ((Control) sender).Update();
              return;
            }
          }
          ((ListControl) sender).SelectedIndex = 0;
          ((Control) sender).Text = ((ComboBox) sender).Items[0].ToString();
          ((ComboBox) sender).DroppedDown = true;
          ((Control) sender).Update();
        }
        if (((ComboBox) sender).Items.Count != 0)
          return;
        e.SuppressKeyPress = true;
      }
      else
        e.SuppressKeyPress = true;
    }

    private void label6_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private SelectCity.countrynode[] loadcon(FileStream fs, string continent)
    {
      byte[] buffer = new byte[1000000];
      fs.Read(buffer, 0, 1000000);
      string[,] strArray = new string[20000, 7];
      int index1 = 0;
      int index2 = 0;
      int index3 = 0;
      int[,] numArray1 = new int[100, 1000];
      int[] numArray2 = new int[100];
      int index4 = 0;
      int index5 = 0;
      int num1 = 0;
      while ((int) buffer[index1] != 0)
      {
        for (; (int) buffer[index1] != 13; ++index1)
        {
          if ((int) buffer[index1] == 9)
            ++index3;
          else if ((int) buffer[index1] != 34)
            strArray[index2, index3] = strArray[index2, index3] + (object) (char) buffer[index1];
        }
        index1 += 2;
        index3 = 0;
        if (index2 > 0)
        {
          if (strArray[index2, 2] != strArray[index2 - 1, 2])
          {
            numArray2[index4] = index5 + 1;
            numArray1[index4, index5] = num1;
            ++index4;
            index5 = 0;
            num1 = 0;
          }
          else if (strArray[index2, 1] != strArray[index2 - 1, 1])
          {
            numArray1[index4, index5] = num1;
            ++index5;
            num1 = 0;
          }
        }
        ++num1;
        ++index2;
      }
      numArray2[index4] = index5 + 1;
      numArray1[index4, index5] = num1;
      SelectCity.countrynode[] countrynodeArray = new SelectCity.countrynode[index4 + 1];
      string str1 = "";
      string str2 = "";
      int index6 = -1;
      int index7 = 0;
      int index8 = 0;
      int num2 = 0;
      for (int index9 = 0; index9 < index2; ++index9)
      {
        if (strArray[index9, 2] != str1)
        {
          if (index8 > 0)
            countrynodeArray[index6].numofcity = num2;
          num2 = 0;
          ++index6;
          string str3 = strArray[index9, 1];
          str1 = strArray[index9, 2];
          SelectCity.countrynode countrynode = new SelectCity.countrynode(strArray[index9, 2], continent, strArray[index9, 3], numArray2[index6]);
          countrynodeArray.SetValue((object) countrynode, index6);
          index7 = 0;
          SelectCity.statenode statenode = new SelectCity.statenode(strArray[index9, 1], numArray1[index6, index7]);
          countrynodeArray[index6].statearray.SetValue((object) statenode, index7);
          index8 = 0;
          str2 = strArray[index9, 1];
        }
        else if (strArray[index9, 1] != str2)
        {
          str2 = strArray[index9, 1];
          ++index7;
          SelectCity.statenode statenode = new SelectCity.statenode(strArray[index9, 1], numArray1[index6, index7]);
          countrynodeArray[index6].statearray.SetValue((object) statenode, index7);
          index8 = 0;
        }
        SelectCity.citynode citynode = new SelectCity.citynode(strArray[index9, 0], countrynodeArray[index6].statearray[index7].name, strArray[index9, 4], strArray[index9, 5], strArray[index9, 6]);
        countrynodeArray[index6].statearray[index7].cityarray.SetValue((object) citynode, index8);
        ++index8;
        ++num2;
      }
      countrynodeArray[index6].numofcity = num2;
      return countrynodeArray;
    }

    private void SelectCity_Load(object sender, EventArgs e)
    {
      this.ready = false;
      FileStream fs1 = new FileStream(this._DataPath + "db\\CityListAsiaAustralia.txt", FileMode.OpenOrCreate);
      this.citydb = new SelectCity.countrynode[3][];
      this.citydb[0] = this.loadcon(fs1, "Asia & Australia");
      fs1.Close();
      FileStream fs2 = new FileStream(this._DataPath + "db\\CityListEurope.txt", FileMode.OpenOrCreate);
      this.citydb[1] = this.loadcon(fs2, "Europe");
      fs2.Close();
      FileStream fs3 = new FileStream(this._DataPath + "db\\CityListNorthAmerica.txt", FileMode.OpenOrCreate);
      this.citydb[2] = this.loadcon(fs3, "North America");
      fs3.Close();
      this.cb_continent.Items.Clear();
      this.cb_continent.Items.Add((object) "Asia & Australia");
      this.cb_continent.Items.Add((object) "Europe");
      this.cb_continent.Items.Add((object) "North America");
      this.cb_country.Items.Clear();
      for (int index1 = 0; index1 < this.citydb.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.citydb[index1].GetLength(0); ++index2)
          this.cb_country.Items.Add((object) this.citydb[index1][index2].name);
      }
      if (this._cdata.cur_con == 0)
        this.cb_continent.Text = "Asia & Australia";
      else if (this._cdata.cur_con == 1)
        this.cb_continent.Text = "Europe";
      else if (this._cdata.cur_con == 2)
      {
        this.cb_continent.Text = "North America";
      }
      else
      {
        this.ready = true;
        return;
      }
      this.cb_country.Text = this.citydb[this._cdata.cur_con][this._cdata.cur_country].name;
      if (this._cdata.cur_state == -1)
      {
        this.cb_state.Text = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[0].name;
        this.cb_city.Text = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[0].cityarray[this._cdata.cur_city].name;
      }
      else
      {
        this.cb_state.Text = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].name;
        this.cb_city.Text = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[this._cdata.cur_city].name;
      }
      this.ready = true;
    }

    private void cb_continent_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cb_country.Items.Clear();
      this.cb_country.DropDownStyle = ComboBoxStyle.DropDown;
      this.cb_state.Items.Clear();
      this.cb_state.DropDownStyle = ComboBoxStyle.DropDown;
      this.cb_city.Items.Clear();
      this.cb_city.DropDownStyle = ComboBoxStyle.DropDown;
      this.cb_state.Enabled = true;
      this.cb_country.Text = "Please Select Country";
      this.cb_state.Text = "Please Select State";
      this.cb_city.Text = "Please Select City";
      switch (this.cb_continent.Text)
      {
        case "Asia & Australia":
          this._cdata.cur_con = 0;
          break;
        case "Europe":
          this._cdata.cur_con = 1;
          break;
        case "North America":
          this._cdata.cur_con = 2;
          break;
        case null:
          return;
        default:
          return;
      }
      for (int index = 0; index < this.citydb[this._cdata.cur_con].GetLength(0); ++index)
        this.cb_country.Items.Add((object) this.citydb[this._cdata.cur_con][index].name);
    }

    private void cb_country_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cb_state.Items.Clear();
      this.cb_state.DropDownStyle = ComboBoxStyle.DropDown;
      this.cb_city.Items.Clear();
      this.cb_city.DropDownStyle = ComboBoxStyle.DropDown;
      this.cb_state.Enabled = true;
      this.cb_state.Text = "Please Select State";
      this.cb_state.Update();
      this.cb_city.Text = "Please Select City";
      this.Cursor = Cursors.AppStarting;
      this.cb_city.Sorted = false;
      this.cb_state.SuspendLayout();
      this.cb_city.SuspendLayout();
      if (this.cb_continent.SelectedIndex == -1)
      {
        for (int index1 = 0; index1 < this.citydb.GetLength(0); ++index1)
        {
          for (int index2 = 0; index2 < this.citydb[index1].GetLength(0); ++index2)
          {
            if (this.citydb[index1][index2].name == this.cb_country.Text && this.cb_country.Text == this.citydb[index1][index2].name)
            {
              this._cdata.cur_con = index1;
              break;
            }
          }
        }
      }
      if (this._cdata.cur_con != -1)
      {
        int index1 = 0;
        while (index1 < this.citydb[this._cdata.cur_con].GetLength(0) && !(this.citydb[this._cdata.cur_con][index1].name == this.cb_country.Text))
          ++index1;
        this._cdata.cur_country = index1;
        if (this.citydb[this._cdata.cur_con][index1].statearray.GetLength(0) == 1)
        {
          this.cb_state.Text = "No State";
          this._cdata.cur_state = 0;
          this.cb_state.Enabled = false;
          for (int index2 = 0; index2 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[0].cityarray.GetLength(0); ++index2)
            this.cb_city.Items.Add((object) this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[0].cityarray[index2].name);
        }
        else if (this.ready)
        {
          this._cdata.cur_state = -1;
          this.sortnamelist = new string[this.citydb[this._cdata.cur_con][this._cdata.cur_country].numofcity];
          int num = 0;
          for (int index2 = 0; index2 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray.GetLength(0); ++index2)
          {
            this.cb_state.Items.Add((object) this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index2].name);
            for (int index3 = 0; index3 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index2].cityarray.GetLength(0); ++index3)
              this.sortnamelist.SetValue((object) (this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index2].cityarray[index3].name + ", " + this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index2].cityarray[index3].state), num++);
          }
          this.sort();
          for (int index2 = 0; index2 < this.sortnamelist.Length; ++index2)
            this.cb_city.Items.Add((object) this.sortnamelist[index2]);
        }
        else
        {
          for (int index2 = 0; index2 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray.GetLength(0); ++index2)
            this.cb_state.Items.Add((object) this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index2].name);
        }
      }
      this.cb_state.ResumeLayout();
      this.cb_city.ResumeLayout();
      this.Cursor = Cursors.Arrow;
    }

    private void sort()
    {
      this.sortnamelist = this.sortpro(this.sortnamelist);
    }

    private string[] sortpro(string[] list)
    {
      if (list.Length == 1)
        return list;
      int length = list.Length;
      string[] list1 = new string[length / 2];
      string[] list2 = new string[length - length / 2];
      for (int index = 0; index < length / 2; ++index)
        list1[index] = list[index];
      for (int index = 0; index < length - length / 2; ++index)
        list2[index] = list[index + length / 2];
      return this.merge(this.sortpro(list1), this.sortpro(list2));
    }

    private string[] merge(string[] str1, string[] str2)
    {
      string[] strArray = new string[str1.Length + str2.Length];
      int index1 = 0;
      int index2 = 0;
      int num = 0;
      while (index1 < str1.Length && index2 < str2.Length)
        strArray[num++] = this.compare(str1[index1], str2[index2]) != 1 ? str2[index2++] : str1[index1++];
      if (num == strArray.Length)
        return strArray;
      if (index1 == str1.Length)
      {
        while (num < strArray.Length)
          strArray[num++] = str2[index2++];
        return strArray;
      }
      while (num < strArray.Length)
        strArray[num++] = str1[index1++];
      return strArray;
    }

    private int compare(string str1, string str2)
    {
      int index;
      for (index = 0; index < str1.Length && index < str2.Length; ++index)
      {
        if ((int) char.ToLower(str1[index]) < (int) char.ToLower(str2[index]))
          return 1;
        if ((int) char.ToLower(str1[index]) > (int) char.ToLower(str2[index]))
          return 2;
      }
      return index == str1.Length ? 1 : 2;
    }

    private void cb_state_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cb_city.Items.Clear();
      this.cb_city.DropDownStyle = ComboBoxStyle.DropDown;
      this.cb_city.Text = "Please Select City";
      if (this._cdata.cur_con != -1)
      {
        int index1 = 0;
        while (index1 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray.GetLength(0) && !(this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index1].name == this.cb_state.Text))
          ++index1;
        this._cdata.cur_state = index1;
        for (int index2 = 0; index2 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index1].cityarray.GetLength(0); ++index2)
          this.cb_city.Items.Add((object) this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index1].cityarray[index2].name);
      }
      this.cb_city.Sorted = true;
    }

    private void cb_dropdown(object sender, EventArgs e)
    {
      this.mouse_offset.X = 5000;
      this.mouse_offset.Y = 5000;
    }

    private void bt_save_Click(object sender, EventArgs e)
    {
      string str1 = "";
      if (this.cb_country.Text == "Please Select Country" || this.cb_city.Text == "Please Select City" || this.cb_country.SelectedIndex == -1 || this.cb_city.SelectedIndex == -1)
      {
        int num1 = (int) MessageBox.Show("Incomplete information. Please reselect.");
      }
      else
      {
        if (this.cb_country.SelectedIndex != -1)
        {
          int index1;
          if (this._cdata.cur_state == -1)
          {
            string text = this.cb_city.Text;
            int index2 = text.Length - 1;
            int index3;
            while (true)
            {
              while ((int) text[index2] != 32 && (int) text[index2] != 44)
                str1 = "" + (object) text[index2--] + str1;
              --index2;
              index3 = 0;
              while (index3 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray.Length && !(this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[index3].name == str1))
                ++index3;
              if (index3 == this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray.Length)
                str1 = " " + str1;
              else
                break;
            }
            this._cdata.cur_state = index3;
            string str2 = text.Remove(text.Length - str1.Length - 2, str1.Length + 2);
            index1 = 0;
            while (index1 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray.GetLength(0) && !(this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[index1].name == str2))
              ++index1;
          }
          else
          {
            index1 = 0;
            while (index1 < this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray.GetLength(0) && !(this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[index1].name == this.cb_city.Text))
              ++index1;
          }
          this._cdata.cityid = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[index1].id;
          string str3 = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[index1].timediff;
          string str4 = "";
          int index4 = 0;
          while ((int) str3[index4] != 58)
            str4 += (string) (object) str3[index4++];
          this._cdata.citytimediff = Convert.ToDouble(str4);
          int num2 = index4 + 1;
          string str5 = "";
          while (num2 < str3.Length)
            str5 += (string) (object) str3[num2++];
          this._cdata.citytimediff = this._cdata.citytimediff + (double) (Math.Sign(this._cdata.citytimediff) * ((int) Convert.ToInt16(str5) / 15)) * 0.25;
          this._cdata.cityDSTmode = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[index1].DSTmode;
          this._cdata.statename = !(this.cb_country.Text == "USA") ? this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].name : this.abbr(this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].name);
          this._cdata.countryname = this.citydb[this._cdata.cur_con][this._cdata.cur_country].name;
          this._cdata.cityname = this.citydb[this._cdata.cur_con][this._cdata.cur_country].statearray[this._cdata.cur_state].cityarray[index1].name;
          this._cdata.cur_city = index1;
          this._cdata.citytimeoffset = 0.0;
        }
        if (this._cdata.cityid.Length > 9)
        {
          string str2 = "1";
          for (int index = 1; index < 9; ++index)
            str2 += (string) (object) this._cdata.cityid[index + 2];
          this._cdata.cityid = str2;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private string abbr(string fullname)
    {
      switch (fullname)
      {
        case "Alabama":
          return "AL";
        case "Alaska":
          return "AK";
        case "Arizona":
          return "AZ";
        case "Arkansas":
          return "AR";
        case "California":
          return "CA";
        case "Colorado":
          return "CO";
        case "Connecticut":
          return "CT";
        case "Delaware":
          return "DE";
        case "District of Columbia":
          return "DC";
        case "Florida":
          return "FL";
        case "Georgia":
          return "GA";
        case "Hawaii":
          return "HI";
        case "Idaho":
          return "ID";
        case "Illinois":
          return "IL";
        case "Indiana":
          return "IN";
        case "Iowa":
          return "IA";
        case "Kansas":
          return "KS";
        case "Kentucky":
          return "KY";
        case "Louisiana":
          return "LA";
        case "Maine":
          return "ME";
        case "Maryland":
          return "MD";
        case "Massachusetts":
          return "MA";
        case "Michigan":
          return "MI";
        case "Minnesota":
          return "MN";
        case "Mississippi":
          return "MS";
        case "Missouri":
          return "MO";
        case "Montana":
          return "MT";
        case "Nebraska":
          return "NE";
        case "Nevada":
          return "NV";
        case "New Hampshire":
          return "NH";
        case "New Jersey":
          return "NJ";
        case "New Mexico":
          return "NM";
        case "New York":
          return "NY";
        case "North Carolina":
          return "NC";
        case "North Dakota":
          return "ND";
        case "Ohio":
          return "OH";
        case "Oklahoma":
          return "OK";
        case "Oregon":
          return "OR";
        case "Pennsylvania":
          return "PA";
        case "Rhode Island":
          return "RI";
        case "South Carolina":
          return "SC";
        case "South Dakota":
          return "SD";
        case "Tennessee":
          return "TN";
        case "Texas":
          return "TX";
        case "Utah":
          return "UT";
        case "Vermont":
          return "VT";
        case "Virginia":
          return "VA";
        case "Washington":
          return "WA";
        case "West Virginia":
          return "WV";
        case "Wisconsin":
          return "WI";
        case "Wyoming":
          return "WY";
        default:
          return fullname;
      }
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
      base.OnPaint(e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.cb_continent = new ComboBox();
      this.cb_country = new ComboBox();
      this.cb_state = new ComboBox();
      this.cb_city = new ComboBox();
      this.bt_save = new Label();
      this.label6 = new Label();
      this.SuspendLayout();
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 14.25f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(120, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(160, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select Location";
      this.label1.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.label1.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 9.75f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = Color.White;
      this.label2.Location = new Point(8, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(88, 24);
      this.label2.TabIndex = 1;
      this.label2.Text = "Continent";
      this.label2.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.label2.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Arial", 9.75f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.label3.ForeColor = Color.White;
      this.label3.Location = new Point(8, 112);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 24);
      this.label3.TabIndex = 2;
      this.label3.Text = "Country";
      this.label3.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.label3.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Arial", 9.75f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.label4.ForeColor = Color.White;
      this.label4.Location = new Point(8, 192);
      this.label4.Name = "label4";
      this.label4.Size = new Size(98, 24);
      this.label4.TabIndex = 3;
      this.label4.Text = "Region/State";
      this.label4.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.label4.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.label5.BackColor = Color.Transparent;
      this.label5.Font = new Font("Arial", 9.75f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.label5.ForeColor = Color.White;
      this.label5.Location = new Point(8, 272);
      this.label5.Name = "label5";
      this.label5.Size = new Size(88, 24);
      this.label5.TabIndex = 4;
      this.label5.Text = "City";
      this.label5.MouseDown += new MouseEventHandler(this.label_mousedown);
      this.label5.MouseMove += new MouseEventHandler(this.label_mousemove);
      this.cb_continent.AutoCompleteMode = AutoCompleteMode.Suggest;
      this.cb_continent.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cb_continent.BackColor = Color.Black;
      this.cb_continent.FlatStyle = FlatStyle.Flat;
      this.cb_continent.ForeColor = Color.White;
      this.cb_continent.Location = new Point(8, 64);
      this.cb_continent.Name = "cb_continent";
      this.cb_continent.Size = new Size(384, 24);
      this.cb_continent.Sorted = true;
      this.cb_continent.TabIndex = 5;
      this.cb_continent.Text = "Please Select Continent";
      this.cb_continent.SelectedIndexChanged += new EventHandler(this.cb_continent_SelectedIndexChanged);
      this.cb_continent.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_continent.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_country.AutoCompleteMode = AutoCompleteMode.Suggest;
      this.cb_country.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cb_country.BackColor = Color.Black;
      this.cb_country.FlatStyle = FlatStyle.Flat;
      this.cb_country.ForeColor = Color.White;
      this.cb_country.Location = new Point(8, 144);
      this.cb_country.Name = "cb_country";
      this.cb_country.Size = new Size(384, 24);
      this.cb_country.Sorted = true;
      this.cb_country.TabIndex = 6;
      this.cb_country.Text = "Please Select Country";
      this.cb_country.SelectedIndexChanged += new EventHandler(this.cb_country_SelectedIndexChanged);
      this.cb_country.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_country.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_state.AutoCompleteMode = AutoCompleteMode.Suggest;
      this.cb_state.AutoCompleteSource = AutoCompleteSource.ListItems;
      this.cb_state.BackColor = Color.Black;
      this.cb_state.FlatStyle = FlatStyle.Flat;
      this.cb_state.ForeColor = Color.White;
      this.cb_state.Location = new Point(8, 224);
      this.cb_state.Name = "cb_state";
      this.cb_state.Size = new Size(384, 24);
      this.cb_state.Sorted = true;
      this.cb_state.TabIndex = 7;
      this.cb_state.Text = "Please Select State";
      this.cb_state.SelectedIndexChanged += new EventHandler(this.cb_state_SelectedIndexChanged);
      this.cb_state.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_state.DropDown += new EventHandler(this.cb_dropdown);
      this.cb_city.BackColor = Color.Black;
      this.cb_city.FlatStyle = FlatStyle.Flat;
      this.cb_city.ForeColor = Color.White;
      this.cb_city.Location = new Point(8, 304);
      this.cb_city.Name = "cb_city";
      this.cb_city.Size = new Size(384, 24);
      this.cb_city.Sorted = true;
      this.cb_city.TabIndex = 8;
      this.cb_city.Text = "Please Select City";
      this.cb_city.KeyDown += new KeyEventHandler(this.cb_keydown);
      this.cb_city.DropDown += new EventHandler(this.cb_dropdown);
      this.bt_save.BackColor = Color.Transparent;
      this.bt_save.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bt_save.ForeColor = Color.DarkGray;
      this.bt_save.Image = (Image) Resources.button21;
      this.bt_save.Location = new Point(104, 352);
      this.bt_save.Name = "bt_save";
      this.bt_save.Size = new Size(84, 40);
      this.bt_save.TabIndex = 9;
      this.bt_save.Text = "SAVE";
      this.bt_save.TextAlign = ContentAlignment.MiddleCenter;
      this.bt_save.MouseLeave += new EventHandler(this.button_mouseleave);
      this.bt_save.Click += new EventHandler(this.bt_save_Click);
      this.bt_save.MouseEnter += new EventHandler(this.button_mouseenter);
      this.label6.BackColor = Color.Transparent;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.ForeColor = Color.DarkGray;
      this.label6.Image = (Image) Resources.button21;
      this.label6.Location = new Point(208, 352);
      this.label6.Name = "label6";
      this.label6.Size = new Size(84, 40);
      this.label6.TabIndex = 10;
      this.label6.Text = "CANCEL";
      this.label6.TextAlign = ContentAlignment.MiddleCenter;
      this.label6.MouseLeave += new EventHandler(this.button_mouseleave);
      this.label6.Click += new EventHandler(this.label6_Click);
      this.label6.MouseEnter += new EventHandler(this.button_mouseenter);
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.BackgroundImage = (Image) Resources.option;
      this.ClientSize = new Size(400, 400);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.bt_save);
      this.Controls.Add((Control) this.cb_city);
      this.Controls.Add((Control) this.cb_state);
      this.Controls.Add((Control) this.cb_country);
      this.Controls.Add((Control) this.cb_continent);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(3, 4, 3, 4);
      this.Name = "SelectCity";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Location";
      this.TransparencyKey = Color.Red;
      this.MouseMove += new MouseEventHandler(this.Main_MouseMove);
      this.MouseDown += new MouseEventHandler(this.Main_MouseDown);
      this.Load += new EventHandler(this.SelectCity_Load);
      this.ResumeLayout(false);
    }

    public class citynode
    {
      public string name;
      public string state;
      public string id;
      public string timediff;
      public string DSTmode;

      public citynode(string name, string state, string id, string timediff, string DSTmode)
      {
        this.name = name;
        this.state = state;
        this.id = id;
        this.timediff = timediff;
        this.DSTmode = DSTmode;
      }
    }

    public class statenode
    {
      public string name;
      public SelectCity.citynode[] cityarray;

      public statenode(string name, int citynum)
      {
        this.name = name;
        this.cityarray = new SelectCity.citynode[citynum];
      }
    }

    public class countrynode
    {
      public string name;
      public string continent;
      public string shortname;
      public SelectCity.statenode[] statearray;
      public int numofcity;

      public countrynode(string name, string continent, string shortname, int statenum)
      {
        this.name = name;
        this.continent = continent;
        this.shortname = shortname;
        this.statearray = new SelectCity.statenode[statenum];
      }
    }
  }
}
