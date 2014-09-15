using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Score.Fake
{
    class FakeUserProfile : IUserProfile
    {
        public FakeUserProfile(string id, string userName)
        {
            this.id = id;
            this.userName = userName;
        }


        public string id
        {
            get;
            set;
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
            get;
            set;
        }
    }
}
