using AbiturientTGBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbiturientTGBot.Service
{
    public class DBService
    {
        ApplicationContext db = new ApplicationContext();
        MessageService msgService = new MessageService();

        // Code with users
        public async void SetUserState(long id, string state)
        {
            User user = db.Users.Where(u => u.UserId == id).First();
            user.State = state;

            db.Users.Update(user);
            await db.SaveChangesAsync();
        }

        public bool IsNewUser(long id)
        {
            try
            {
                // if there's not users with 'id' it throws exception
                db.Users.Where(u => u.UserId == id).First();
            }
            catch (Exception ex)
            {
                return true;
            }

            return false;
        }

        public async void InsertNewUser(long id, string username)
        {
            if (IsNewUser(id) == false)
            {
                return;
            }

            User newUser = new User
            {
                UserId = id,
                State = "newUser",
                Username = username
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();
        }

        public string GetUserState(long id)
        {
            string state;

            if (IsNewUser(id) == true)
            {
                state = "notUser";
                return state;
            }

            state = db.Users.Where(u => u.UserId == id)
                .Select(u => u.State)
                .First();

            return state;
        }

        // Code with specializations

        public Specialization[] GetSpecializations()
        {
            return db.Specializations.ToArray();
        }

        public Specialization[] GetMidSpecializations()
        {
            return db.Specializations.Where(s => s.ClassRequired == 11).ToArray();
        }

        public Specialization[] GetBaseSpecializations()
        {
            return db.Specializations.Where(s => s.ClassRequired == 9).ToArray();
        }

        public string GetSpecInfo(string specQualification, int classReq)
        {
            Specialization specialization = db.Specializations.Where(s => s.Qualification == specQualification)
                .Where(s => s.ClassRequired == classReq).First();

            return msgService.CreateInfoMessage(specialization);

            //return db.Specializations.Where(s => s.Qualification == specQualification)
            //    .Where(s => s.ClassRequired == classReq)
            //    .Select(s => s.Name).First();
        }
    }
}
