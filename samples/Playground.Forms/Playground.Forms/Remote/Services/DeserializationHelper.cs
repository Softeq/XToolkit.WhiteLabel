// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;
using Playground.Forms.Remote.Services.Dtos;

namespace Playground.Forms.Remote.Services
{
    public static class DeserializationHelper
    {
        public static async IAsyncEnumerable<EchoResponse> DeserializeAsync(
            JsonTextReader jsonReader,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            EchoResponse? model = null;

            while (await jsonReader.ReadAsync(cancellationToken))
            {
                // begin construct
                if (jsonReader.TokenType == JsonToken.StartObject && jsonReader.LinePosition < 3)
                {
                    model = new EchoResponse();
                }

                // manual construct model
                if (model != null)
                {
                    switch (jsonReader.Path)
                    {
                        // some properties for example
                        case "args":
                            model.Args = new Dictionary<string, string>();
                            break;
                        case "args.n" when jsonReader.TokenType == JsonToken.String:
                            model.Args!.Add("n", jsonReader.Value!.ToString());
                            break;
                        case "url" when jsonReader.TokenType == JsonToken.String:
                            model.Url = new Uri(jsonReader.Value!.ToString());
                            break;
                    }
                }

                // end construct
                if (jsonReader.TokenType == JsonToken.EndObject && jsonReader.LinePosition < 3
                    && model != null)
                {
                    yield return model;
                }
            }
        }
    }
}
