# Developer Guide

## Clone source code

```sh
git clone https://github.com/Softeq/XToolkit.WhiteLabel.git
```

## Building the repository

1. Open the main solution [XToolkit.sln](https://github.com/Softeq/XToolkit.WhiteLabel/tree/master/) via Visual Studio for Mac or Visual Studio IDE or Rider;
2. Restore NuGet packages;
3. For build Android project:
   - Change build configuration to **Debug.Droid/AnyCPU**
   - Set **Playground.Droid** as startup project
4. For build iOS project:
   - Change build configuration to **Debug.iOS/iPhoneSimulator**
   - Set **Playground.iOS** as startup project
5. For build Xamarin.Forms projects:
   - Android: **Playground.Forms.Droid**
   - iOS: **Playground.Forms.iOS**

---
