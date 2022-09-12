using LiteDB;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBB.Online.BLL
{
    public class UserService
    {
        public UserService(string Path)
        {
            this.Path = Path;
        }

        private string Path { get; set; }
        public List<personal_data> Users { get; set; }


        public bool CreateUser(personal_data user, out string message)
        {
            try
            {
                using (var db = new LiteDatabase(Path))
                {
                    var users = db.GetCollection<personal_data>("Users");

                    if (GetUser(user.IIN, user.Password) != null)
                    {
                        message = "Пользователь с ИИН: "+ user.IIN+" уже зарегистрирован!";
                        return false;
                    }
                    else
                    {

                        users.Insert(user);
                    }
                }

                message = "Successfully";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }            
        } 
        
        public personal_data GetUser(string iin, string password)
        {
            personal_data user = null;
            try
            {
                using (var db = new LiteDatabase(Path))
                {
                    var users = db.GetCollection<personal_data>("Users");
                    user = users.FindOne(f => f.IIN == iin);
                }
            }
            catch 
            {
                user = null;
            }

            return user;
        }

        public bool GetUserData(string iin, out string message)
        {
            try
            {
                var restClient = new RestClient("https://meteor.almaty.e-orda.kz/"); //подключили базу данных

                var request = new RestRequest("/ru/api-form/load-info-by-iin/?iin=" + iin + "&params[city_check] = true");
                RestResponse response = restClient.Execute(request);

                if(response.ErrorException!=null) //получили данные
                {
                    throw new Exception("При запросе возникла ошибка: " + response.ErrorMessage, response.ErrorException);
                }
                else if(response.Content == "[]")
                {
                    throw new Exception("По введенному ИИН данных нет");
                }
                else 
                {
                    User user = JsonConvert.DeserializeObject<User>(response.Content);

                    return CreateUser(user.personal_data, out message);  //метод для добавяления в базу данных
                   
                }

               //User personalData = JsonConvert.DeserializeObject<User>(response.Content);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
