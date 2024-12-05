using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmojiSpammer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            while (true)
            {
                Console.Write("Message or reaction? [m/r]: ");
                string type = Console.ReadLine();

                if (type != "r" && type != "m")
                    continue;

                Console.Write("Input: ");
                string targetString = Console.ReadLine();

                bool charsOk = true;
                foreach (char c in targetString.ToLowerInvariant())
                {
                    if (!"abcdefghijklmnopqrstuvwxyz ".Contains(c))
                    {
                        charsOk = false;
                        Console.WriteLine("Bad character: \"" + c + "\"");
                        break;
                    }
                }

                if (!charsOk)
                    continue;

                if (type == "r")
                {
                    if (targetString.Length > 23)
                    {
                        Console.WriteLine("Too long, max 23 chars (you entered " + targetString.Length + ")");
                        continue;
                    }

                    Console.WriteLine("Sending in 5 seconds, click on the message you want to react to...");
                    Thread.Sleep(1000 * 5);
                    sendReactionString(targetString);
                }
                else if (type == "m")
                {
                    string output = "";
                    foreach (char c in targetString.ToLowerInvariant())
                    {
                        string charName = "" + c;
                        if (c == ' ')
                            charName = "space_";
                        output += ":" + charName + "_1:";
                    }

                    Console.WriteLine("Message: " + output);
                }
            }
        }

        static void sendReactionString(string targetString)
        {
            Dictionary<char, int> letterCounts = new Dictionary<char, int>();

            foreach (char c in targetString.ToLowerInvariant())
            {
                int num = 1;
                if (letterCounts.ContainsKey(c))
                    num += letterCounts[c];
                else
                    letterCounts[c] = 0;

                letterCounts[c]++;

                string charName = "" + c;
                if (c == ' ')
                    charName = "space_";

                rightClick();
                Thread.Sleep(500);
                send("r");
                Thread.Sleep(500);
                send(":" + charName + "_" + num + ":");
                Thread.Sleep(500);
                send("{ENTER}");
            }
        }

        static void send(string s)
        {
            // Console.WriteLine(s);
            SendKeys.SendWait(s);
        }

        static void rightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    }
}
