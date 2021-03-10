using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Text;

namespace FreqAn
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch SW = new Stopwatch();
			ConcurrentDictionary<string, int> triplets = new ConcurrentDictionary<string, int>();



			//TODO better path
			//Get path
			string path = @"save.txt";
			//Console.WriteLine("Default file is " + path + ". Press enter to continue or set new path. \nPath:") ;
			//string temp = Console.ReadLine();
			//if(temp != "")
			//	path = temp;
			//while (!File.Exists(path))
			//{
			//	Console.WriteLine("Incorrect path, try again");
			//	path = Console.ReadLine();
			//}


			SW.Start();

			//Tripplet counter using dictionary
			Parallel.ForEach(File.ReadLines(@"save.txt"), str =>
			{
				Queue<char> word = new Queue<char>();
				foreach (char c in str)
				{
					if (!char.IsLetter(c))
					{
						word.Clear();
						continue;
					}
					word.Enqueue(c);
					if (word.Count == 3)
					{
						string sword = new string(word.ToArray());
						triplets.AddOrUpdate(sword, 1, (word, oldValue) => oldValue + 1);
						word.Dequeue();
					}
				}
			});

			var ans = triplets.ToArray();
			Array.Sort(ans, (x, y) => y.Value.CompareTo(x.Value));
			for (int i = 0; i < ans.Length && i < 10; i++)
				Console.Write(ans[i].ToString() + (i < ans.Length-1 && i < 9 ? ", " : "."));

			SW.Stop();
			Console.WriteLine("\n" + SW.Elapsed.TotalMilliseconds);
			Console.ReadKey();
		}
	}
}
