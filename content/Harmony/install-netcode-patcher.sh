#!/bin/sh
cd ../
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli
cd Harmony.ModTemplate || exit
rm install-netcode-patcher.sh
