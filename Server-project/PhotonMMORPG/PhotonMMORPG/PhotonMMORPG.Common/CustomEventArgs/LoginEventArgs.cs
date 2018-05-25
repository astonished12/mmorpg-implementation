using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonMMORPG.Common.CustomEventArgs
{
    public class LoginEventArgs:EventArgs
    {

        public ErrorCode Error { get; private set; }

        public LoginEventArgs(ErrorCode error)
        {
            Error = error;
        }
    }
}
