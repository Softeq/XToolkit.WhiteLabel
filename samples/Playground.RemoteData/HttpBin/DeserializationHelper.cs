// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;

namespace Playground.RemoteData.HttpBin
{
    public static class DeserializationHelper
    {
        public static async IAsyncEnumerable<T> DeserializeAsync<T>(
            JsonTextReader jsonReader,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var serializer = new JsonSerializer();

            while (await jsonReader.ReadAsync(cancellationToken))
            {
                yield return serializer.Deserialize<T>(jsonReader)!;
            }
        }
    }
}
