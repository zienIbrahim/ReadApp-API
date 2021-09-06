using APIAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace APIAngular.DataLayer
{

    public interface IAuthRepos
    {

        Task<User> Register(User user , string Password);


        Task<User> Login(string username, string Password);

        Task<bool> UserExist(string username);


    }


}
