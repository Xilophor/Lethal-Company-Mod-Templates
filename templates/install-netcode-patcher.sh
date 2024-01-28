#!/bin/sh
cd ModTemplate
dotnet new tool-manifest
dotnet tool install --local evaisa.netcodepatcher.cli
rm ../install-netcode-patcher.sh