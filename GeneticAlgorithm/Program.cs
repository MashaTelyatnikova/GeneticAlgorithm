using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GeneticAlgorithm
{
    public class Program
    {
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

        public static void Main(string[] args)
        {
            var maxPopulationSize = 100;
            var text = PrepareText(File.ReadAllLines(args[0]));

            var cipcher = File.ReadAllText(args[1]).ToLower();
            // chipcher = Regex.Replace(chipcher, @"\s+", " ");
            var alphabet = text.ToCharArray().Distinct().OrderBy(x => x).ToArray();

            var trigrams = GetTrigrams(text);
            var bigrams = GetBiigrams(text);
            var letters = GetLetters(text);

            var startPopulation =
                Enumerable.Range(0, maxPopulationSize)
                    .Select(x => new Chromosome(KeyGenerator.Generate(alphabet), alphabet)).ToList();

            var random = new Random(Guid.NewGuid().GetHashCode());
            var mutationRandom = new Random(Guid.NewGuid().GetHashCode());
            Console.WriteLine("START");
           

            for (var i = 0; i < 1000; ++i)
            {
                Console.WriteLine(i);
                var count = 0;
                while (count <= 5)
                {
                    var x = random.Next(0, startPopulation.Count);
                    var y = random.Next(0, startPopulation.Count);
                    var used = new HashSet<string>();
                    if (x != y && !used.Contains($"{x},{y}"))
                    {
                        var mother = startPopulation[x];
                        var father = startPopulation[y];
                        var child = mother.Cross(father);
                        if (mutationRandom.Next(0, 2) == 1)
                        {
                            child.Mutate();
                        }

                        startPopulation.Add(child);
                        used.Add($"{x}, {y}");
                       // used.Add(child.Value);
                        count++;
                        /*  if (!used.Contains(child.Value))
                          {
                              startPopulation.Add(child);
                              used.Add(child.Value);
                              count++;
                          }*/
                    }
                }


                if (startPopulation.Count > maxPopulationSize)
                {
                    startPopulation =
                        startPopulation.OrderBy(j => j.GetFittness(trigrams, bigrams, letters, cipcher))
                            .Take(maxPopulationSize)
                            .ToList();
                }
            }

            var max = startPopulation.OrderBy(x => x.GetFittness(trigrams, bigrams, letters, cipcher)).First();
            File.WriteAllText("decryption_result.txt", max.Apply(cipcher));
        }

        public static Dictionary<string, double> GetTrigrams(string text)
        {
            var result = new Dictionary<string, double>();
            for (var i = 0; i < text.Length - 2; i++)
            {
                var word = text.Substring(i, 3);
                if (!result.ContainsKey(word))
                {
                    result[word] = 0;
                }

                result[word]++;
            }

            return result.ToDictionary(x => x.Key, x => x.Value/(text.Length - 2));
        }

        public static Dictionary<string, double> GetBiigrams(string text)
        {
            var result = new Dictionary<string, double>();
            for (var i = 0; i < text.Length - 1; i++)
            {
                var word = text.Substring(i, 2);
                if (!result.ContainsKey(word))
                {
                    result[word] = 0;
                }

                result[word]++;
            }

            return result.ToDictionary(x => x.Key, x => x.Value/(text.Length - 1));
        }

        public static Dictionary<string, double> GetBiigrams(string[] words)
        {
            var result = new Dictionary<string, double>();
            var length = 0;
            foreach (var text in words)
            {
                for (var i = 0; i < text.Length - 1; i++)
                {
                    var word = text.Substring(i, 2);
                    if (!result.ContainsKey(word))
                    {
                        result[word] = 0;
                    }

                    result[word]++;
                }

                length += text.Length - 1;
            }


            return result.ToDictionary(x => x.Key, x => x.Value/(length));
        }

        public static Dictionary<string, double> GetTrigrams(string[] words)
        {
            var result = new Dictionary<string, double>();
            var length = 0;
            foreach (var text in words)
            {
                for (var i = 0; i < text.Length - 2; i++)
                {
                    var word = text.Substring(i, 3);
                    if (!result.ContainsKey(word))
                    {
                        result[word] = 0;
                    }

                    result[word]++;
                }

                length += text.Length - 2;
            }


            return result.ToDictionary(x => x.Key, x => x.Value/(length));
        }

        public static Dictionary<string, double> GetLetters(string[] words)
        {
            var result = new Dictionary<string, double>();
            var length = 0;
            foreach (var text in words)
            {
                for (var i = 0; i < text.Length; i++)
                {
                    var word = text.Substring(i, 1);
                    if (!result.ContainsKey(word))
                    {
                        result[word] = 0;
                    }

                    result[word]++;
                }

                length += text.Length;
            }


            return result.ToDictionary(x => x.Key, x => x.Value/(length));
        }


        public static Dictionary<string, double> GetLetters(string text)
        {
            var result = new Dictionary<string, double>();
            for (var i = 0; i < text.Length; i++)
            {
                var word = text.Substring(i, 1);
                if (!result.ContainsKey(word))
                {
                    result[word] = 0;
                }

                result[word]++;
            }

            return result.ToDictionary(x => x.Key, x => x.Value/(text.Length));
        }
    }
}