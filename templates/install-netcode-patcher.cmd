cd ModTemplate
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli
cd ../
start /b del "install-netcode-patcher.cmd"