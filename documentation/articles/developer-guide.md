# Developer Guide

## Clone source code

> git clone https://github.com/Softeq/XToolkit.WhiteLabel.git

## Building the repository

1. Open the Playground solution [samples/Playground/Playground.sln](https://github.com/Softeq/XToolkit.WhiteLabel/tree/master/samples/Playground) via Visual Studio for Mac or Visual Studio IDE or Rider;
2. Restore NuGet packages;
3. For build Android project:
   - Change build configuration to **Debug/AnyCPU**
   - Set **Playground.Droid** as startup project
4. For build iOS project:
   - Change build configuration to **Debug/iPhoneSimulator**
   - Set **Playground.iOS** as startup project