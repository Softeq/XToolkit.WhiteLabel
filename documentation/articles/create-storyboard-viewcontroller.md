# Create Storyboard ViewController

First of all, see [Navigation Requirements](xtoolkit/whitelabel/navigation-requirements.md).

## Steps

### 1. Create Storyboard

- VS4Mac: `Solution folder -> Add -> New File -> iOS -> Storyboard`
- Rider: `Solution folder -> Add -> Storyboard`
- Create empty `MainPageStoryboard`

### 2. Interface Builder

- Open storyboard via Interface Builder (IB)
  - VS4Mac: `MainPageStoryboard -> Open With -> Xcode Interface Builder`
  - Rider: `MainPageStoryboard -> Open in Xcode`
- IB: Select **Identity Inspector** and update next properties:
  - Custom Class > `Class` to `MainPageViewController`
  - Identity > `Storyboard ID` to `MainPageViewController`

> More detailed info: [Creating a Storyboard with Xcode
](https://learn.microsoft.com/en-us/xamarin/ios/user-interface/storyboards/indepth-storyboard?tabs=macos#creating-a-storyboard-with-xcode)

### 3. Setup ViewController

```cs
public partial class MainPageViewController : ViewControllerBase<MainPageViewModel>
{
    public MainPageViewController(NativeHandle handle)
        : base(handle)
    {
    }
}
```

**Note**
You must add this constructor when you want to use storyboard:

```cs
public MainPageViewController(NativeHandle handle) : base(handle) {}
```

If you load storyboard via code you have to create constructor without parameters.

### 4. Profit

After these steps, you should be able to navigate to `MainPageViewModel`.

---
