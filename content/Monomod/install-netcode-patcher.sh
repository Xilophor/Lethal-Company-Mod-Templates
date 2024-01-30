#!/bin/sh
cd ../
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli
cd ModTemplate
rm install-netcode-patcher.sh
