# Create Storyboard ViewController

First of all, see [Navigation Requirements](xtoolkit/whitelabel/navigation-requirements.md).

## Steps

1. Create `MainPageStoryboard` from `Solution folder -> Add -> New File -> iOS -> Storyboard`.
2. Open storyboard via Interface Builder(`MainPageStoryboard -> Open With -> XCode Interface Builder`)
3. Select `Identity Inspector` and update next properties:
    - `Class` to `MainPageViewController.cs`
    - `Storyboard Id` to `MainPageViewController`

4. Setup `MainPageViewController` class:

```csharp
public partial class MainPageViewController : ViewControllerBase<MainPageViewModel>
{
    public MainPageViewController(IntPtr handle) : base(handle) {}
}
```
---

**Note**
You need to add constructor when you want to use storyboard:
```cs
public MainPageViewConstroller(IntPtr handle) : base(handle) {}
```
If you load storyboard via code you have to create constructor without parameters.

After these steps, you should be able to navigate to `MainPageViewModel`.

---
