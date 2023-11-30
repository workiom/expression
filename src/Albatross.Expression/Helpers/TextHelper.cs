using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Albatross.Expression.Helpers
{
    public static class TextHelper
    {
        public static double CountChars(string text)
        {
            // Strip of markdown syntax
            text = StripMarkdownTags(text);
            
            // Define the pattern to match non-space characters
            string pattern = @"\S";
        
            // Use Regex.Matches to find all matches
            MatchCollection matches = Regex.Matches(text, pattern);
        
            // Count the number of matches
            var charCount = (double)matches.Count;

            return charCount;
        }
        
        public static double CountWords(string text)
        {
            // Strip of markdown syntax
            text = StripMarkdownTags(text);
            
            // Define a regular expression pattern for matching words
            string pattern = @"\b\w+\b";

            // Use Regex.Matches to find all matches in the input text
            var matches = Regex.Matches(text, pattern);

            // The Count property of MatchCollection gives the number of matches
            var wordCount = (double)matches.Count;

            return wordCount;
        }
        
        private static string StripMarkdownTags(string markdownText)
        {
            // Replace All \n to into \\n so it can be seen as a new line
            markdownText = markdownText.Replace("\\n", "\n");

            // Convert it to Html and then to plain text
            markdownText = ConvertHtmlToPlainText(CommonMark.CommonMarkConverter.Convert(markdownText));
            
            // Remove all markdown tags
            markdownText = Regex.Replace(markdownText, "\n=+", ""); // Headers    
            markdownText = Regex.Replace(markdownText, @"\*\*(.*?)\*\*", "$1", RegexOptions.Multiline); // Remove bold tags
            markdownText = Regex.Replace(markdownText, @"\*(.*?)\*", "$1", RegexOptions.Multiline); // Remove italic tags
            markdownText = Regex.Replace(markdownText, @"~~(.*?)~~", "$1", RegexOptions.Multiline); // Remove strikethrough tags
            markdownText = Regex.Replace(markdownText, @"`(.*?)`", "$1", RegexOptions.Multiline); // Remove inline code tags
            markdownText = Regex.Replace(markdownText, @"\[(.*?)\]\(.*?\)", "$1", RegexOptions.Multiline); // Remove link tags
            markdownText = Regex.Replace(markdownText, @"!\[(.*?)\]\(.*?\)", "$1", RegexOptions.Multiline); // Remove image tags
            markdownText = Regex.Replace(markdownText, @"^#+\s+", "", RegexOptions.Multiline); // Remove heading tags
            markdownText = Regex.Replace(markdownText, @"\n[*-]\s+", ""); // Remove list item tags
            markdownText = Regex.Replace(markdownText, @"\n\d+\.\s+", ""); // Remove ordered list item tags
            markdownText = Regex.Replace(markdownText, @"^-{3,}", "", RegexOptions.Multiline); // Remove horizontal rule tags
            markdownText = Regex.Replace(markdownText, @"`{3}[\s\S]*?`{3}", "", RegexOptions.Multiline); // Remove code block tags
            markdownText = Regex.Replace(markdownText, @"\|.*?\|(\n\|.*?\|)*", ""); // Remove table tags

            // Replace escape sequences with white space
            markdownText = Regex.Replace(markdownText, @"\\[abfnrt\'\""\0]|\\x[0-9a-fA-F]{2}|\\u[0-9a-fA-F]{4}", " ");
            
            // Add regex to remove escape sequences
            markdownText = Regex.Replace(markdownText, @"\\[nt\""']+", "");
            
            return markdownText.Trim(); // Trim leading and trailing spaces
        }
        
        private static string ConvertHtmlToPlainText(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Use HtmlAgilityPack to extract text from HTML
            return doc.DocumentNode.InnerText;
        }
    }
}