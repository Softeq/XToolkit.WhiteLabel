// Developed by Softeq Development Corporation
// http://www.softeq.com
using System;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    [ObsoleteAttribute("Will be removed in future versions. Please use For<T>().WithParam(x=>x.ParameterName, parameterValue) method in NavigationService")]
    public interface IViewModelParameter<T>
    {
        T Parameter { get; set; }
    }
}