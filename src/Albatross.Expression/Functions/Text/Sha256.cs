using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Albatross.Expression.Operations
{
    [FunctionDoc(Group.Text, "{token}( )",
        @"### Compute SHA-256 hash of the input text and return it as a lowercase hex string.
#### Inputs:
- text: String
#### Outputs:
- String (lowercase hex)"
    )]
    [ParserOperation]
    public class Sha256 : PrefixOperationToken
    {
        public override string Name => "Sha256";
        public override int MinOperandCount => 1;
        public override int MaxOperandCount => 1;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            List<object> list = GetOperands(context);
            if (list.Count == 0)
                return null;

            object value = list[0];

            string text;
            if (value is string)
                text = Convert.ToString(value);
            else
                throw new UnexpectedTypeException(value?.GetType());

            using (var sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2")); 

                return sb.ToString();
            }
        }
    }
}