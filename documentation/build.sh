#!/bin/bash

DOCFX_VERSION="2.62.1"
DOCFX_CONFIG_PATH="docfx.json"

# install tool
dotnet tool update -g docfx --version $DOCFX_VERSION

# build documentation
docfx build $DOCFX_CONFIG_PATH

# dev server
#docfx $DOCFX_CONFIG_PATH --serve