using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace CN_Exam
{
	class T23
	{
		static Ping ping;
		public static void Test()
		{
			ping = new Ping();
			while (true)
			{
				try
				{
					Console.Write("Введите символьный адрес: ");
					string ip = Console.ReadLine();

					if (ip.Any(symbol => @"?></\|=}{[]‘".Any(forbidden => forbidden == symbol)))
						throw new Exception("Введён недопустимый символ");

					var reply = ping.Send(ip);
					if (reply.Status == IPStatus.Success)
						Console.WriteLine($"IP адрес {ip}: {reply.Address}");
					else
						throw new Exception("Узел не найден");
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
