#!/bin/bash

# Build via csproj
# msbuild -r Softeq.XToolkit.Documentation.csproj

DOCFX_VERSION="2.58.9"
DOCFX_PATH="$HOME/.nuget/packages/docfx.console/$DOCFX_VERSION/tools/docfx.exe"
DOCFX_CONFIG_PATH="docfx.json"

# Prepate toc.yml
mono $DOCFX_PATH metadata $DOCFX_CONFIG_PATH

# Apply patch to toc.yml
sudo pwsh unflatten-namespaces.ps1 ./obj/docfxapi/toc.yml

# Build documentation
mono $DOCFX_PATH build $DOCFX_CONFIG_PATH
