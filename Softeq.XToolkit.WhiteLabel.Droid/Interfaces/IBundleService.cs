// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Interfaces;

/// <summary>
///     Represents methods for saving/restoring ViewModel navigation parameters on Android.
/// </summary>
public interface IBundleService
{
    /// <summary>
    ///     Marks that we have a state that should be restored.
    /// </summary>
    /// <param name="bundle">Android Bundle.</param>
    void SaveInstanceState(Bundle bundle);

    /// <summary>
    ///     Saves navigation <paramref name="parameters"/> into the <paramref name="intent"/>.
    /// </summary>
    /// <param name="intent">Android Intent object.</param>
    /// <param name="parameters">ViewModel navigation parameters.</param>
    void TryToSetParams(Intent intent, IReadOnlyList<NavigationParameterModel>? parameters);

    /// <summary>
    ///     Restores <paramref name="viewModel"/> navigation parameters from <paramref name="intent"/>
    ///     if <paramref name="bundle"/> was marked for restore.
    /// </summary>
    /// <remarks>
    ///     Skip restore when:
    ///     <list type="number">
    ///         <item>ViewModel was alive</item>
    ///         <item>Activity never been destroyed</item>
    ///         <item>we don't have data to restore.</item>
    ///     </list>
    /// </remarks>
    /// <param name="viewModel">Target ViewModel.</param>
    /// <param name="intent">Android Intent.</param>
    /// <param name="bundle">Android Bundle.</param>
    void TryToRestoreParams(ViewModelBase viewModel, Intent? intent, Bundle? bundle);
}
