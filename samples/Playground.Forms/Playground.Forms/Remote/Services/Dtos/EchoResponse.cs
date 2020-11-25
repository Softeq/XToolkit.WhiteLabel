// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Playground.Forms.Remote.Services.Dtos
{
    public class EchoResponse
    {
        public Dictionary<string, string>? Args { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
        public Uri? Url { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");

            if (Args != null)
            {
                sb
                    .AppendLine("Args: {")
                    .AppendLine(DictToString(Args))
                    .AppendLine("}");
            }

            if (Headers != null)
            {
                sb
                    .AppendLine("Headers: {")
                    .AppendLine(DictToString(Headers))
                    .AppendLine("}");
            }

            if (Url != null)
            {
                sb.AppendLine($"Url: {Url}");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string DictToString(Dictionary<string, string> dict) =>
            string.Join("\n", dict.Select(x => $"\t\"{x.Key}\": \"{x.Value}\""));
    }
}
