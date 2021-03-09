using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace FreqAn
{
	class Program
	{
		static void Main(string[] args)
		{
			ConcurrentDictionary<string, int> triplets = new ConcurrentDictionary<string, int>();
			Stack<string> file = new Stack<string>();



			//TODO better path
			//Get path
			string path = "save.txt";
			//Console.WriteLine("Default file is " + path + ". Press enter to continue or set new path. \nPath:") ;
			//string temp = Console.ReadLine();
			//if(temp != "")
			//	path = temp;
			//while (!File.Exists(path))
			//{
			//	Console.WriteLine("Incorrect path, try again");
			//	path = Console.ReadLine();
			//}


			Stopwatch SW = new Stopwatch();
			SW.Start();

			//TODO parallel reader
			//Reader
			using (StreamReader reader = new StreamReader(path))
			{
				string readText;
				while ((readText = reader.ReadLine()) != null)
					file.Push(readText);
			}

			Console.WriteLine(	"\nЧтение файла окончено " + SW.Elapsed + 
								"\nКол-во строчек в файле " + file.Count);

			Parallel.ForEach(file, str =>
			{
				Queue<char> word = new Queue<char>();
				foreach (char c in str)
				{
					if (!char.IsLetter(c))
					{
						word.Clear();
						continue;
					}
					if (word.Count == 3)
					{
						string sword = new string(word.ToArray());
						triplets.AddOrUpdate(sword, 1, (word, oldValue) => oldValue + 1);
						word.Dequeue();
					}
					word.Enqueue(c);
				}
			});

			var ans = triplets.ToArray();
			Array.Sort(ans, (x, y) => y.Value.CompareTo(x.Value));
			for (int i = 0; i < ans.Length && i < 10; i++)
				Console.Write(ans[i].ToString() + ", ");
			SW.Stop();
			Console.WriteLine("\n" + SW.Elapsed);
			Console.ReadKey();
		}
		//static Task<Stack<string>> readit(string path)
		//{
		//	Stack<string> file = new Stack<string>();
		//	using (StreamReader reader = File.ReadLines(path)
		//			.AsParallel().WithDegreeOfParallelism(10)
		//			ForAll(x => file.Push(x)));
		//	return file;
		//}
	}
}
