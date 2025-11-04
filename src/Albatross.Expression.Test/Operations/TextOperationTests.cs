using Albatross.Expression.Exceptions;
using Albatross.Expression.Operations;
using Albatross.Expression.Tokens;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Albatross.Expression.Test
{
    [TestFixture]
    [Category("Operations")]
    public class TextOperationTests
    {
        // Sha256
        [TestCase("sha256('abc')", ExpectedResult = "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]
        [TestCase("sha256('The quick brown fox jumps over the lazy dog')", ExpectedResult = "d7a8fbb307d7809469ca9abcb0082e4f8d5651e46d3cdb762d02d0bf37c9e592")]
        
        // Padding
        [TestCase("padleft(1, 3)", ExpectedResult = "  1")]
        [TestCase("padleft(1, 3, 0)", ExpectedResult = "001")]
        [TestCase("padleft(1111, 3, 0)", ExpectedResult = "1111")]
        
        [TestCase("padright(1, 3)", ExpectedResult = "1  ")]
        [TestCase("padright(1, 3, 0)", ExpectedResult = "100")]
        [TestCase("padright(1111, 3, 0)", ExpectedResult = "1111")]
        
        
        // Right & Left
        [TestCase("left(\"abc\", 2)", ExpectedResult = "ab")]
        [TestCase("left(\"abc\", 0)", ExpectedResult = "")]
        [TestCase("left(\"abc\", 3)", ExpectedResult = "abc")]
        [TestCase("left(\"abc\", 10)", ExpectedResult = "abc")]
        [TestCase("right(\"123456789\", 8)", ExpectedResult = "23456789")]
        [TestCase("right(\"123456789\", 1)", ExpectedResult = "9")]
        [TestCase("right(\"123456789\", 0)", ExpectedResult = "")]
        [TestCase("right(\"123456789\", 9)", ExpectedResult = "123456789")]
        [TestCase("right(\"123456789\", 100)", ExpectedResult = "123456789")]
        [TestCase("right(\"123456789\", 100)", ExpectedResult = "123456789")]
        
        // Index Of
        [TestCase("indexof(\"123456789\", \"3\")", ExpectedResult = 2)]
        [TestCase("indexof(\"123456789\", \"a\")", ExpectedResult = -1)]
        [TestCase("indexof(\"121212\", \"2\")", ExpectedResult = 1)]
        [TestCase("indexof(\"hom.rcp@gmail.com\", \"@\")", ExpectedResult = 7)]
        [TestCase("indexof(\"hom.rcp@gmail.com\", \"@\") - 1", ExpectedResult = 6)]
        [TestCase("indexof(\"hom.rcp@gmail.com\", \"XYZ\") ", ExpectedResult = -1)] // does not exist
        
        // Replace
        [TestCase("replace(\"Hello World\", \"l\", \"x\")", ExpectedResult = "Hexxo Worxd")]
        [TestCase("replace(\"1 2 3 4 5\", \" \", \" - \")", ExpectedResult = "1 - 2 - 3 - 4 - 5")]
        
        // Trim
        [TestCase("trim(\" Ahmad   \")", ExpectedResult = "Ahmad")]
        [TestCase("trimRight(\" Ahmad   \")", ExpectedResult = " Ahmad")]
        [TestCase("trimLeft(\" Ahmad   \")", ExpectedResult = "Ahmad   ")]
        
        // General
        [TestCase("format(1000, \"#,#\")", ExpectedResult = "1,000")]
        [TestCase("Text(1)", ExpectedResult = "1")]
        
        // Compound
        [TestCase("if(\"\", true, false)", ExpectedResult = false)]
        [TestCase("if(\" S \", true, false)", ExpectedResult = true)]
        [TestCase("if(indexof(\"something in germany\" , \"germany\") <> -1, true , false)", ExpectedResult = true)]
        [TestCase("if(indexof(\"something in germany\" , \"germany\") <> Number(\"-1\") ,True ,False )", ExpectedResult = true)]
        
        [TestCase("substring(\"123456789\", 3)", ExpectedResult = "456789")]
        [TestCase("substring(\"123456789\", 5)", ExpectedResult = "6789")]
        [TestCase("substring(\"123456789\", 0, 2)", ExpectedResult = "12")]
        [TestCase("substring(\"123\", 3)", ExpectedResult = "")]
        [TestCase("substring(\"hom.rcp@gmail.com\", 8)", ExpectedResult = "gmail.com")]
        
        [TestCase("substring(\"hom.rcp@gmail.com\", indexof(\"hom.rcp@gmail.com\", \"@\"))", ExpectedResult = "@gmail.com")]
        [TestCase("concat(true,5, 8,\"-variable\")", ExpectedResult = "58-variable")]
      
        // Contains
        [TestCase("contains(\"[Option one, Option two]\", \"[Option one]\")", ExpectedResult = true)]
        [TestCase("contains(\"[Option one, Option two]\", \"[Option one, Option two]\")", ExpectedResult = true)]
        [TestCase("contains(\"[Option one, Option two]\", \"[Option one, Option three]\")", ExpectedResult = false)]
        [TestCase("contains(\"[Option one, Option two]\", \"Option three\")", ExpectedResult = false)]
        [TestCase("contains(\"Option one\", \"Option one\")", ExpectedResult = true)]
        [TestCase("contains(\"Option one\", \"Option two\")", ExpectedResult = false)]
        
        // Word Count markdown text
        [TestCase("wordCount(\"**123**\")", ExpectedResult = 1)]
        [TestCase("wordCount(\"_123_\")", ExpectedResult = 1)]
        [TestCase("wordCount(\"**__123 123__**\")", ExpectedResult = 2)]
        [TestCase("wordCount(\"### H3\\n\\nH2\\n--\\n\\nH1\\n==\")", ExpectedResult = 3)]
        [TestCase("wordCount(\"**Bold **_Italic_ ~~StrikeThrough~~ **_~~BoldItalicStrikeThrough~~_****_ BoldItalic_** _~~ItalicStrikeThrough~~__ _**~~BoldStrikeThrough~~**\")", ExpectedResult = 7)]
        [TestCase("wordCount(\"> Block Quote\\n\\n`Code`\\n\\n    Code Block\\n\\nEmoji \\n\\n[Link](https://stackedit.io/app#)\")", ExpectedResult = 7)]
        [TestCase("wordCount(\"1. LevelOne\\n\\n2. LevelTwo\\n\\n\\n\\n* LevelOne\\n\\n* LevelTwo\")", ExpectedResult = 4)]
        [TestCase("wordCount(\"1. **Bold**\\n\\n2. **_BoldItalic_**\\n\\n3. **_~~BoldItalicStrikeThrough~~_**\\n\\n4. [Link](http://asdasdasdad@dasfv/asdqw)\\n\\n5. Emoji 2 \ud83d\ude07 \ud83d\ude17\")", ExpectedResult = 6)]

        // Not Contains
        [TestCase("NotContains(\"Option One\", \"Option two\")", ExpectedResult = true)] 
        [TestCase("NotContains(\"Option One\", \"Option One\")", ExpectedResult = false)] 
        [TestCase("NotContains(\"[Option One, Option two]\", \"Option One\")", ExpectedResult = false)] 
        [TestCase("NotContains(\"[Option One, Option two]\", \"Option three\")", ExpectedResult = true)]
        [TestCase("wordCount(\"1. **Bold**\\n\\n2. **_BoldItalic_**\\n\\n3. **_~~BoldItalicStrikeThrough~~_**\\n\\n4. [Link](http://asdasdasdad@dasfv/asdqw)\\n\\n5. Emoji 2 \ud83d\ude07 \ud83d\ude17\")", ExpectedResult = 6)]
        
        // In
        [TestCase("In(\"Option one\", \"[Option one, Option two]\")", ExpectedResult = true)]
        [TestCase("In(\"Option three\", \"[Option one, Option two]\")", ExpectedResult = false)]
        [TestCase("In(\"[Option one, Option two]\", \"[Option one, Option two, Option three]\")", ExpectedResult = true)]
        [TestCase("In(\"[Option one, Option four]\", \"[Option one, Option two, Option three]\")", ExpectedResult = false)]
        [TestCase("In(\"Option one\", \"Option one\")", ExpectedResult = true)]
        [TestCase("In(\"Option one\", \"Option two\")", ExpectedResult = false)]
      
        // Not In
        [TestCase("NotIn(\"Option three\", \"[Option one, Option two]\")", ExpectedResult = true)]
        [TestCase("NotIn(\"Option one\", \"[Option one, Option two]\")", ExpectedResult = false)]
        [TestCase("NotIn(\"[Option four, Option five]\", \"[Option one, Option two, Option three]\")", ExpectedResult = true)]
        [TestCase("NotIn(\"[Option one, Option two]\", \"[Option one, Option two, Option three]\")", ExpectedResult = false)]
        [TestCase("NotIn(\"Option two\", \"Option two\")", ExpectedResult = false)]
        [TestCase("NotIn(\"Option three\", \"Option two\")", ExpectedResult = true)]
        // Char Count   
        [TestCase("charCount(\"123\")", ExpectedResult = 3)]
        [TestCase("charCount(\"123 123\")", ExpectedResult = 6)]
        [TestCase("charCount(\"Word\")", ExpectedResult = 4)]
        [TestCase("charCount(\"C Sharp\")", ExpectedResult = 6)]
        
        // Char Count markdown text
        [TestCase("charCount(\"**123**\")", ExpectedResult = 3)]
        [TestCase("charCount(\"_123_\")", ExpectedResult = 3)]
        [TestCase("charCount(\"**__123 123__**\")", ExpectedResult = 6)]
        [TestCase("charCount(\"### H3\\n\\nH2\\n--\\n\\nH1\\n==\")", ExpectedResult = 6)]
        [TestCase("charCount(\"**Bold **_Italic_ ~~StrikeThrough~~ **_~~BoldItalicStrikeThrough~~_****_ BoldItalic_** _~~ItalicStrikeThrough~~__ _**~~BoldStrikeThrough~~**\")", ExpectedResult = 96)]
        [TestCase("charCount(\"> Block Quote\\n\\n`Code`\\n\\n    Code Block\\n\\nEmoji \\n\\n[Link](https://stackedit.io/app#)\")", ExpectedResult = 32)]
        [TestCase("charCount(\"1. LevelOne\\n\\n2. LevelTwo\\n\\n\\n\\n* LevelOne\\n\\n* LevelTwo\")", ExpectedResult = 32)]
        [TestCase("charCount(\"1. **Bold**\\n\\n2. **_BoldItalic_**\\n\\n3. **_~~BoldItalicStrikeThrough~~_**\\n\\n4. [Link](http://asdasdasdad@dasfv/asdqw)\\n\\n5. Emoji 2 \ud83d\ude07 \ud83d\ude17\")", ExpectedResult = 47)]
        [TestCase("charCount(\"[IRFAN](https://testqa.workiom.club/app/apps/b226f5a6-7a1a-41be-ac89-546bd3084bf5/list/1f815325-d905-49e5-8976-c26dafff4f89/394417) IRFAN\")", ExpectedResult = 10)]
        [TestCase("charCount(\"IRFAN 💫\")", ExpectedResult = 5)]
        [TestCase("charCount(\"IRFAN\\n=====\")", ExpectedResult = 5)]
        [TestCase("charCount(\"> IRFAN\")", ExpectedResult = 5)]
        public object OperationsTesting(string expression)
        {
            return Factory.Instance.Create().Compile(expression).EvalValue(null);
        }
    }
}
