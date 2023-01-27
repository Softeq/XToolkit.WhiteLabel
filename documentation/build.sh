#!/bin/bash

# Build via csproj
# msbuild -r Softeq.XToolkit.Documentation.csproj

DOCFX_VERSION="2.60.2"
DOCFX_CONFIG_PATH="docfx.json"

dotnet tool install -g docfx --version $DOCFX_VERSION

# Prepate toc.yml
docfx metadata $DOCFX_CONFIG_PATH

# Apply patch to toc.yml
sudo pwsh nested-namespaces.ps1 ./obj/docfxapi/toc.yml

# Build documentation
docfx build $DOCFX_CONFIG_PATH
