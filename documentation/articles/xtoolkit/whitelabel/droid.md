# XToolkit WhiteLabel Droid

Android platform support for WhiteLabel.

## Classes

Class Name | Links
---------- | ------
[MainApplicationBase](xref:Softeq.XToolkit.WhiteLabel.Droid.MainApplicationBase) | [Configure](../../configure-android.md#configure-project)
[DroidBootstrapperBase](xref:Softeq.XToolkit.WhiteLabel.Droid.DroidBootstrapperBase) | [Details](bootstrapper.md)
[ActivityBase](xref:Softeq.XToolkit.WhiteLabel.Droid.ActivityBase`1) | [Configure](../../create-activity.md)

## Controls

### [ColoredClickableSpan](xref:Softeq.XToolkit.WhiteLabel.Droid.Controls.ColoredClickableSpan)

`colorResourceId` is screen background color

Text color is the theme's _accent color_ or `android:textColorLink` if this attribute is defined in the theme.

For the TextView in which you want to set text with this Span you should also set

```cs
textView.MovementMethod = LinkMovementMethod.Instance;
```

---
