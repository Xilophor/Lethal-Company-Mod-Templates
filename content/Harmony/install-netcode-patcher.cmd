cd ../
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli
cd ModTemplate
start /b del "install-netcode-patcher.cmd"
