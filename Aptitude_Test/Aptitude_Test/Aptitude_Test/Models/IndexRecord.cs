using Aptitude_Test.Models;

namespace Aptitude_Test.Models
{
    public class IndexRecord
    {
         public IEnumerable<User> user { get; set; }
         public IEnumerable<Job> jobs { get; set; }
        public IEnumerable<Candidate> candidates { get; set; }

        public IEnumerable<Finalresult> finalresults { get; set; }


    }
}
