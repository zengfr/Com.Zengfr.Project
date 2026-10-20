/*
 * A Combination & Permutation Algorithm Implemented with C#
 * Nov. 25, 2006
 * by Adrian
 * E-mail(Windows Live Messenger): sanyuexx@hotmail.com
 * Blog: http://www.cnblogs.com/dah
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Com.Zengfr.CodeGenerator.Common.Algorithm
{
    class Program
    {
        static void MainT(string[] args)
        {
            ShowCombinations<int>(new int[] { 1, 2, 3, 4, 5 }, 3);
            ShowPermutations<string>(new string[] { "dog", "cat", "bird", "bat" }, 4);
        }

        static void ShowPermutations<T>(T[] array, int length)
        {
            Permutations<T> permutations = new Permutations<T>(array, length);
            Console.WriteLine("{0} Permutations of Array \"{1}\":\n", permutations.Count, GetString<T>(array));
            foreach (IList<T> perm in permutations)
            {
                Console.WriteLine(GetString<T>(perm) + "\n");
            }
        }

        static void ShowCombinations<T>(T[] array, int length)
        {
            Combinations<T> combinations = new Combinations<T>(array, length);
            Console.WriteLine("{0} Combinations of Array \"{1}\":\n", combinations.Count, GetString<T>(array));
            foreach (IList<T> oneCom in combinations)
            {
                Console.WriteLine(GetString<T>(oneCom) + "\n");
            }
        }

        static string GetString<T>(IList<T> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
            {
                sb.Append(item.ToString() + ",");
            }
            sb.Remove(sb.Length-1, 1);
            return sb.ToString();
        }
    }
}
