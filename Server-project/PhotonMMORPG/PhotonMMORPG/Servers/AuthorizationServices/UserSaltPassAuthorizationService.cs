using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MGF.Domain;
using MGF.Mappers;
using Servers.Interfaces;
using SHA512Managed = System.Security.Cryptography.SHA512Managed;

namespace Servers.AuthorizationServices
{
    public class UserSaltPassAuthorizationService : IAuthorizationService
    {
        public ReturnCode IsAuthorized(out User user, params string[] authorizationParameters)
        {
            user = null;
            if (authorizationParameters.Length != 2)
            {
                return ReturnCode.OperationInvalid;
            }

            user = UserMapper.LoadByUserName(authorizationParameters[0]);
            if (null == user)
            {
                return ReturnCode.InvalidUserPass;
            }
            //valid user, check password

            var sha512 = SHA512Managed.Create();
            //get the salt from the user and add it to the password passed in

            //compute hash password using hash object
            var hashedpw = sha512.ComputeHash(Encoding.UTF8.GetBytes(authorizationParameters[1]).Concat(Convert.FromBase64String(user.Salt)).ToArray());
            if (user.PasswordHash.Equals(Convert.ToBase64String(hashedpw), StringComparison.OrdinalIgnoreCase))
            {
                return ReturnCode.Ok;
            }
            else
            {
                return ReturnCode.InvalidUserPass;
            }
        }
    }

}
