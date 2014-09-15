using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score.Fake
{
    class FakeLocalUser : ILocalUser
    {
        public const string UserID = "fake";
        public const string UserName = "Alena";
        
        public void Authenticate(Action<bool> callback)
        {
            callback(true);
        }

        public void LoadFriends(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public bool authenticated
        {
            get { return true; }
        }

        public IUserProfile[] friends
        {
            get { throw new NotImplementedException(); }
        }

        public bool underage
        {
            get { throw new NotImplementedException(); }
        }

        public string id
        {
            get { return UserID; }
        }

        public UnityEngine.Texture2D image
        {
            get { throw new NotImplementedException(); }
        }

        public bool isFriend
        {
            get { throw new NotImplementedException(); }
        }

        public UserState state
        {
            get { throw new NotImplementedException(); }
        }

        public string userName
        {
            get { return UserName; }
        }
    }
}
