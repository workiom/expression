using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Albatross.Expression.Functions.Text
{
    [FunctionDoc(Group.Text, "{token}(@object)",
        @"
### Transforms any object into base64 string.
#### Inputs:
- object: Any

#### Outputs:
- String

#### References:
- [{token}](https://help.workiom.com/article/formula#{token})
        ",
        @"
{token}(1)
        "
    )]
    [ParserOperation]
    public class ToBase64 : PrefixOperationToken
    {
        public override string Name { get { return "ToBase64"; } }
        public override int MinOperandCount { get { return 1; } }
        public override int MaxOperandCount { get { return 1; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            object value = Operands.First().EvalValue(context);
            return ObjectToBase64(value);
        }

        private string ObjectToBase64(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
