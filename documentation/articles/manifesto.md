# XToolkit Manifesto

## Structure

XToolkit contains boilerplate features that you need in every Xamarin project, like MVVM, Navigation, DI. Also, it has additional components like [Permissions](xtoolkit/permissions.md), commonly used ViewModels, simple ImagePicker, [Push-Notifications](xtoolkit/push-notifications.md) and [others](xtoolkit/overview.md).

## Contribution

If you want to add something to the XToolkit follow the checklist below:

- [ ] It’s a very common feature for every Xamarin project;
- [ ] It’s a fix of the existing functionality;

Also, see [Contributing to XToolkit](contributing.md)

## Philosophy

Write what you need and no more, but when you write it, do it right.

Avoid implementing features you don’t need. You can’t design a feature without knowing what the constraints are. Implementing features "for completeness" results in unused code that is expensive to maintain, learn about, document, test, etc.

When you do implement a feature, implement it the right way. Avoid workarounds. Workarounds merely kick the problem further down the road, but at a higher cost: someone will have to relearn the problem, figure out the workaround and how to dismantle it (and all the places that now use it), and implement the feature. It’s much better to take longer to fix a problem properly than to be the one who fixes everything quickly but in a way that will require cleaning up later.

### Avoid complecting (braiding multiple concepts together)

Each API should be self-contained and should not know about other features. Interleaving concepts leads to complexity.

### Solve real problems by literally solving a real problem

Where possible, especially for new features, you should partner with a real customer who wants that feature and is willing to help you test it. Only by actually using a feature in the real world can we truly be confident that a feature is ready for prime time.

Listen to their feedback, too. If your first customer is saying that your feature doesn’t actually solve their use case completely, don’t dismiss their concerns as esoteric. Often, what seems like the problem when you start a project turns out to be a trivial concern compared to the real issues faced by real developers.

---
