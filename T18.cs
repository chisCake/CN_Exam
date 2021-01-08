using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace CN_Exam
{
	class T18
	{
		public static void Test()
		{
			Table();
		}

		// Структура arp записи
		[StructLayout(LayoutKind.Sequential)]
		struct MIB_IPNETROW
		{
			public int dwIndex;
			public int dwPhysAddrLen;
			public byte mac0;
			public byte mac1;
			public byte mac2;
			public byte mac3;
			public byte mac4;
			public byte mac5;
			public byte mac6;
			public byte mac7;
			public int dwAddr;
			public int dwType;
		}

		// Получение собсно самой таблицы
		[DllImport("IpHlpApi.dll")]
		static extern int GetIpNetTable(IntPtr pIpNetTable, ref int pdwSize, bool bOrder);

		//[DllImport("IpHlpApi.dll")]
		//internal static extern int FreeMibTable(IntPtr plpNetTable);

		// Ошибка выделения буффера
		// const int ERROR_INSUFFICIENT_BUFFER = 122;

		static void Table()
		{
			// Получение нужного кол-ва памяти, с записью в никуда
			int bytesNeeded = 0;
			GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);
			// int result = GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);
			// if (result != ERROR_INSUFFICIENT_BUFFER)
			//	throw new Win32Exception(result);

			// Запись в буффер
			IntPtr buffer = Marshal.AllocCoTaskMem(bytesNeeded);
			GetIpNetTable(buffer, ref bytesNeeded, false);
			//result = GetIpNetTable(buffer, ref bytesNeeded, false);
			//if (result != 0)
			//	throw new Win32Exception(result);

			// Получение кол-ва всех arp записей
			int entries = Marshal.ReadInt32(buffer);

			// Буффер, в котором будет храниться одна arp запись
			IntPtr currentBuffer = new IntPtr(buffer.ToInt64() + Marshal.SizeOf(typeof(int)));

			// Сохранение всех arp записей в массив структур
			MIB_IPNETROW[] table = new MIB_IPNETROW[entries];
			for (int index = 0; index < entries; index++)
			{
				// Вспоминаем указатели на плюсах
				table[index] = (MIB_IPNETROW)Marshal.PtrToStructure(
					// Новый указатель на позицию со сдвигов index*(кол-во байт занимаемое одной структурой)
					new IntPtr(currentBuffer.ToInt64() + (index * Marshal.SizeOf(typeof(MIB_IPNETROW)))),
					// Какого типа будет указатель
					typeof(MIB_IPNETROW));
			}

			for (int index = 0; index < entries; index++)
			{
				MIB_IPNETROW row = table[index];

				if (Skip(row))
					continue;

				IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
				string macStr = $"{row.mac0:X2}:{row.mac1:X2}:{row.mac2:X2}:{row.mac3:X2}:{row.mac4:X2}:{row.mac5:X2}";

				Console.WriteLine($"{ip,-20} {macStr}");
			}

			// Чтобы пропускать arp записи с 00 и FF
			static bool Skip(MIB_IPNETROW row) =>
					row.mac0 == 0 &&
					row.mac1 == 0 &&
					row.mac2 == 0 &&
					row.mac3 == 0 &&
					row.mac4 == 0 &&
					row.mac5 == 0 ||
					row.mac0 == 255 &&
					row.mac1 == 255 &&
					row.mac2 == 255 &&
					row.mac3 == 255 &&
					row.mac4 == 255 &&
					row.mac5 == 255;
		}
	}
}
