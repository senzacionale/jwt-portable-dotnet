tools\nuget.exe update -self

if not exist package mkdir package
if not exist package\content mkdir package\content
if not exist package\lib mkdir package\lib
if not exist package\lib\4.5 mkdir package\lib\4.5

copy JWT\bin\Release\JWT.dll package\lib\4.5\
copy LICENSE package\content

tools\nuget.exe pack Portable.JWT.nuspec -BasePath package

pause