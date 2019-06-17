using System.Net;

namespace Models
{
    public static class Environment {

        public static bool IsTestEnvironment()
        {
            return true;
        }

        public static string LocalHost()
        {
            return "127.0.0.1";
        }

        public static string Remote()
        {
            return "52.143.161.188";
        }
    }
}
