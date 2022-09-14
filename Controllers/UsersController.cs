using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserInfo.Model;

namespace UserInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        //[EnableCors("AllowOrigin")]
        [HttpGet(Name = "GetUsers")]
        public string GetUserDetails()
        {
            try
            {
                var userDetailJson = System.IO.File.ReadAllText("userdetails.json");
                return userDetailJson;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost(Name = "AddUsers")]
        public string AddUserDetails(User user)
        {
            try
            {
                var userDetailJson = System.IO.File.ReadAllText("userdetails.json");
                var jsonObj = JObject.Parse(userDetailJson);
                var userArray = jsonObj.GetValue("users") as JArray;
                user.Id = userArray.Count + 1;
                var userJson = new JavaScriptSerializer().Serialize(user);
                var newCompany = JObject.Parse(userJson);
                userArray.Add(newCompany);

                jsonObj["users"] = userArray;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                       Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText("userdetails.json", newJsonResult);
                return "";
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut(Name = "UpdateUuser/{id}")]
        public void UpdateUuser(int id, User user)
        {
            string json = System.IO.File.ReadAllText("userdetails.json");

            try
            {
                var jObject = JObject.Parse(json);
                JArray userArray = (JArray)jObject["users"];

                if (user.Id > 0)
                {

                    foreach (var userdetail in userArray.Where(obj => obj["id"].Value<int>() == id))
                    {
                        userdetail["name"] = !string.IsNullOrEmpty(user.name) ? user.name : "";
                        userdetail["username"] = !string.IsNullOrEmpty(user.username) ? user.username : "";
                        userdetail["phone"] = !string.IsNullOrEmpty(user.phone) ? user.phone : "";
                        userdetail["website"] = !string.IsNullOrEmpty(user.website) ? user.website : "";
                    }

                    jObject["users"] = userArray;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText("userdetails.json", output);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Update Error : " + ex.Message.ToString());
            }
        }

        [HttpDelete(Name = "DeleteUser/{id}")]
        public void DeleteUser(int id)
        {
            var json = System.IO.File.ReadAllText("userdetails.json");
            try
            {
                var jObject = JObject.Parse(json);
                JArray usersArrays = (JArray)jObject["users"];


                var companyName = string.Empty;
                var userToDelete = usersArrays.FirstOrDefault(obj => obj["id"].Value<int>() == id);

                usersArrays.Remove(userToDelete);

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText("userdetails.json", output);

            }
            catch (Exception)
            {

                throw;
            }
        }

        //[HttpGet(Name = "GetUserById/{id}")]
        [Route("GetUserById/{id}")]
        [HttpGet]
        public string GetUserById(int id)
        {

            var user = string.Empty;
            string json = System.IO.File.ReadAllText("userdetails.json");
            try
            {
                var jObject = JObject.Parse(json);
                JArray userArray = (JArray)jObject["users"];

                foreach (var userdetail in userArray.Where(obj => obj["id"].Value<int>() == id))
                {
                    user = userdetail.ToString();
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine("Update Error : " + ex.Message.ToString());
            }
            return user;
        }

    }
}
