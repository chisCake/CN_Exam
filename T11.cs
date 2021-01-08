using System;
using System.Collections;

namespace CN_Exam
{
	static class T11
	{
		public static void Test()
		{
			while (true)
			{
				try
				{
					Console.Write("Введите IP-адрес: ");
					string ip = Console.ReadLine();
					Console.Write("Введите маску сети: ");
					string netmask = Console.ReadLine();

					var network = GetNetwork(new IPAddress(ip), new Netmask(netmask));
					var host = GetHost(new IPAddress(ip), new Netmask(netmask));

					Console.WriteLine("Network ID: " + network);
					Console.WriteLine("Host ID: " + host);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				Console.ReadKey();
				Console.Clear();
			}
		}

		static IPAddress GetNetwork(IPAddress ip, Netmask netmask) =>
			new IPAddress(ip.Bits.And(netmask.Bits));

		static IPAddress GetHost(IPAddress ip, Netmask netmask) =>
			new IPAddress(ip.Bits.And(netmask.Bits.Not()));

		abstract class Address
		{
			public int[] Octets { get; }
			public BitArray Bits { get; }

			protected Address(string address)
			{
				Octets = ParseToInt(address);
				Bits = GetBits(this);
			}

			protected Address(BitArray bits)
			{
				Octets = ParseToInt(bits);
				Bits = bits;
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

			public static int[] ParseToInt(BitArray bits)
			{
				if (bits.Length != 32)
					throw new FormatException("Кол-во битов != 32");

				var octets = new int[4];
				for (int i = 0; i < 4; i++)
				{
					int octet = 0;
					for (int j = 7, pos = 8 * i; j >= 0; j--, pos++)
						if (bits[pos] == true)
							octet += Convert.ToInt32(Math.Pow(2, j));
					octets[i] = octet;
				}
				return octets;
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

			public override string ToString() => $"{Octets[0]}.{Octets[1]}.{Octets[2]}.{Octets[3]}";
		}

		class IPAddress : Address
		{
			public IPAddress(string address) : base(address) { }
			public IPAddress(BitArray bits) : base(bits) { }
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
