using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace CN_Exam
{
	class T24
	{
		static Ping ping;
		static string alphabet = "abcdefghijklmnopqrstuvwxyz";
		public static void Test()
		{
			ping = new Ping();
			while (true)
			{
				try
				{
					Console.Write("Введите символьный адрес: ");
					string ip = Console.ReadLine();

					// Проверка на одно единственное вхождение ? в строку
					if (ip.Count(symbol => symbol == '?') == 1)
						foreach (var letter in alphabet)
						{
							var newIp = ip.Replace('?', letter);
							Ping(newIp);
						}
					else
						Ping(ip);

					static void Ping(string ip)
					{
						try
						{
							var reply = ping.Send(ip, 200);
							if (reply.Status == IPStatus.Success)
								Console.WriteLine($"{ip}: {reply.Address}");
							else
								throw new Exception();
						}
						catch (Exception)
						{
							Console.WriteLine($"{ip}: Узел не найден");
						}
					}

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
