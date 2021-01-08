using System;
using System.Collections;
using System.Linq;

namespace CN_Exam
{
	class T15
	{
		public static void Test()
		{
			while (true)
			{
				try
				{
					// Без понятия зачем тут ip, если кол-во хостов определяет маска
					// Но пусть будет, коль задание требует
					Console.Write("Введите IP-адрес: ");
					string ip = Console.ReadLine();
					Console.Write("Введите маску сети: ");
					string netmask = Console.ReadLine();

					int hosts = Convert.ToInt32(Math.Pow(2, GetHostBits(new Netmask(netmask)))) - 2;

					Console.WriteLine("Возможное кол-во адресов в сети: " + hosts);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				Console.ReadKey();
				Console.Clear();
			}
		}

		static int GetHostBits(Netmask netmask) =>
			netmask.Bits.Cast<bool>().Where(bit => bit == false).Count();

		abstract class Address
		{
			public int[] Octets { get; protected set; }
			public BitArray Bits { get; protected set; }

			protected Address(string address)
			{
				Octets = ParseToInt(address);
				Bits = GetBits(this);
			}

			public static BitArray GetBits(Address address)
			{
				var bits = new BitArray(32);
				for (int i = 0; i < 4; i++)
				{
					BitArray b = new BitArray(new byte[] { (byte)address.Octets[i] });
					for (int j = 7, pos = 8 * i; j >= 0; j--, pos++)
						bits[pos] = b[j];
				}
				return bits;
			}

			public static int[] ParseToInt(string address)
			{
				var strOctets = address.Split('.');
				if (strOctets.Length != 4)
					throw new FormatException("Кол-во октетов != 4");

				var octets = new int[4];
				for (int i = 0; i < 4; i++)
					if (!int.TryParse(strOctets[i], out octets[i]) || octets[i] < 0 || octets[i] > 255)
						throw new FormatException("Неверный формат октета");

				return octets;
			}

			public override string ToString() => $"{Octets[0]}.{Octets[1]}.{Octets[2]}.{Octets[3]}";
		}

		class Netmask : Address
		{
			public Netmask(string address) : base(address)
			{
				var octets = ParseToInt(address);
				if (!Check(octets))
					throw new FormatException("Неверный формат маски");
			}

			static bool Check(int[] octets)
			{
				for (int i = 0; i < 4; i++)
					if (!IsCorrect(octets[i]))
						return false;
				return true;

				static bool IsCorrect(int num)
				{
					for (int i = 0; i < 9; i++)
						if (256 - Math.Pow(2, i) == num)
							return true;
					return false;
				}
			}
		}
	}
}
