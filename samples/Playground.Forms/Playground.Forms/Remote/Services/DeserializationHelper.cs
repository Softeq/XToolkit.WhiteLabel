// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;
using Playground.Forms.Remote.Services.Dtos;

namespace Playground.Forms.Remote.Services
{
    public static class DeserializationHelper
    {
        public static async IAsyncEnumerable<EchoResponse?> DeserializeAsync(
            JsonTextReader jsonReader,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var serializer = new JsonSerializer();

            while (await jsonReader.ReadAsync(cancellationToken))
            {
                yield return serializer.Deserialize<EchoResponse?>(jsonReader);
            }
        }
    }
}
