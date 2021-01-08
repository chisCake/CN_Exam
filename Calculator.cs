using System;
using System.Linq;

using Addresses;

namespace CN_Exam
{
	static class Calculator
	{
		// Network ID
		public static IPAddress GetNetwork(IPAddress ip, Netmask netmask) =>
			new IPAddress(ip.Bits.And(netmask.Bits));

		public static IPAddress GetNetwork(IPAddress ip, int netmaskLength) =>
			new IPAddress(ip.Bits.And(new Netmask(netmaskLength).Bits));


		// Host ID
		public static IPAddress GetHost(IPAddress ip, Netmask netmask) =>
			new IPAddress(ip.Bits.And(netmask.Wildcard));

		public static IPAddress GetHost(IPAddress ip, int netmaskLength) =>
			new IPAddress(ip.Bits.And(new Netmask(netmaskLength).Wildcard));


		// Широковещательный адрес
		public static IPAddress GetBroadcast(IPAddress ip, Netmask netmask) =>
			new IPAddress(ip.Bits.Or(netmask.Wildcard));

		public static IPAddress GetBroadcast(IPAddress ip, int netmaskLength) =>
			new IPAddress(ip.Bits.Or(new Netmask(netmaskLength).Wildcard));


		// Кол-во битов выделенных под хосты
		public static int GetHostBits(Netmask netmask) =>
			netmask.Bits.Cast<bool>().Where(bit => bit == false).Count();

		public static int GetHostBits(int netmaskLength) => 
			32 - netmaskLength;


		// Маска под определённое нужное кол-во хостов
		public static Netmask GetNetmask(int hosts)
		{
			if (hosts < 1)
				throw new ArgumentOutOfRangeException("Минимальное кол-во хостов = 2");

			int bitPos;
			int places = 2;
			for (bitPos = 2; places < hosts; bitPos++)
				places += Convert.ToInt32(Math.Pow(2, bitPos));
			return new Netmask(32 - bitPos);
		}
	}
}
