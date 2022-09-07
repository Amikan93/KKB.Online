using KBB.Online.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKB.Online
{
    internal class Program
    {
        static string Path = @"C:\Users\ФедотовА\Desktop\KKB.Online\MyData.db";
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            UserService userService = new UserService(Path);

            string message = "";

            Console.ForegroundColor = ConsoleColor.White;

            try
            {
               

                Console.WriteLine("Добро пожаловать в Интернет Банкинг");
                Console.WriteLine("");
                Console.WriteLine("Выберите пункты меню: ");
                Console.WriteLine("1. Авторизация");
                Console.WriteLine("2. Регистрация");
                Console.WriteLine("3. Выход");

                int ch = Convert.ToInt32(Console.ReadLine());

                switch (ch)
                {
                    case 1:
                        {
                            //string IIN;s
                            //string Password;
                            Console.Clear();
                            Console.Write("Введите ИНН: ");
                            string IIN = Console.ReadLine();
                            Console.Write("Введите пароль: ");
                            string Password = Console.ReadLine();
                            personal_data users = userService.GetUser(IIN, Password);
                            if (users == null)
                            {
                                Console.Write("ИНН и пароль введен не правильно!");
                            }
                            else
                            {
                                Console.Write("Добро пожаловать {0}", users.FirstName);
                            }

                        }
                        break;

                    case 2:
                        {
                            personal_data user = new personal_data();
                            user.Accounts = null;
                            user.AddressOfRegistation = null;
                            user.BirthDate = new DateTime(1988, 01, 11);
                            user.IIN = "880111300392";
                            user.LastName = "Yevgeniy";
                            user.FirstName = "Gertsen";
                            user.Password = "123";
                            user.PhoneNumber = "+7 777 209 43 43";
                            user.Gender = "M";
                        }
                        break;
                    default:
                        throw new Exception("Необходимо выбрать пункт меню");
                }
            }
            catch
            {

            }

            Console.WriteLine("Введите Ваш ИНН");
            string inn = Console.ReadLine();


            if (userService.GetUserData(inn, out message))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
