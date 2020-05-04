# Android Resources Naming Convention

## Resource files

Resources file names are written in __lowercase_underscore__.

### Drawable files

Naming conventions for drawables:

Asset Type   | Prefix            | Example
-------------|-------------------|-----------------------------
Action bar   | `ab_`             | `ab_stacked.9.png`
Button       | `btn_`            | `btn_send_pressed.9.png`
Dialog       | `dialog_`         | `dialog_top.9.png`
Divider      | `divider_`        | `divider_horizontal.9.png`
Icon         | `ic_`             | `ic_star.png`
Menu         | `menu_`           | `menu_submenu_bg.9.png`
Notification | `notification_`   | `notification_bg.9.png`
Tabs         | `tab_`            | `tab_pressed.9.png`

Naming conventions for icons (taken from [Android iconography guidelines](http://developer.android.com/design/style/iconography.html)):

Asset Type                      | Prefix             | Example
--------------------------------|--------------------|----------------------------
Icons                           | `ic_`              | `ic_star.png`
Launcher icons                  | `ic_launcher`      | `ic_launcher_calendar.png`
Menu icons and Action Bar icons | `ic_menu`          | `ic_menu_archive.png`
Status bar icons                | `ic_notify`        | `ic_notify_msg.png`
Tab icons                       | `ic_tab`           | `ic_tab_recent.png`
Dialog icons                    | `ic_dialog`        | `ic_dialog_info.png`

Naming conventions for selector states:

State	     | Suffix          | Example
-------------|-----------------|---------------------------
Normal       | `_normal`       | `btn_order_normal.9.png`
Pressed      | `_pressed`      | `btn_order_pressed.9.png`
Focused      | `_focused`      | `btn_order_focused.9.png`
Disabled     | `_disabled`     | `btn_order_disabled.9.png`
Selected     | `_selected`     | `btn_order_selected.9.png`

### Layout files

Layout files should match the name of the Android components that they are intended for but moving the top level component name to the beginning. For example, if we are creating a layout for the `SignInActivity`, the name of the layout file should be `activity_sign_in.xml`.

Component        | Prefix      | Class Name             | Layout Name
-----------------|-------------|------------------------|----------------------
Activity         | `activity_` | MainPageActivity       | `activity_main.xml`
Fragment         | `fragment_` | DetailsPageFragment    | `fragment_details.xml`
Dialog           | `dialog_`   | NewItemPageDialogFragment | `dialog_new_item.xml`
AdapterView item | `item_`     | PersonViewHolder       | `item_person.xml`
Custom view layout | `view_`   | ProfileAvatarView      | `view_profile_avatar.xml`
Partial layout   | `partial_`  | ---                    | `partial_stats_bar.xml`

A slightly different case is when we are creating a layout that is going to be inflated by an `Adapter`, e.g to populate a `ListView`. In this case, the name of the layout should start with `item_`.

Note that there are cases where these rules will not be possible to apply. For example, when creating layout files that are intended to be part of other layouts. In this case you should use the prefix `partial_`.

### Menu files

Similar to layout files, menu files should match the name of the component. For example, if we are defining a menu file that is going to be used in the `UserActivity`, then the name of the file should be `activity_user.xml`

A good practice is to not include the word `menu` as part of the name because these files are already located in the `menu` directory.

### Values files

Resource files in the values folder should be __plural__:

File          | Description
--------------|-------------
`strings.xml` | Strings
`colors.xml`  | Colors
`dimens.xml`  | Sizes
`attrs.xml`   | Custom attributes
`themes.xml`  | Theme & ThemeOverlay
`styles.xml`  | Widget styles
`shape.xml`   | ShapeAppearance
`motion.xml`  | Animations styles

## IDs

Resource IDs and names are written in __lowercase_underscore__.

Naming pattern: `@+id/<module>_<layout>_<element_name>_<element_type>`

Sample                           | Description
---------------------------------|----------------
`@+id/activity_main_lst`         | `Any List` data from MainPageActivity.
`@+id/fragment_details_name_txt` | `EditText` for Name from DetailsPageFragment.
`@+id/dialog_new_item_save_btn`  | `Button` for Save from NewItemDialog.
`@+id/item_person_name_lbl`      | `TextView` for Name Person from list item.
`@+id/notifications_activity_settings_camera_cb` | `CheckBox` for Camera from Settings page from Notifications module.

Common controls prefix mapping:

Prefix  | Controls
--------|----------
btn     | Button, ImageButton
label   | TextView
input   | EditText
list    | ListView, RecyclerView
img     | ImageView

For other controls use the short form:

Prefix| Controls
------|----------
cb    | CheckBox
ll    | LinearLayout
rl    | RelativeLayout
pb    | ProgressBar

---
