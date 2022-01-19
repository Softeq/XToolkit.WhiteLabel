#!/bin/bash

# msbuild -r Softeq.XToolkit.Documentation.csproj

DOCFX_VERSION="2.58.9"
DOCFX_PATH="$HOME/.nuget/packages/docfx.console/$DOCFX_VERSION/tools/docfx.exe"
DOCFX_CONFIG_PATH="docfx.json"

mono $DOCFX_PATH metadata $DOCFX_CONFIG_PATH
sudo pwsh unflatten-namespaces.ps1 ./obj/docfxapi/toc.yml
mono $DOCFX_PATH build $DOCFX_CONFIG_PATH
