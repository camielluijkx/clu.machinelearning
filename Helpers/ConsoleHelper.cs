using System;
using System.Collections.Generic;
using System.Linq;

namespace clu.machinelearning.Helpers
{
    public static class ConsoleHelper
    {
        public delegate void MenuItemEvent();

        public class MenuItem
        {
            public int Index { get; set; }

            public string Text { get; set; }

            public MenuItemEvent Event { get; set; }

            public MenuItem(int itemIndex, string itemText, MenuItemEvent itemEvent)
            {
                Index = itemIndex;
                Text = itemText;
                Event = itemEvent;
            }
        }

        private static int CharToInt(char input)
        {
            int result = -1;

            if (input >= 48 && input <= 57)
            {
                result = input - '0';
            }

            return result;
        }

        public static void Initialize()
        {
            Console.WriteLine("Initializing...");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("****************************************************************");
            Console.WriteLine($"******************* Machine Learning Console *******************");
            Console.WriteLine("****************************************************************");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.BackgroundColor = ConsoleColor.Blue;

            Console.WriteLine("\t\t\t\t\t\t\t\t");
            Console.WriteLine(@"                 _________ .____     ____ ___                   ");
            Console.WriteLine(@"                 \_   ___ \|    |   |    |   \                  ");
            Console.WriteLine(@"                 /    \  \/|    |   |    |   /                  ");
            Console.WriteLine(@"                 \     \___|    |___|    |  /                   ");
            Console.WriteLine(@"                  \______  /_______ \______/                    ");
            Console.WriteLine(@"                         \/        \/                           ");
            Console.WriteLine("\t\t\t\t\t\t\t\t");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void ShowMenu(List<MenuItem> menuItems)
        {
            Console.WriteLine("Hello {0}, what would you like to do?", Environment.UserName);

            char choice = ' ';
            while (choice != '0')
            {
                Console.WriteLine("");
                Console.WriteLine("[0] Exit");

                menuItems.ForEach(mi => Console.WriteLine($"[{mi.Index}] {mi.Text}"));

                ConsoleKeyInfo consoleKey = Console.ReadKey(true);
                choice = consoleKey.KeyChar;

                var menuItem = menuItems.SingleOrDefault(mi => mi.Index == CharToInt(choice));
                menuItem?.Event.Invoke();
            }

            Environment.Exit(0);
        }
    }
}
