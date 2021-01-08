using System;
using System.Collections;
using System.Linq;

namespace CN_Exam
{
	class T14
	{
		public static void Test()
		{
			while (true)
			{
				try
				{
					Console.Write("Введите IP адрес: ");
					string ip = Console.ReadLine().Trim();

					// Сначала проверка на цифры и точки
					foreach (var symbol in ip)
						if (symbol != '.' && !int.TryParse(symbol.ToString(), out _))
							throw new Exception("Введён недопустимый символ");

					// Проверка разделителей на кол-во
					if (ip.Count(symbol => symbol == '.') != 3)
						throw new Exception("Неверно введены символы-разделители");

					var octets = ip.Split('.');

					// Проверка на кол-во октетов
					if (octets.Length != 4)
						throw new Exception("Введено неверное кол-во октетов");

					// Проверка всех октетов на значение
					if (octets.Any(octet => Convert.ToInt32(octet) > 255))
						throw new Exception("Неверно введено значение октета");

					Console.WriteLine("IP адрес введён верно");
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				Console.ReadKey();
				Console.Clear();
			}
		}
	}
}
