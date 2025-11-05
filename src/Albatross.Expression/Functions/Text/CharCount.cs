using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Group = Albatross.Expression.Documentation.Group;

namespace Albatross.Expression.Functions.Text
{
    [FunctionDoc(Group.Text, "{token}( )",
        @"### Returns the number of characters in a string.
#### Inputs:
- string: String
#### Outputs:
- Integer"
    )]
    [ParserOperation]
    public class CharCount : PrefixOperationToken
    {
        public override string Name => "CharCount";
        public override int MinOperandCount => 1;
        public override int MaxOperandCount => 1;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            List<object> list = GetOperands(context);

            object value = list[0];
            switch (value)
            {
                case null:
                    return null;
                case string s:
                    return CountCharacters(s);
                case ICollection collection:
                    return Convert.ToDouble(collection.Count);
                default:
                    throw new UnexpectedTypeException(value.GetType());
            }
        }

        private static double CountCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;
            
            text = text.Trim();

            var result = text.TryNormalizeText(out string normalizedText);
            if (result)
                text = normalizedText;
    
            // Count ONLY alphanumeric characters (letters A-Z, a-z and numbers 0-9)
            // Per requirements: Do NOT count apostrophes, spaces, punctuation, or special symbols
            int count = 0;
            foreach (var c in text)
            {
                if (char.IsLetter(c) || char.IsDigit(c))
                    count++;
            }

            return count;
        }
    }
}