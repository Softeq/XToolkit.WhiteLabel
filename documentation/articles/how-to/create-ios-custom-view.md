# Create iOS Custom View

How to create custom view via VS for Mac:

## Approaches

### 1. ViewCell

- Create Table/CollectionViewCell
- Change base class to the `CustomViewBase`
- Remove `..Cell.xib`
- Create new view (xib) with the same name (use freeform size)
- Attach new xib to the custom view (as File's Owner Class)

### 2. xib

- Create new view (xib) (use freeform size)
- Create empty class with attribute `[Register("MyCustomView")]`
- Change base class to the `CustomViewBase`
- Restart XCode if opened
- Attach new xib to the custom view (as **_File's Owner Class_**)

---
