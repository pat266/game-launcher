using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher_VLCM_niua_lsaj.Utils
{
    public class Server
    {
    }

    public class UpcomingServer
    {
        public int serverNumber { get; set; }
        public DateTime releaseDateEst { get; set; }
        public double hoursTillRelease { get; set; }
        public override string ToString()
        {
            return $"Server Number: {serverNumber}, Release Date: {releaseDateEst} EST, Hours until release: {hoursTillRelease}";
        }
    }
    public class Game2cnServer
    {
        public string state { get; set; }
        public string code { get; set; }
        public string open_date { get; set; }
    }
}
