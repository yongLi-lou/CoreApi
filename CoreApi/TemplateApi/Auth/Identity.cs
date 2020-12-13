using Qj.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TemplateApi.Auth
{
    public class Identity : ClaimsIdentity
    {
        private QjCurrentUser _qjUser;

        /// <summary>
        /// 当前登陆用户
        /// </summary>
        public QjCurrentUser CurrentUser
        {
            get
            {
                //if (_qjUser == null)
                //{
                //    var userString = string.Empty;

                //    foreach (var c in Claims)
                //    {
                //        if (c.Type == ClaimTypes.Name)
                //        {
                //            userString = c.Value;
                //        }
                //    }

                //    _qjUser = JsonConvert.DeserializeObject<QjCurrentUser>(userString);
                //}
                return _qjUser;
            }
        }

        public Identity(QjCurrentUser model)
        {
            _qjUser = model;
        }

        public Identity()
        {
        }

        public Identity(string authenticationType) : base(authenticationType)
        {
        }

        public Identity(IEnumerable<Claim> claims) : base(claims)
        {
        }

        public Identity(IIdentity identity) : base(identity)
        {
        }

        public Identity(IIdentity identity, IEnumerable<Claim> claims) : base(identity, claims)
        {
        }

        public Identity(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType)
        {
        }

        public Identity(string authenticationType, string nameType, string roleType) : base(authenticationType, nameType, roleType)
        {
        }

        public Identity(IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(claims, authenticationType, nameType, roleType)
        {
        }

        public Identity(IIdentity identity, IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(identity, claims, authenticationType, nameType, roleType)
        {
        }

        protected Identity(SerializationInfo info) : base(info)
        {
        }

        protected Identity(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}