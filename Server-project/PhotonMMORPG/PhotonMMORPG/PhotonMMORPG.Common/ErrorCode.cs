using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotonMMORPG.Common
{
    public enum ErrorCode:byte
    {
        Ok = 0,

        InvalidParameters,

        NameExists,

        RequestNotImplemented
    }
}
