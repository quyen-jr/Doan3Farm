using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FairyField.Logic
{
    public class UserData : Singleton<UserData>
    {
        public string UserAccessToken;
        public string UserRefreshToken;
        public string ResendToken;
        private string Username;

        protected override void Awake() 
        {
            base.Awake();

            DontDestroyOnLoad(this);
        }

        public void SetUsername(string _username) 
        {
            Username = _username;
        }
        public string GetUsername() => Username;
    }
}

