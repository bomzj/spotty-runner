using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score.Facebook
{
    class FacebookLocalUser : ILocalUser
    {
        #region Facebook Callbacks

        private void AuthenticationResultCallback(Action<bool> callback)
        {
            callback(authenticated);
        }

        #endregion

        public void Authenticate(Action<bool> callback)
        {
            // Call facebook auth
            authenticated = true;
            AuthenticationResultCallback(callback);
        }

        public void LoadFriends(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public bool authenticated
        {
            get;
            private set;
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
            get { throw new NotImplementedException(); }
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
            get { throw new NotImplementedException(); }
        }

        public void Logout()
        {
            authenticated = false;
        }
    }
}
