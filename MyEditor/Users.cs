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
            usersList.Clear();
            if (!File.Exists("login.txt"))
            {
                //不存在就新建一个文本文件,并写入一些内容
                using (StreamWriter sw = File.CreateText("login.txt"))
                {
                    sw.WriteLine("");
                }
                
            }
            using(StreamReader file = new StreamReader("login.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    usersList.Add(line);
                }
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

        public bool IsUserExist(string u)
        {
            bool isExist = false;
            foreach (var VARIABLE in usersList)
            {
                if (VARIABLE.ToString().Contains(u))
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public void AddUser(string username, string password, string fName, string sName, string doBirth, string uType)
        {
            string line = username + "," + password + "," + uType + "," + fName + "," + sName + "," + doBirth;
            using (StreamWriter writer = new StreamWriter("login.txt", true))
            {
                writer.WriteLine("\n" + line);
            }
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
