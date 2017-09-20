using System.Collections.Generic;
using System.Linq;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class UserManager 
    {
        private DatabaseInterface _db;
        private List<User> _users = new List<User>();
        
        public UserManager(DatabaseInterface db)
        {
            _db = db;
        }

        // accept instance of user and add to database, return id of new user
        public int CreateUser(User user)
        {
            user.UserId = _db.Insert($"INSERT INTO user VALUES (null, '{user.Name}', '{user.Password}', null, '{user.Email}'");
            _users.Add(user);
            return user.UserId;
        }

        // list all users in the system
        public List<User> ListUsers()
        {
            _users.Clear();
            _db.Query("SELECT * FROM user", (SqliteDataReader reader) =>{
                while(reader.Read())
                {
                    _users.Add(new User(){
                        UserId = reader.GetInt32(0),
                        Name =  reader[1].ToString(),
                        Password = reader[2].ToString(),
                        DateCreated = reader.GetDateTime(3),
                        Email = reader[4].ToString()
                    });
                }
            });
            return _users;
        }

        // accept user, replace user with that userId in database
        public void UpdateUser(User user)
        {
            _users.Remove(_users.Find(u => u.UserId == user.UserId));
            _users.Add(user);
            _db.Update($"UPDATE user SET name = {user.Name}, password = {user.Password}, email = {user.Email} WHERE user.userid = {user.UserId}");
        }

        // get a single user by userId
        public User GetUser(int userId)
        {
            return _users.SingleOrDefault(u => u.UserId == userId);
        }

        // send a message to the given user with their password
        public void PasswordReminder(int userId) 
        {
            Program.Warning("The password is " + _users.SingleOrDefault(u => u.UserId == userId).Password); 
        }
    }
}