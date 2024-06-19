@echo off
cd Backend\CSharp\API\bin\Release\net8.0\publish
start API.exe

cd ../../../../../frontend
start cmd /k "npm start"
