@echo off
cd Backend\CSharp\publish
start API.exe
cd ../../../frontend
setlocal

:: Check for Windows
if not "%OS%"=="" (
    echo Running on Windows
    set REACT_APP_API_URL=http://localhost:5000
    call npm start
    goto end
)

:: If not Windows, assume Unix-like (Linux/macOS)
echo Running on Unix-like OS (Linux or macOS)
set REACT_APP_API_URL=http://localhost:5000
npm start

:end
endlocal