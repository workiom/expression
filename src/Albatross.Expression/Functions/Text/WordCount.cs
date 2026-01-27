using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Group = Albatross.Expression.Documentation.Group;

namespace Albatross.Expression.Operations
{
    [FunctionDoc(Group.Text, "{token}( )",
@"### Returns the number of words in a string.
#### Inputs:
- string: String
#### Outputs:
- Integer"
    )]
    [ParserOperation]
    public class WordCount : PrefixOperationToken
    {
        public override string Name { get { return "WordCount"; } }
        public override int MinOperandCount { get { return 1; } }
        public override int MaxOperandCount { get { return 1; } }
        public override bool Symbolic { get { return false; } }

        private static readonly Regex EmailRegex = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);
        private static readonly Regex UrlRegex = new Regex(@"^(https?://|www\.)\S+$", RegexOptions.Compiled);
        private static readonly Regex CleanTokenRegex = new Regex(@"[^\p{L}\p{N}']+", RegexOptions.Compiled);

        public override object EvalValue(Func<string, object> context)
        {
            List<object> list = GetOperands(context);
            object value = list[0];

            if (value == null)
                return null;

            var s = value as string;
            if (s != null)
                return CountWords(s);

            var collection = value as ICollection;
            if (collection != null)
                return Convert.ToDouble(collection.Count);

            throw new UnexpectedTypeException(value.GetType());
        }

        private static double CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            text = text.Trim();

            if (text.TryNormalizeText(out var normalized))
                text = normalized;

            text = Regex.Replace(text, @"\s+", " ").Trim();
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var tokens = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int count = 0;

            foreach (var token in tokens)
            {
                string t = token.Trim();
                if (t.Length == 0)
                    continue;

                if (EmailRegex.IsMatch(t))
                {
                    count++;
                    continue;
                }

                if (UrlRegex.IsMatch(t))
                {
                    count++;
                    continue;
                }

                string cleaned = CleanTokenRegex.Replace(t, "");

                if (cleaned.Any(char.IsLetterOrDigit))
                    count++;
            }

            return count;
        }
    }
}
