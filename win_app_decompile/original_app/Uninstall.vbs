Dim shell, systempath

set shell = WScript.CreateObject( "WScript.Shell" )

systempath = shell.ExpandEnvironmentStrings("%SystemRoot%")

shell.Run Chr(34) & systempath & "\system32\msiexec.exe" & Chr(34) & "  /x{7AA7B9FF-F4B4-4B64-8C51-7C3B4D0F5A28}"

WScript.Quit