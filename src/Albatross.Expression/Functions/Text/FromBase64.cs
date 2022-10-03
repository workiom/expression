using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Albatross.Expression.Functions.Text
{
    [FunctionDoc(Group.Text, "{token}(@text)",
        @"
### Transforms any base64 string into an object.
#### Inputs:
- text: Any

#### Outputs:
- Object

#### References:
- [{token}](https://help.workiom.com/article/formula#{token})
        ",
        @"
{token}(""MQ=="")
        "
    )]
    [ParserOperation]
    public class FromBase64 : PrefixOperationToken
    {
        public override string Name { get { return "FromBase64"; } }
        public override int MinOperandCount { get { return 1; } }
        public override int MaxOperandCount { get { return 1; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            object value = Operands.First().EvalValue(context);
            return Base64ToObject(value?.ToString());
        }

        private object Base64ToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
