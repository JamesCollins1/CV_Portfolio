using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace server
{
    public class Room
    {
        /*
         * This is a helper function used to store information for use later.
         * 
         * 
         */ 
        public Room(String name, String desc)
        {
            this.desc = desc;
            this.name = name;
        }

        public String name = "";
        public String desc = "";
        public String north;
        public String south;
        public String east;
        public String west;
        public String up;
        public String down;

    }

}
