using System.Diagnostics;
using System;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace NAudioApp
{
    class Program
    {
        // TODO: add some compression
        // TODO: add remote player
        // TODO: add discovery/connection
        // TODO: add automatic muting of host speakers
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        // static void Main(string[] args)
        // {
        //    Program.start();
        //    Thread.Sleep(2000);
        //    Program.stop();
        // }

        public static void start()
        {
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
        }

        public static void stop()
        {
            record("save recsound C:\\Users\\PiyuNir\\hackerton-project\\test.wav", "", 0, 0);
            record("close recsound", "", 0, 0);
        }
    }
}