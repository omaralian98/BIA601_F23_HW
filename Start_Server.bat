@echo off
set BACKEND_PORT=5000

cd Backend\CSharp\publish
start API.exe --port=%BACKEND_PORT%
cd ../../../frontend
setlocal

:: Check for Windows
if not "%OS%"=="" (
    echo Running on Windows
    set REACT_APP_API_URL=http://localhost:%BACKEND_PORT%
    call npm start
    goto end
)

:: If not Windows
echo Running on Unix-like OS (Linux or macOS)
set REACT_APP_API_URL=http://localhost:%BACKEND_PORT%
npm start

:end
endlocal
