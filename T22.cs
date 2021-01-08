using System;
using System.Net.NetworkInformation;

namespace CN_Exam
{
	class T22
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
