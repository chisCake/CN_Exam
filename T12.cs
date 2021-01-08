using System;

namespace CN_Exam
{
	class T12
	{
		public static void Test()
		{
			while (true)
			{
				try
				{
					Console.Write("Введите маску сети: ");
					string netmask = Console.ReadLine().Trim();

					if (netmask.Length != 32)
						throw new Exception("Число битов != 32");

					// Если вдруг понадобится сделать нормальную маску (конструктор: new Netmask(bits))
					// var bits = new System.Collections.BitArray(32);

					bool zero = false;
					for (int i = 0; i < 32; i++)
					{
						if (netmask[i] == '1')
						{
							// Если встретилась 1, но уже встречался ноль
							if (zero)
								throw new Exception("Непрерывность едениц была прервана");

							//bits.Set(i, true);
						}
						else if (netmask[i] == '0')
						{
							// Если первым же символом идёт 0
							if (i == 0)
								throw new Exception("Маска начинается с 0");

							zero = true;
							//bits.Set(i, false);
						}
						else
							throw new Exception("Встретился недопустимый символ");
					}
					Console.WriteLine("Маска введена верно");
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
