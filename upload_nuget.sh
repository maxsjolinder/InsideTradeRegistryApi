#!/usr/bin/env bash

mkdir -p ./nuget
dotnet pack ./InsideTradeRegistry.Api/InsideTradeRegistry.Api.csproj -c Release -o ./nuget
dotnet nuget push ./InsideTradeRegistry.Api/nuget/NorthernLight.InsideTradeRegistry.Api.*.nupkg -s https://api.nuget.org/v3/index.json -k $NUGET_API_KEY