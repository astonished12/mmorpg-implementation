using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameCommon;
using MGF.Domain;
using MGF.Mappers;
using Servers.Services.Interfaces;
using SHA512Managed = System.Security.Cryptography.SHA512Managed;

namespace Servers.Services.AuthorizationServices
{
    public class UserPassAuthorizationService : IAuthorizationService
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
            //compute hash password using hash object
            var hashedpw = sha512.ComputeHash(Encoding.UTF8.GetBytes(authorizationParameters[1])); ;
            if (user.PasswordHash.Equals(Convert.ToBase64String(hashedpw), StringComparison.OrdinalIgnoreCase))
            {
                return ReturnCode.Ok;
            }
            else
            {
                return ReturnCode.InvalidUserPass;
            }
        }

        public ReturnCode CreateAccount(params string[] authorizationParameters)
        {
            if (authorizationParameters.Length != 2)
            {
                return ReturnCode.OperationInvalid;
            }

            UserMapper userMapper = new UserMapper();;
            User user = UserMapper.LoadByUserName(authorizationParameters[0]);

            if (null == user)
            {
                //Create the user
                var sha512 = SHA512Managed.Create();
                //compute hash password using hash object
                var hashedpw = sha512.ComputeHash(Encoding.UTF8.GetBytes(authorizationParameters[1]));

                user = new User()
                {
                    LoginName = authorizationParameters[0],
                    PasswordHash = Convert.ToBase64String(hashedpw)
                };
                userMapper.Save(user); 
                
                return ReturnCode.Ok;
            }
            else
            {
                return ReturnCode.InvalidUserPass;
            }
        }
    }

}
