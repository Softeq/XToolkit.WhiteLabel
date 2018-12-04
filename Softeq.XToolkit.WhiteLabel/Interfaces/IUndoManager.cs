// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IUndoManager
    {
        void AddOperation(Action<CancellationTokenSource> undoAction, Action<CancellationToken> applyAction,
            string infoLabel, string actionLabel, CancellationTokenSource cancellationTokenSource);
    }
}