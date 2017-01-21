using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class KeyGenerator
    {
        public static Dictionary<char, char> Generate(char[] alphabet)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var used = new Dictionary<char, bool>();
            var result = new Dictionary<char, char>();
            foreach (var letter in alphabet)
            {
                var ch = alphabet[random.Next(0, alphabet.Length)];
                while (used.ContainsKey(ch))
                {
                    ch = alphabet[random.Next(0, alphabet.Length)];
                }

                used[ch] = true;
                result[letter] = ch;
            }

            return result;
        }
    }
}