using System;
using System.Linq;

namespace CN_Exam
{
	class T13
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

					var octets = ip.Split('.');

					// Проверка всех октетов на значение
					// Хотя в задании этого и нет                    пусть будет
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
