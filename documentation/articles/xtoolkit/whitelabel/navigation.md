# WhiteLabel Navigation

To navigate from ViewModel to ViewModel you can use the following code:

## Navigation

### Simple Navigation

```csharp
var _pageNavigationService = new PageNavigationService(viewLocator, jsonSerializer);

_pageNavigationService
    .For<MainPageViewModel>()
    .Navigate();
```

### Navigation with parameter

```cs
_pageNavigationService
    .For<MainPageViewModel>()
    .WithParam(x => x.ParameterName, parameterValue)
    .Navigate(shouldClearBackstack);
```

### Navigation with several parameters

```cs
_pageNavigationService
    .For<MainPageViewModel>()
    .WithParam(x => x.Name, "Sherlock Holmes")
    .WithParam(x => x.Age, 25)
    .WithParam(x => x.Gender, null)
    .Navigate(shouldClearBackstack);
```

<div class="IMPORTANT">
  <h5>IMPORTANT</h5>
  <p>Use only simple models as parameters, because they can be de/serialized.</p>
</div>

---
