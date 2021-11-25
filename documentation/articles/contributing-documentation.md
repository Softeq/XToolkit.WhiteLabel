# Contribution to documentation

This document provides an overview of how articles published on  https://softeq.github.io/XToolkit.WhiteLabel/ should be formatted. You can actually use this file, itself, as a template when contributing articles.

## Article Structure

We are using [DocFX](https://dotnet.github.io/docfx/) for our documentation, which means that each page needs to have a Markdown layout, which would look something like:

```md
# Title

## Sub-title

Text

---

```

Edit an existing documentation page, to find out what the current layout looks like.

## Documentation Syntax

The documentation uses [DocFX Flavored Markdown](https://dotnet.github.io/docfx/spec/docfx_flavored_markdown.html), aka DFM. It supports all [GitHub Flavored Markdown](https://github.github.com/gfm/) syntax and compatible with CommonMark.

### Adding relative links

To reference other pages inside the documentation use the following syntax:

```md
[Softeq](https://www.softeq.com)
```

### Adding images

Please add any images for the documentation in the `images/` folder. Then you can reference you image like:

```md
![My helpful screenshot](../images/screenshot.png)
```

### References

#### Articles

```md
[IBindingsOwner](xref:Softeq.XToolkit.Bindings.Abstract.IBindingsOwner)
```

#### Code

System types:

```xml
<see cref="T:UIKit.UIViewController"/>
```

XToolkit types:

```xml
<see cref="AsyncCommand"/>
```

## Build documentation locally

In some cases it might be more comfortable to work locally on updating the documentation pages instead of using the online GitHub editor. This is especially the case when working on bigger changes you’d most likely do on a separate branch and maybe spend multiple days working on. In those cases it might be usefull to be able to generate the site locally, so you can see what your changes look like when rendered in the browser. This means you will have to follow these steps:

1. Make changes in `documentation/*` folder
2. Build documentation via `cd documentation && ./build.sh`
3. Open `documentation/_site/index.html` in browser

## Summary

This style guide is intended to help contributors quickly create new articles for https://softeq.github.io/XToolkit.WhiteLabel/. It includes the most common syntax elements that are used, as well as overall document organization guidance. If you discover mistakes or gaps in this guide, please submit an issue.

---
