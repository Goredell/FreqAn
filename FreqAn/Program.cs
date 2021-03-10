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
			ConcurrentDictionary<string, int> triplets = new ConcurrentDictionary<string, int>();	// Thread-safe dictionary of trilets; Key is triplet, Value is amount;
			string path = "text.txt";
			Console.OutputEncoding = Encoding.UTF8;



			//Get path
			Console.Write("Default file is \"{0}\". Press enter to continue or set new path. \nPath:", path);
			string temp = Console.ReadLine();
			if(temp != "")
				path = temp;
			while (!File.Exists(@path))
			{
				Console.WriteLine("Incorrect path, try again.  \nPath:");
				path = Console.ReadLine();
			}
			Console.WriteLine("File is valid, starting file processing.");


			SW.Start();

			//Count triplets using dictionary
			Parallel.ForEach(File.ReadLines(@path), str =>
			{
				Queue<char> word = new Queue<char>();									// works as a coursor in the string
				foreach (char c in str)																
				{																					
					if (!char.IsLetter(c))										// block any non Letter from word and starts word anew if encounters non letter
					{																				
						word.Clear();																
						continue;																	
					}																				
					word.Enqueue(c);										// Add new char to for new word aka triplet to form a new coursor
					if (word.Count == 3)										// Since we block all nonletters any triplet is valid
					{																				
						string sword = new string(word.ToArray());						// Make string out of queue
						triplets.AddOrUpdate(sword, 1, (sword, oldValue) => oldValue + 1);			// if key is known add 1 to value, if not add key with value of 1
						word.Dequeue();										// remove 1st char to prepare new word
					}																				
				}
			});

			//Process the answer
			var ans = triplets.ToArray();
			if (ans != null && ans.Length !=0)
			{
				Array.Sort(ans, (x, y) => y.Value.CompareTo(x.Value));
				for (int i = 0; i < ans.Length && i < 10; i++)
					Console.Write(ans[i].ToString() + (i < ans.Length - 1 && i < 9 ? ", " : ".\n"));
			}
			else
				Console.WriteLine("File was empty or didn't have any triplets");

			SW.Stop();
			Console.WriteLine("{0:F1} ms", SW.Elapsed.TotalMilliseconds);
			Console.ReadKey();
		}
	}
}
