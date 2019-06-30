using KurtzGlide.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurtzGlide
{
    public class LocalPlayer
    {
        private MemoryUtility memory;
        private IntPtr baseAddress;

        public LocalPlayer(MemoryUtility memory)
        {
            this.memory = memory;                          
        }

        #region Offsets
        private struct Offsets
        {
            internal const int AttackSpeed = 0x17E0;
            internal const int CastSpeed = 0x17E4;
            internal const int MoveSpeed = 0x17E8;
            internal const int DamageMultiplier = 0x17EC;
            internal const int Health = 0x1884;
            internal const int RegenHealth = 0x1888;
            internal const int Stamina = 0x1910;
            internal const int Karma = 0x1914;
            internal const int Mana = 0x1918;
        }
        #endregion

        public float AttackSpeed
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.AttackSpeed });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.AttackSpeed });
        }
        public float CastSpeed
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.CastSpeed });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.CastSpeed });
        }
        public float MoveSpeed
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.MoveSpeed });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.MoveSpeed });
        }
        public float DamageMultiplier
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.DamageMultiplier });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.DamageMultiplier });
        }
        public float Health
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.Health });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.Health });
        }
        public float RegenHealth
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.RegenHealth });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.RegenHealth });
        }
        public float Stamina
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.Stamina });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.Stamina });
        }
        public float Karma
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.Karma });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.Karma });
        }
        public float Mana
        {
            get => memory.ReadValue<float>(this.baseAddress, new int[] { Offsets.Mana });
            set => memory.WriteValue<float>(this.baseAddress, value, new int[] { Offsets.Mana });
        }
        public bool IsFound
        {
            get => memory.ReadValue<int>(this.baseAddress, new int[] { 0x0 }) == 844814848;
        }

        public void FindBaseAddress()
        {
            try { this.baseAddress = memory.GetAddressWithSigScan(new int[] { 0x30, 0x3d0, 0x460 }); }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
    }
}
