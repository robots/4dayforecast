// Decompiled with JetBrains decompiler
// Type: Forecast.Program
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System;
using System.Threading;
using System.Windows.Forms;

namespace Forecast
{
  internal static class Program
  {
    public static bool Startup;

    [STAThread]
    private static void Main(string[] args)
    {
      bool initiallyOwned = true;
      Program.Startup = false;
      foreach (string str in args)
      {
        if (str == "/Startup")
          Program.Startup = true;
      }
      bool createdNew;
      Mutex mutex = new Mutex(initiallyOwned, "4-Day Forecast", out createdNew);
      if (!createdNew)
      {
        int num = (int) new ErrorBox(5).ShowDialog();
      }
      else
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new Main());
        mutex.ReleaseMutex();
      }
    }
  }
}
