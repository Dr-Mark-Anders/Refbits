using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    public class ParseRegular
    {
        [TestMethod]
        public void Testc()
        {
            string pattern = @"\b[at]\w+";
            string text = "The threaded application ate up the thread pool as it executed.";
            MatchCollection matches;

            Regex defaultRegex = new(pattern);
            // Get matches of pattern in text
            matches = defaultRegex.Matches(text);
            Console.WriteLine("Parsing '{0}'", text);
            // Iterate matches
            for (int ctr = 0; ctr < matches.Count; ctr++)
                Debug.WriteLine("{0}. {1}", ctr, matches[ctr].Value);
        }

        [TestMethod]
        public void Test2()
        {
            string pattern = @"\w{1,}\d{1,}";
            string text = "=A1+AB2+A3+A44";
            MatchCollection matches;

            Regex defaultRegex = new(pattern);
            // Get matches of pattern in text
            matches = defaultRegex.Matches(text);
            Console.WriteLine("Parsing '{0}'", text);
            // Iterate matches
            for (int ctr = 0; ctr < matches.Count; ctr++)
                Debug.WriteLine("{0}. {1}", ctr, matches[ctr].Value);
        }

        [TestMethod]
        public void Test3()
        {
            string pattern = @"[A-Z]{1,}";
            string text = "=A1+AB2+A3+A44";
            MatchCollection matches;

            Regex defaultRegex = new(pattern);
            // Get matches of pattern in text
            matches = defaultRegex.Matches(text);
            Console.WriteLine("Parsing '{0}'", text);
            // Iterate matches
            for (int ctr = 0; ctr < matches.Count; ctr++)
                Debug.WriteLine("{0}. {1}", ctr, matches[ctr].Value);
        }
    }
}