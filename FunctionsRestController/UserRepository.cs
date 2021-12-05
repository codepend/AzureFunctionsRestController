using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionsRestController
{
    public class UserRepository
    {
        private List<UserModel> users;
        public UserRepository()
        {
            SetupUsers();
        }

        private void SetupUsers()
        {
            users = new List<UserModel>();
            users.Add(new UserModel { FirstName = "Jim",    LastName = "Shoe", Id = users.Count + 1 , CreatedDate = DateTime.UtcNow });
            users.Add(new UserModel { FirstName = "Jane",   LastName = "Doe", Id = users.Count + 1 , CreatedDate = DateTime.UtcNow });
            users.Add(new UserModel { FirstName = "Captian",LastName = "Picard", Id = users.Count + 1 , CreatedDate = DateTime.UtcNow });
            users.Add(new UserModel { FirstName = "Fisty",  LastName = "McGee", Id = users.Count + 1 , CreatedDate = DateTime.UtcNow });
            users.Add(new UserModel { FirstName = "Bobble", LastName = "Head", Id = users.Count + 1 , CreatedDate = DateTime.UtcNow });

        }

        public Task<List<UserModel>> GetUsers()
        {
            return Task.FromResult(users);
        }

        public Task<UserModel> GetUserById(int id)
        {
            var result = users.SingleOrDefault(x=>x.Id == id);
            if (result == null) return Task.FromResult(new UserModel());
            return Task.FromResult(result);
        }
        public Task UpdateUser(UserModel user)
        {
            var result = users.SingleOrDefault(x => x.Id == user.Id);
            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
            }
            return Task.CompletedTask;
        }
        public Task<UserModel> AddUser(UserModel user)
        {
            user.Id = users.Count +1;
            users.Add(user);
            return Task.FromResult(user);
        }
        public Task Delete(int id)
        {
            var user = users.SingleOrDefault(x => x.Id ==id);
            if (user != null) users.Remove(user);
            return Task.CompletedTask;
        }
    }
}
