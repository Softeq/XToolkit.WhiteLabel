# Navigation Requirements

WhiteLabel internal ViewLocators require some rules for correct registration of ViewModel-UIPage navigation, you should follow some rules:

## Core

Every page ViewModels should have:

- `ViewModels` namespace;
- `{NAME}ViewModel` class name;
- `ViewModelBase` inherit.

## Android

Every Activity/Fragment should have:

- `Droid.Views` namespace;
- `{NAME}Activity` or `{NAME}Fragment`;
- `ActivityBase<T>` or `FragmentBase<T>` inherit.

## iOS

Every ViewController should have:

- `iOS.ViewControllers` namespace;
- `{NAME}ViewController` class name;
- `ViewControllerBase<T>` inherit;
- `{NAME}Storyboard` storyboard name (only when ViewController has Storyboard).

## Auto-registration ViewModels

- Add all assemblies with UI pages to the [SelectAssemblies](bootstrapper.md#assemblysource) (per-platform);
- Every UI page should meet the requirements above.

> UI Page - UI platform-specific page definition like Activity, Fragment, ViewController.
