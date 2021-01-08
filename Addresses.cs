using System;
using System.Collections;
using System.Linq;

// В этом файле чисто то, что было изначально
// В самих заданиях только то, что нужно чтобы их сделать
// Т.е. там просто вырезки отсюда

namespace Addresses
{
	// Представляет структуру-логику самого адреса
	abstract class Address
	{
		// Октеты [192,168,0,1]
		public int[] Octets { get; protected set; }
		// Биты [1,1,0,0,0,0,0,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1]
		public BitArray Bits { get; protected set; }

		public int this[int index]
		{
			get => Octets[index];
			private set => Octets[index] = value;
		}

		protected Address()
		{
			Octets = new int[4];
			Bits = new BitArray(32);
		}

		protected Address(string address)
		{
			Octets = ParseToInt(address);
			Bits = GetBits(this);
		}

		protected Address(int[] octets)
		{
			if (octets.Length != 4 || octets.Any(octet => octet < 0 || octet > 255))
				throw new FormatException("Неверный формат адреса");

			Octets = octets;
			Bits = GetBits(this);
		}

		protected Address(BitArray bits)
		{
			Octets = ParseToInt(bits);
			Bits = bits;
		}

		// Получение адреса в битовом представлении
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

		// Строковый адрес в октеты
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

		// Биты в октеты
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

		public override string ToString() => $"{Octets[0]}.{Octets[1]}.{Octets[2]}.{Octets[3]}";
	}

	// IP адрес - весь его функционал есть уже в базовом классе
	class IPAddress : Address
	{
		public IPAddress(string address) : base(address) { }
		public IPAddress(BitArray bits) : base(bits) { }
	}

	// Маска - дополнительные проверки/ограничения + длина
	class Netmask : Address
	{
		public int Length { get; }
		public BitArray Wildcard { get => Bits.Not(); }

		// Конструктор маски по адресу
		public Netmask(string address) : base(address)
		{
			var octets = ParseToInt(address);
			if (!Check(octets))
				throw new FormatException("Неверный формат маски");

			Length = Bits.Cast<bool>().Where(bit => bit == true).Count();
		}

		// Конструктор маски по длине
		public Netmask(int length) : base(ParseToBits(length)) { }

		// Конструктор маски по массиву битов
		public Netmask(BitArray bits) : base(bits)
		{
			var octets = ParseToInt(bits);
			if (!Check(octets))
				throw new FormatException("Неверный формат маски");

			Length = Bits.Cast<bool>().Where(bit => bit == true).Count();
		}

		public static BitArray ParseToBits(int length)
		{
			if (length < 0 || length > 32)
				throw new FormatException("Неверная длина маски");

			var bits = new BitArray(32);
			for (int i = 0; i < 32; i++)
				bits[i] = i < length;

			return bits;
		}

		// Проверка правильности маски
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
