# Create Activity

First of all, see [Navigation Requirements](xtoolkit/whitelabel/navigation-requirements.md).

## Steps

1. Create `MainPageActivity.cs` in `Views` folder of your Android project:

```cs
[Activity]
public class MainPageActivity : ActivityBase<MainPageViewModel>
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.my_custom_name_page);
    }
}
```

2. Create `activity_main.xml` layout:

```xml
<?xml version="1.0" encoding="utf-8"?>
<RelativeLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="match_parent">

    <Button android:id="@+id/button1"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Button" />

</RelativeLayout>
```

After that, you should be able to navigate to the first page.

---
