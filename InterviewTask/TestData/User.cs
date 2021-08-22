using System;
using InterviewTask.Helpers;

namespace InterviewTask.TestData
{
    [Serializable]
    public class User
    {
        [NonSerialized]
        private StringHelper _stringHelper = new StringHelper();
        
        [NonSerialized]
        public uint id;
        
        [NonSerialized]
        public string created_at;
        
        [NonSerialized]
        public string updated_at;
        
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public User()
        {
            username = _stringHelper.GetRandomString(6);
            email = _stringHelper.GetRandomEmail(5);
            password = _stringHelper.GetRandomString(5);
        }

        public User(string username, string email, string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }
}

