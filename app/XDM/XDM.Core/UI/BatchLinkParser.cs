using System;
using System.Collections.Generic;

namespace XDM.Core.UI
{
    public static class BatchLinkParser
    {
        private static readonly char[] Separators = { ' ', '\t', '\r', '\n' };

        public static bool TryParse(string text, out List<Uri> links, out string invalidLink)
        {
            links = new List<Uri>();
            invalidLink = string.Empty;

            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            foreach (var value in text.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
            {
                var candidate = value.Trim();
                if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    invalidLink = candidate;
                    links.Clear();
                    return false;
                }

                links.Add(uri);
            }

            return true;
        }
    }
}
