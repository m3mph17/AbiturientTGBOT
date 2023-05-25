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

        // Code with users
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
                State = 0,
                Username = username
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();
        }

        public int GetUserState(long id)
        {
            int state = 0;

            if (IsNewUser(id) == true)
            {
                state = 0;
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
    }
}
