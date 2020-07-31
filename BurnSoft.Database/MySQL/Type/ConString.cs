using System.Dynamic;

namespace BurnSoft.Database.MySQL.Type
{
    internal class ConString
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string PersistSecurityInfo { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
