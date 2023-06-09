﻿using AbiturientTGBot.Models;
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

        public void SetUserStateByTableId(int id, string state)
        {
            User user = db.Users.Where(u => u.Id == id).First();
            user.State = state;

            db.Users.Update(user);
            db.SaveChanges();
        }

        public void SetUserState(long id, string state)
        {
            User user = db.Users.Where(u => u.UserId == id).First();
            user.State = state;

            db.Users.Update(user);
            db.SaveChanges();
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

        public void InsertNewUser(long id, string username)
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
            db.SaveChanges();
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

        public User GetUser(long id)
        {
            return db.Users.Where(u => u.UserId == id).First();
        }

        // Code with specializations

        public int GetSpecializationsCount()
        {
            return db.Specializations.Count();
        }

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

        public string GetSpecInfo(string userMsg)
        {
            string[] words = userMsg.Split(' ');

            int classReq = Convert.ToInt32(words[3]);
            string spec = words[0];

            Specialization specialization = db.Specializations.Where(s => s.ClassRequired == classReq)
                .Where(s => s.Qualification == spec).First();

            return msgService.CreateInfoMessage(specialization);
        }

        public Specialization GetSpecizalization(string userMsg)
        {
            string[] words = userMsg.Split(' ');

            int classReq = Convert.ToInt32(words[3]);
            string spec = words[0];

            Specialization specialization = db.Specializations.Where(s => s.ClassRequired == classReq)
                .Where(s => s.Qualification == spec).First();

            return specialization;
        }

        public string GetSpecName(int id)
        {
            string specName = db.Specializations.Where(s => s.SpecId == id)
                .Select(s => s.Name)
                .ToList()
                .First();

            return specName;
        }

        public int GetSpecClassReq(int id)
        {
            int classReq = db.Specializations.Where(s => s.SpecId == id)
                .Select(s => s.ClassRequired)
                .ToList()
                .First();

            return classReq;
        }

        // Abiturient info

        public bool IsNewAbiturient(int id)
        {
            try
            {
                db.Abiturients.Where(a => a.UserId == id).First();
            }
            catch (Exception ex)
            {
                return true;
            }

            return false;
        }

        public void CreateNewAbiturient(Abiturient abiturient)
        {
            if (IsNewAbiturient(abiturient.UserId) == false)
                return;

            db.Abiturients.Add(abiturient);
            db.SaveChanges();
        }
        public void UpdateAbiturient(Abiturient abiturient)
        {
            db.Abiturients.Update(abiturient);
            db.SaveChanges();
        }
        public Abiturient GetAbiturient(int id)
        {
            try
            {
                return db.Abiturients.Where(a => a.UserId == id).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<Abiturient> GetAllAbiturients()
        {
            IEnumerable<Abiturient> _abiturients = db.Abiturients.ToList();
            return _abiturients;
        }
    }
}
