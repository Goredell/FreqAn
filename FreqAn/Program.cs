using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace FreqAn
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch SW = new Stopwatch();
			ConcurrentDictionary<string, int> triplets = new ConcurrentDictionary<string, int>();	// Thread-safe dictionary of trilets; Key is triplet, Value is amount;
			Console.OutputEncoding = Encoding.UTF8;



			//Get path
			string path = Validate_Path();
			SW.Start();

			//Count triplets using dictionary
			Parallel.ForEach(File.ReadLines(@path), str =>
			{
				var word = new LinkedList<char>();													// works as a coursor in the string
				foreach (char c in str)																
				{																					
					if (!char.IsLetter(c))															// block any non-Letter from word and starts word anew if encounters non-letter
					{																				
						word.Clear();																
						continue;																	
					}																				
					word.AddLast(c);																// Add new char to for new word aka triplet to form a new coursor
					if (word.Count == 3)															// Since we block all nonletters any triplet is valid
					{																				
						string sword = new string(word.ToArray());									
						triplets.AddOrUpdate(sword, 1, (sword, oldValue) => oldValue + 1);			// if key is known add 1 to value, if not add key with value of 1
						word.RemoveFirst();															// remove 1st char to prepare new word
					}																				
				}
			});

			//Process the answer
			var ans = (from tri in triplets
					   orderby tri.Value descending
					   select tri)
					   .Take(10)
					   .ToArray();

			if (ans != null && ans.Length !=0)
			{
				for (int i = 0; i < ans.Length && i < 10; i++)
					Console.Write(ans[i].ToString() + (i < ans.Length - 1 && i < 9 ? ", " : ".\n"));
			}
			else
				Console.WriteLine("File was empty or didn't have any triplets");

			SW.Stop();
			Console.WriteLine("{0:F1} ms", SW.Elapsed.TotalMilliseconds);
			Console.ReadKey();
		}

		static public string Validate_Path()
		{
			string path = "save.txt";
			Console.Write($"Default file is \"path\". Press enter to continue or set new path. \nPath:") ;

			string temp = Console.ReadLine();
			if (temp != "")
				path = temp;

			while (!File.Exists(@path))
			{
				Console.WriteLine("Incorrect path, try again.  \nPath:");
				path = Console.ReadLine();
			}
			Console.WriteLine("File is valid, starting file processing.");
			return path;
		}
	}
}
