using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Encryptor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("You have to specify file with text and open text");
                Environment.Exit(0);
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File with text doesn't exist");
                Environment.Exit(0);
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("File with open text doesn't exist");
                Environment.Exit(0);
            }
            var text = PrepareText(File.ReadAllLines(args[0]));
            var alphabet = text.ToCharArray().Distinct().OrderBy(x => x).ToArray();
            var key = KeyGenerator.Generate(alphabet);

            var openText = PrepareText(File.ReadAllLines(args[1]));
            var encyptedText = new StringBuilder();
            foreach (var letter in openText)
            {
                encyptedText.Append(key[letter]);
            }

            File.WriteAllText("result.txt", encyptedText.ToString());
        }

        public static string PrepareText(string[] text)
        {
            return Regex.Replace(string.Join(" ",
                text.Select(
                    x =>
                        x.Trim().ToLower()
                            .Replace(".", "")
                            .Replace(",", "")
                            .Replace("!", "")
                            .Replace("?", "")
                            .Replace(";", "")
                            .Replace(":", "")
                )), @"\s+", " ");
        }
    }
}