using Mobil.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mobil.Service
{
    public class ApiService
    {
        public static string apiurl = "https://api-kq3.conveyor.cloud/api/";
        public static async Task<bool> RegisterUser(string hairName, string email, string nickname, string password, string phone, string address, string city, string ilce, string gender, DateTime date)
        {
            var userModel = new UserClass()
            {
                Name = hairName,
                Email = email,
                Nickname = nickname,
                Password = password,
                PhoneNumber = phone,
                Address = address,
                City = city,
                Ilce = ilce,
                Gender = gender,
                Date = Convert.ToDateTime(date)
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(userModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "Account/Register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> Login(string nickname, string password)
        {
            var login = new UserClass()
            {
                Nickname = nickname,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "Account/Login", content);
            if (!response.IsSuccessStatusCode) return false;

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokenClass>(jsonResult);
            Preferences.Set("accessToken", result.access_token);
            Preferences.Set("userId", Convert.ToString(result.user_Id));
            Preferences.Set("userNames", Convert.ToString(result.user_Name));
            Preferences.Set("userNick", result.user_NickName);
            Preferences.Set("userPN", result.user_PhoneNumber);
            Preferences.Set("userAddress", result.user_Address);
            Preferences.Set("userExp", result.user_Exp);
            return true;
        }

        public static async Task<bool> RegisterCustomer(string userName, string nickName, string Email, string phone, string pass, DateTime date)
        {
            var customerModel = new CustomerClass()
            {
                UserName = userName,
                NickName = nickName,
                Email = Email,
                PhoneNumber = phone,
                Password = pass,
                Date = Convert.ToDateTime(date)
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(customerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "Customer/CuReg", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> LoginCustomer(string nickname, string password)
        {
            var login = new CustomerClass()
            {
                NickName = nickname,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "Customer/CuLog", content);
            if (!response.IsSuccessStatusCode) return false;

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokensClass>(jsonResult);
            Preferences.Set("accessToken", result.access_token);
            Preferences.Set("customerId", Convert.ToString(result.customer_Id));
            Preferences.Set("customerNames", Convert.ToString(result.customer_Name));
            Preferences.Set("customerNick", result.customer_NickName);
            Preferences.Set("customerPN", result.customer_PhoneNumber);
            Preferences.Set("customerEmail", result.customer_Email);
            return true;
        }

        public static async Task<List<CityClass>> GetCity()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "city");
            return JsonConvert.DeserializeObject<List<CityClass>>(response);
        }

        public static async Task<List<IlceClass>> GetIlceler()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "ilce");
            return JsonConvert.DeserializeObject<List<IlceClass>>(response);
        }

        public static async Task<List<IlceClass>> GetIlces(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "ilce/" + id);
            return JsonConvert.DeserializeObject<List<IlceClass>>(response);
        }

        public static async Task<List<IlceClass>> GetIlce()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "ilce");
            return JsonConvert.DeserializeObject<List<IlceClass>>(response);
        }

        public static async Task<List<GenderClass>> GetGender()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "gender");
            return JsonConvert.DeserializeObject<List<GenderClass>>(response);
        }

        public static async Task<List<UserClass>> GetUsers()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "users");
            return JsonConvert.DeserializeObject<List<UserClass>>(response);
        }

        public static async Task<UserClass> GetUser(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "users/" + id);
            return JsonConvert.DeserializeObject<UserClass>(response);
        }

        public static async Task<List<TimeClass>> GetTime()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "time");
            return JsonConvert.DeserializeObject<List<TimeClass>>(response);
        }

        public static async Task<List<ReservationClass>> GetReservations()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "reservation");
            return JsonConvert.DeserializeObject<List<ReservationClass>>(response);
        }

        public static async Task<ReservationClass> GetReservation(int Id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "reservation/" + Id);
            return JsonConvert.DeserializeObject<ReservationClass>(response);
        }

        public static async Task<List<ReservationClass>> GetKIdReservation(int KId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "rs/" + KId);
            return JsonConvert.DeserializeObject<List<ReservationClass>>(response);
        }

        public static async Task<List<ReservationClass>> GetCIdReservation(int CId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "cs/" + CId);
            return JsonConvert.DeserializeObject<List<ReservationClass>>(response);
        }

        public static async Task<List<CommentClass>> GetComment(int KId)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "comment/" + KId);
            return JsonConvert.DeserializeObject<List<CommentClass>>(response);
        }

        public static async Task<List<CommentsClass>> GetComments()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "comments");
            return JsonConvert.DeserializeObject<List<CommentsClass>>(response);
        }

        public static async Task<List<OperationClass>> GetOperation(string g)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "operation/"+g);
            return JsonConvert.DeserializeObject<List<OperationClass>>(response);
        }

        public static async Task<List<HOperationClass>> GetHOperation(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "hoperation/" + id);
            return JsonConvert.DeserializeObject<List<HOperationClass>>(response);
        }

        public static async Task<List<TimesClass>> GetTimes(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "times/" + id);
            return JsonConvert.DeserializeObject<List<TimesClass>>(response);
        }

        public static async Task<List<HOperationClass>> GetLHO(int id)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "lho/" + id);
            return JsonConvert.DeserializeObject<List<HOperationClass>>(response);
        }

        public static async Task<List<UserClass>> SearchUser(string name, string city, string ilce, string gender)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(apiurl + "user?name=" + name + "&city=" + city + "&ilce="+ ilce + "&gender="+ gender +"");
            return JsonConvert.DeserializeObject<List<UserClass>>(response);
        }

        public static async Task<ReservationClass> PostReservation(ReservationClass reservation)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(reservation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "reservation", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReservationClass>(jsonResult);
        }

        public static async Task<CommentClass> PostComment(CommentClass comment)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "comment", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CommentClass>(jsonResult);
        }

        public static async Task<HOperationClass> PostHOperation(HOperationClass hOperation)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(hOperation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "hoperation", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HOperationClass>(jsonResult);
        }

        public static async Task<TimesClass> PostTime(TimesClass times)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(times);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(apiurl + "times", content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TimesClass>(jsonResult);
        }

        public static async Task<bool> PutUser(int id, string hairName, string nickname, string phone, string address, string city, string ilce, string gender)
        {
            var userModel = new UserClass()
            {
                Id = id,
                Name = hairName,
                Nickname = nickname,
                PhoneNumber = phone,
                Address = address,
                City = city,
                Ilce = ilce,
                Gender = gender
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(userModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(apiurl + "users/" + id, content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> CancelReservation(int id, int kid, int cid, string cName, string operation, string time, DateTime date)
        {
            var resModel = new ReservationClass()
            {
                Id = id,
                KId = kid,
                CId = cid,
                CName = cName,
                Operation = operation,
                Time = time,
                Date = Convert.ToDateTime(date)
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(resModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(apiurl + "reservation/" + id, content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> PutHoperation(int id, int kid, string operation, string price)
        {
            var opModel = new HOperationClass()
            {
                Id = id,
                KId = kid,
                Operation = operation,
                Price = price
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(opModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(apiurl + "hoperation/" + id, content);
            return response.IsSuccessStatusCode;
        }

        /*public static async Task<bool> PutCustomer(int id, string user, string nick, string email, string phone)
        {
            var cusModel = new CustomerClass()
            {
                Id = id,
                UserName = user,
                NickName = nick,
                Email = email,
                PhoneNumber = phone
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(cusModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(apiurl + "customer/" + id, content);
            return response.IsSuccessStatusCode;
        }*/
    }
}
