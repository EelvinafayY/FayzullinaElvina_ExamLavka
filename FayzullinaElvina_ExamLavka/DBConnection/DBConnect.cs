using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FayzullinaElvina_ExamLavka.DBConnection
{
    public class DBConnect
    {
        public static FayzullinaElvina_LavkaExamEntities1 DB = new FayzullinaElvina_LavkaExamEntities1();
        public static Workers loginedWorker;
        public static Users loggedUsers;
    }
}
