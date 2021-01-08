using System;
using System.Collections.Generic;

namespace CN_Exam
{
	class Program
	{
		static readonly List<Action> tasks = new List<Action>
		{
			T11.Test,
			T12.Test,
			T13.Test,
			T14.Test,
			T15.Test,
			T16.Test,
			T17.Test,
			T18.Test,
			T19.Test,
			T20.Test,
			T21.Test,
			T22.Test,
			T23.Test,
			T24.Test,
			T25.Test
		};

		static void Main()
		{
			while (true)
			{
				Console.Write(
					"1 - задание 11" +
					"\n2 - задание 12" +
					"\n3 - задание 13" +
					"\n4 - задание 14" +
					"\n5 - задание 15" +
					"\n6 - задание 16" +
					"\n7 - задание 17" +
					"\n8 - задание 18" +
					"\n9 - задание 19" +
					"\n10 - задание 20" +
					"\n11 - задание 21" +
					"\n12 - задание 22" +
					"\n13 - задание 23" +
					"\n14 - задание 24" +
					"\n15 - задание 25" +
					"\n0 - выход" +
					"\nВыберите действие: "
					);

				if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > tasks.Count)
				{
					Console.WriteLine("Нет такого действия");
					Console.ReadKey();
					Console.Clear();
					continue;
				}

				if (choice == 0)
				{
					Console.WriteLine("Выход...");
					Environment.Exit(0);
				}

				Console.Clear();
				tasks[choice - 1]();
				Console.ReadKey();
				Console.Clear();
			}
		}
	}
}
