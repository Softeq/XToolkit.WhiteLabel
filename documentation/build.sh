#!/bin/bash

DOCFX_VERSION="2.62.0" # see https://github.com/dotnet/docfx/pull/8371
DOCFX_CONFIG_PATH="docfx.json"

dotnet tool install -g docfx --version $DOCFX_VERSION

# Build documentation
docfx build $DOCFX_CONFIG_PATH
