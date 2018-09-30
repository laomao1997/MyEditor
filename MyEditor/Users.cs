using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEditor
{
    class Users
    {
        private ArrayList usersList = new ArrayList();

        public Users()
        {
            Initial();
        }

        public void Initial()
        {
            StreamReader file = new StreamReader("login.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                usersList.Add(line);
            }
        }

        public bool IsUserExist(string u, string p)
        {
            bool isExist = false;
            foreach (var VARIABLE in usersList)
            {
                if (VARIABLE.ToString().Contains(u + "," + p)) 
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public String ShowUsers(string u, string p)
        {
            String users = "";
            foreach (var VARIABLE in usersList)
            {
                users += VARIABLE.ToString() + " " + VARIABLE.ToString().Contains(u+","+p) + "\n";
            }

            return users;
        }
    }
}
