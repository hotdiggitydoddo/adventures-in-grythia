using System;
using System.Text;
using System.Text.RegularExpressions;
using AdventuresInGrythia.Domain.Contracts;

namespace AdventuresInGrythia.Web
{
    public class WebOutputFormatter : IOutputFormatter
    {
        public string FormatMessage(string msg)
        {
             DoColorsAndLineBreaks(ref msg);

            var sb = new StringBuilder();
            sb.Append("<div>").Append(msg).Append("</div>");

            return sb.ToString();
        }

        private void DoColorsAndLineBreaks(ref string msg)
        {
            var colorTokens = Regex.Matches(msg, "<#\\w{3,}>");

            foreach (Match match in colorTokens)
            {
                var clr = match.Value.Substring(2, match.Value.Length - 3);
                msg = msg.Replace(match.Value, $"<span style=\"color: {clr}\">");
            }

            msg = msg.Replace("<#>", "</span>");
            msg = msg.Replace("\n", "</div><div>");
            msg = msg.Replace("<tab>", "<span style=\"margin-right: 25px\"></span>");
        }
    }
}