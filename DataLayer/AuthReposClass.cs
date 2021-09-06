using APIAngular.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace APIAngular.DataLayer
{


    public class AuthReposClass : IAuthRepos
    {


        private readonly DbZContext db;


        public AuthReposClass(DbZContext context)
        {

            db = context; 
        }




        public async Task<User> Login(string username, string Password)
        {

            var user = await db.User.FirstOrDefaultAsync(a =>a.Username == username);

            if (user == null) return null;

            if (!VerifyPassHash(Password , user.SaltePass , user.PassHash)) return null;

            return user;


        }



        private bool VerifyPassHash(string password, byte[] saltePass, byte[] passHash)
        {


            using (var Hmac = new System.Security.Cryptography.HMACSHA512(saltePass))
            {

                var ComputedHashPass = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < ComputedHashPass.Length; i++)
                {

                    if (ComputedHashPass[i] != passHash[i]) return false;

                }


                return true;

            }

            throw new NotImplementedException();

        }

         

        //---- Register User In DB ------------------------------------------------
        public async Task<User> Register(User user, string Password)
        {

            byte[] HashPassword, PasswordSalt;


            GenPasswordHash(Password , out HashPassword, out PasswordSalt);

            user.SaltePass = PasswordSalt;

            user.PassHash = HashPassword;

            await db.User.AddAsync(user);

            await db.SaveChangesAsync();

            return user;

        }



        private void GenPasswordHash(string password, out byte[] hashPass, out byte[] passSalt)
        {

            using (var Hmac = new  System.Security.Cryptography.HMACSHA512()) 
            {
                passSalt = Hmac.Key;

                hashPass = Hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }



        public async Task<bool> UserExist(string username)
        {

            if (await db.User.AnyAsync(a => a.Username == username)) return true;
           
            return false;


        }



    }


}
