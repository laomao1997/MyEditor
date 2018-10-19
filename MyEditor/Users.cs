using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
---------------------------------------------------------------------------------------------------------------
|    User                                                                                                     |
---------------------------------------------------------------------------------------------------------------
| - usersList: ArrayList                                                                                      |
---------------------------------------------------------------------------------------------------------------
| + Initial()                                                                                                 |
| + IsUserExist(u: string, p: string)                                                                         |
| + IsUserExist(u: string)                                                                                    |
| + AddUser(username: string, password: string, fName: string, sName: string, doBirth: string, uType: string) |
| + isEditType(username: string, password: string)                                                            |
| + ShowUsers(u: string, p: string)                                                                           |
--------------------------------------------------------------------------------------------------------------- 
*/
namespace MyEditor
{
    class Users
    {
        private ArrayList usersList = new ArrayList();

        public Users()
        {
            Initial();
        }

        // Initialize
        public void Initial()
        {
            usersList.Clear();
            if (!File.Exists("login.txt"))
            {
                // If login.txt does not exist, 
                // create a new text file and write something in it
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

        // Return if the user exist in login.txt
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

        // Return if the user exist in login.txt
        public bool IsUserExist(string u)
        {
            bool isExist = false;
            foreach (var VARIABLE in usersList)
            {
                if (VARIABLE.ToString().Contains(u+","))
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        // Add new user to login.txt
        public void AddUser(string username, string password, string fName, string sName, string doBirth, string uType)
        {
            string line = username + "," + password + "," + uType + "," + fName + "," + sName + "," + doBirth;
            using (StreamWriter writer = new StreamWriter("login.txt", true))
            {
                writer.WriteLine("\n" + line);
            }
        }

        // Return is the user can edit
        public bool isEditType(string username, string password)
        {
            bool isEditTypeExist = false;
            foreach (var VARIABLE in usersList)
            {
                if (VARIABLE.ToString().Contains(username+","+password+","+"Edit"))
                {
                    isEditTypeExist = true;
                }
            }

            return isEditTypeExist;
        }

        // Show all the users in login.txt file
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
