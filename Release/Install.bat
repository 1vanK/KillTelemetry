set "PATH=c:\Windows\Microsoft.NET\Framework\v4.0.30319\;c:\Windows\System32\" 
cd /D "%~dp0"
InstallUtil.exe KillTelemetry.exe
net start KillTelemetry
pause
