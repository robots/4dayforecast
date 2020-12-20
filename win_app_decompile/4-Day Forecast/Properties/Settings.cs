// Decompiled with JetBrains decompiler
// Type: Forecast.Properties.Settings
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Forecast.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings settings = Settings.defaultInstance;
        return settings;
      }
    }
  }
}
