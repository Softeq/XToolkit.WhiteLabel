// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Extensions;

namespace Softeq.XToolkit.Common.Disposables;

/// <summary>
/// Provides a set of methods to create multiple subscriptions and dispose them when needed.
/// Useful for base bindable views when it's needed to subscribe in OnAppearing and unsubscribe in OnDisappearing.
/// </summary>
public class DisposableSubscriptionsComponent
{
    private readonly Func<IEnumerable<IDisposable>> _funcCreateSubscriptions;
    private readonly List<IDisposable> _subscriptions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableSubscriptionsComponent"/> class.
    /// </summary>
    /// <param name="funcCreateSubscriptions">Function which actually creates subscriptions.</param>
    public DisposableSubscriptionsComponent(Func<IEnumerable<IDisposable>> funcCreateSubscriptions)
    {
        _funcCreateSubscriptions = funcCreateSubscriptions;
    }

    /// <summary>
    /// Creates subscriptions and keeps them for future disposal.
    /// </summary>
    public void CreateSubscriptions()
    {
        var subscriptions = _funcCreateSubscriptions();
        _subscriptions.AddRange(subscriptions);
    }

    /// <summary>
    /// Disposes previously created subscriptions.
    /// </summary>
    public void DisposeSubscriptions()
    {
        _subscriptions.Apply(x => x.Dispose());
        _subscriptions.Clear();
    }
}
