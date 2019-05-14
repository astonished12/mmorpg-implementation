using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace PubComp.NoSql.ServiceStackRedis
{
    public class RedisConnectionInfo
    {
        private const string Prefix = @"redis://";
        private const string DefaultHost = @"localhost";
        private const int DefaultPort = 6379;
        private const long DefaultDb = 0L;

        public string Host { get; set; }

        public int Port { get; set; }

        public string Client { get; set; }

        public string Password { get; set; }

        public long Db { get; set; }

        public IList<KeyValuePair<string, string>> Options { get; set; }

        public RedisConnectionInfo()
        {
            this.Host = DefaultHost;
            this.Port = DefaultPort;
            this.Client = null;
            this.Password = null;
            this.Db = DefaultDb;
            this.Options = new List<KeyValuePair<string, string>>();
        }

        public string ConnectionString
        {
            get
            {
                string userPass = string.Empty;
                if (!string.IsNullOrEmpty(Client))
                {
                    userPass = WebUtility.UrlEncode(Client);
                    if (!string.IsNullOrEmpty(Password))
                        userPass += ':' + WebUtility.UrlEncode(Password);

                    userPass += '@';
                }

                if (Db != 0 && !Options.Any(o => o.Key.ToLower() == "db"))
                {
                    if (Options == null)
                        Options = new List<KeyValuePair<string, string>>();

                    Options.Add(new KeyValuePair<string, string>("db", this.Db.ToString(CultureInfo.InvariantCulture)));
                }

                string dbParams = string.Empty;
                if (Options != null && Options.Any())
                {
                    dbParams += '?';
                    dbParams += string.Join(";", Options.Select(opt => opt.Key + '=' + opt.Value));
                }

                var hostPort = string.Concat(this.Host, ':', this.Port);

                var result = string.Concat(Prefix, userPass, hostPort, dbParams);
                return result;
            }
        }
    }
}
