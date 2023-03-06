// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Softeq.XToolkit.WhiteLabel.Tests.Services.JsonSerializers;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Reviewed.")]
internal class JsonSerializationTestsStub
{
    public string Name { get; set; }
    public int? Age { get; set; }
    public DateTime? Date { get; set; }
    public TimeSpan? Time { get; set; }

    [Newtonsoft.Json.JsonProperty("first_name")]
    public string FirstName { get; set; }

#pragma warning disable SA1300
    public string last_name { get; set; }
#pragma warning restore SA1300

    [Newtonsoft.Json.JsonIgnore]
    public string IgnoreData { get; set; }
}

// circular references
internal class JsonSerializationTestsStubRef
{
    public string Id { get; set; }
    public JsonSerializationTestsStubRef Child { get; set; }
    public List<JsonSerializationTestsStubRef> Children { get; set; }
}
