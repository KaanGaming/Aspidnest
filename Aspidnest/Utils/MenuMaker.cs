using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;

namespace Aspidnest.Utils
{
    public class MenuMaker
    {
        public MenuMaker()
        {

        }

        public KeyCode GetKeybind(int id)
        {
            return id switch
            {
                0 => KeyCode.A,
                1 => KeyCode.B,
                2 => KeyCode.C,
                3 => KeyCode.D,
                4 => KeyCode.E,
                5 => KeyCode.F,
                6 => KeyCode.G,
                7 => KeyCode.H,
                8 => KeyCode.I,
                9 => KeyCode.J,
                10 => KeyCode.K,
                11 => KeyCode.L,
                12 => KeyCode.M,
                13 => KeyCode.N,
                14 => KeyCode.O,
                15 => KeyCode.P,
                16 => KeyCode.Q,
                17 => KeyCode.R,
                18 => KeyCode.S,
                19 => KeyCode.T,
                20 => KeyCode.U,
                21 => KeyCode.V,
                22 => KeyCode.W,
                23 => KeyCode.X,
                24 => KeyCode.Y,
                25 => KeyCode.Z,
                _ => KeyCode.None
            };
        }

        public int IdFromKeybind(KeyCode val)
        {
            return val switch
            {
                KeyCode.A => 0,
                KeyCode.B => 1,
                KeyCode.C => 2,
                KeyCode.D => 3,
                KeyCode.E => 4,
                KeyCode.F => 5,
                KeyCode.G => 6,
                KeyCode.H => 7,
                KeyCode.I => 8,
                KeyCode.J => 9,
                KeyCode.K => 10,
                KeyCode.L => 11,
                KeyCode.M => 12,
                KeyCode.N => 13,
                KeyCode.O => 14,
                KeyCode.P => 15,
                KeyCode.Q => 16,
                KeyCode.R => 17,
                KeyCode.S => 18,
                KeyCode.T => 19,
                KeyCode.U => 20,
                KeyCode.V => 21,
                KeyCode.W => 22,
                KeyCode.X => 23,
                KeyCode.Y => 24,
                KeyCode.Z => 25,
                _ => 0
            };
        }

        public float GetFloat(int id)
        {
            return id switch
            {
                0 => 0.25f,
                1 => 0.5f,
                2 => 0.75f,
                3 => 1f,
                4 => 1.25f,
                5 => 1.5f,
                6 => 1.75f,
                7 => 2f,
                _ => 1f
            };
        }

        public int IdFromFloat(float val)
        {
            return val switch
            {
                0.25f => 0,
                0.5f => 1,
                0.75f => 2,
                1 => 3,
                1.25f => 4,
                1.5f => 5,
                1.75f => 6,
                2 => 7,
                _ => 3
            };
        }

        public IMenuMod.MenuEntry KeybindEntry(string name, string description, Action<int> saver, Func<int> loader)
        {
            return new IMenuMod.MenuEntry(
                name, new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" },
                description, saver, loader
                );
        }

        public IMenuMod.MenuEntry FloatEntry(string name, string description, Action<int> saver, Func<int> loader)
        {
            return new IMenuMod.MenuEntry(
                name, new string[] { "0.25x", "0.5x", "0.75x", "1x", "1.25x", "1.5x", "1.75x", "2x" },
                description, saver, loader
                );
        }

        public IMenuMod.MenuEntry Entry(string name, string description, Action<int> saver, Func<int> loader, params string[] choices)
        {
            return new IMenuMod.MenuEntry(
                name, choices,
                description, saver, loader
                );
        }

        public string[] GenerateInts(int min, int max)
        {
            List<string> rv = new List<string>();
            for (int i = min; i <= max; i++)
                rv.Add(i.ToString());

            return rv.ToArray();
        }

        public IMenuMod.MenuEntry IntEntry(string name, string description, Action<int> saver, Func<int> loader, int min, int max)
        {
            return new IMenuMod.MenuEntry(
                name, GenerateInts(min, max),
                description, saver, loader
                );
        }

        public IMenuMod.MenuEntry Empty(string name = " ", string description = " ")
        {
            return new IMenuMod.MenuEntry(
                name, new string[] { },
                description, v => { }, () => 0
                );
        }
    }
}
