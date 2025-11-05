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

        public override string Name => "WordCount";
        public override int MinOperandCount => 1;
        public override int MaxOperandCount => 1;
        public override bool Symbolic => false;
        
        static readonly Regex WordPattern = new Regex(@"\b[\w']+\b", RegexOptions.Compiled);

        public override object EvalValue(Func<string, object> context)
        {
            List<object> list = GetOperands(context);

            object value = list[0];
            switch (value)
            {
                case null:
                    return null;
                case string s:
                    return CountWords(s);
                case ICollection collection:
                    return Convert.ToDouble(collection.Count);
                default:
                    throw new UnexpectedTypeException(value.GetType());
            }
        }
        
        private static double CountWords(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;
            
            text = text.Trim();

            var result = text.TryNormalizeText(out string normalizedText);
            if (result)
                text = normalizedText;
        
            // Collapse all whitespace to single spaces
            text = Regex.Replace(text, @"\s+", " ");
            text = text.Trim();
        
            if (string.IsNullOrEmpty(text))
                return 0;

            MatchCollection matches = WordPattern.Matches(text);
        
            // Filter out matches that are only apostrophes or punctuation
            int count = 0;
            foreach (Match match in matches)
            {
                string word = match.Value;
                // Only count if the word contains at least one alphanumeric character
                if (word.Any(char.IsLetterOrDigit))
                {
                    count++;
                }
            }
        
            return count;
        }
    }
}
