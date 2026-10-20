@echo off
for %%i in (*.proto) do (
    C:\Users\zengfr\Desktop\apk\15\ljd\protobuf\bin\protoc --csharp_out=./ %%i
    rem 从这里往下都是注释，可忽略
    echo From %%i To %%~ni.cs Successfully!  
)
pause
 