# Kill Telemetry

В сети полно способов, как отключить телеметрию, но нет ни одного рабочего. Не помогает даже [физическое удаление](https://www.youtube.com/watch?v=yEOsQdTfPEY)
CompatTelRunner.exe: через некоторое время телеметрия опять насилует винт.

Данный сервис раз в секунду просматривает список процессов Windows 10 и убивает Microsoft Compatibility Telemetry, если она запущена.

Установка:
1) Скопировать все файлы из Release в любую папку (например в c:\Programs\KillTelemetry\)
2) Запустить Install.bat от администратора (правой кнопкой -> Запуск от имени администратора)
