#!/bin/sh
cd ../
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli
cd MonoMod.ModTemplate || exit
rm install-netcode-patcher.sh
