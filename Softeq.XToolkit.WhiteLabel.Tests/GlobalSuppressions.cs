// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1502:Element should not be on a single line",
    Justification = "Allowed for tests, stubs")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1649:File name should match first type name",
    Justification = "Mixed types allowed for tests")]

[assembly: SuppressMessage(
    "Reliability",
    "CA2007:Consider calling ConfigureAwait on the awaited task",
    Justification = "Allowed for tests")]
