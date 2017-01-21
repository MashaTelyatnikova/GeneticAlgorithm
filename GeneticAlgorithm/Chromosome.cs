using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgorithm
{
    public class Chromosome
    {
        private readonly char[] alphabet;
        private readonly Dictionary<char, char> permutation;
        private double? fittness;
        public string Value { get; set; }

        public Chromosome(Dictionary<char, char> permutation, char[] alphabet)
        {
            this.permutation = permutation;
            Value = "";
            foreach (var v in permutation)
            {
                Value += v.Value;
            }
            this.alphabet = alphabet;
        }

        public double GetFittness(Dictionary<string, double> trainTrigrams, Dictionary<string, double> trainBigrams,
            Dictionary<string, double> trainLetters, string text)
        {
            if (fittness.HasValue)
            {
                return fittness.Value;
            }
            var applyed = Apply(text);
            var chipcherTrigrams = Program.GetTrigrams(applyed);
            var chipperBigrams = Program.GetBiigrams(applyed);
            var chipperLetters = Program.GetLetters(applyed);

            var s1 = GetSum(trainTrigrams, chipcherTrigrams);
            var s2 = GetSum(trainBigrams, chipperBigrams);
            var s3 = GetSum(trainLetters, chipperLetters);

            return new[] {s1, s2, s3}.Sum();
        }

        public double GetSum(Dictionary<string, double> source, Dictionary<string, double> target)
        {
            var result = 0.0;
            foreach (var val in source)
            {
                var r = val.Value - (target.ContainsKey(val.Key) ? target[val.Key] : 0);
                result += r*r;
            }

            return result;
        }

        public void Mutate()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var x = random.Next(0, alphabet.Length);

            var y = random.Next(0, alphabet.Length);
            while (x == y)
            {
                y = random.Next(0, alphabet.Length);
            }

            var tmp = permutation[alphabet[x]];
            permutation[alphabet[x]] = permutation[alphabet[y]];
            permutation[alphabet[y]] = tmp;
        }

        public Chromosome Cross(Chromosome other)
        {
            var child = new Dictionary<char, char>();
            foreach (var ch in permutation.Keys)
            {
                child[ch] = other.permutation[permutation[ch]];
            }

            return new Chromosome(child, alphabet);
        }

        public string Apply(string text)
        {
            var result = new StringBuilder();
            foreach (var ch in text)
            {
                result.Append(permutation[ch]);
            }
            return result.ToString();
        }

        public override bool Equals(object obj)
        {
            return Value.Equals(((Chromosome) obj).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}