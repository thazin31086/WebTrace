FOR /R "C:\PhD\Research Play Zone\ProjectConverter\Project" %%f IN (*.cs) DO REN "%%f" *.txt
for /r "C:\PhD\Research Play Zone\ProjectConverter\Project" %%i in (*.txt) do copy "%%~fi" "C:\PhD\Research Play Zone\ProjectConverter\After"
@echo off
pause