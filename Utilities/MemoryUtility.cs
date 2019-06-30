using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurtzGlide.Utilities
{
    public class MemoryUtility
    {
        private Process process;
        private VAMemory vam;

        public MemoryUtility(string processName)
        {
            this.vam = new VAMemory(processName);
            this.process = Process.GetProcessesByName(processName).FirstOrDefault();
        }

        public IntPtr GetAddressWithSigScan(int[] offsetArr)
        {
            long lTime;
            var sigScan = new SigScanSharp(this.process.Handle, this.process.MainModule);
            IntPtr baseAddress = (IntPtr)sigScan.FindPattern("A0 0F 00 02 00 00 00 00 ? ? ? ? ? ? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B4 42", out lTime) + 0x8;
            return GetPointer(baseAddress, offsetArr);
        }

        private IntPtr GetPointer(IntPtr baseAddress, int[] offsetArr)
        {
            if (offsetArr.Length == 0)
                return (IntPtr)vam.ReadInt64(baseAddress);

            IntPtr pointer = IntPtr.Add((IntPtr)vam.ReadInt64(baseAddress), offsetArr[0]);
            for (int i = 1; i < offsetArr.Length; i++)
                pointer = IntPtr.Add((IntPtr)vam.ReadInt64(pointer), offsetArr[i]);

            return pointer;
        }

        public T ReadValue<T>(IntPtr baseAddress, int[] offsetArr)
        {
            var pointer = GetPointer(baseAddress, offsetArr);
            if (typeof(T).Equals(typeof(int)))
            {
                return (T)Convert.ChangeType(vam.ReadInt32(pointer), typeof(T));
            }
            else if (typeof(T).Equals(typeof(long)))
            {
                return (T)Convert.ChangeType(vam.ReadInt64(pointer), typeof(T));
            }
            else if (typeof(T).Equals(typeof(double)))
            {
                return (T)Convert.ChangeType(vam.ReadDouble(pointer), typeof(T));
            }
            else if (typeof(T).Equals(typeof(float)))
            {
                return (T)Convert.ChangeType(vam.ReadFloat(pointer), typeof(T));
            }
            return default;
        }

        public void WriteValue<T>(IntPtr baseAddress, T value, int[] offsetArr)
        {
            var pointer = GetPointer(baseAddress, offsetArr);
            Type type = value.GetType();
            if (type.Equals(typeof(int)))
            {
                vam.WriteInt32(pointer, Convert.ToInt32(value));
            }
            else if (type.Equals(typeof(float)))
            {
                vam.WriteFloat(pointer, (float)Convert.ToDouble(value));
            }
            else if (type.Equals(typeof(double)))
            {
                vam.WriteDouble(pointer, Convert.ToDouble(value));
            }
        }    
}
}
