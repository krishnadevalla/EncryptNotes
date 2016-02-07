using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**************************************************************************
 * Programmer : Leela Krishna Devalla
 * Date       : 11/18/2015
 * 
 * Class Name: Settings (static)
 * Fields    : NoteList of type ObservableCollection<Note>, 
 *             settings of type IsolatedStorageSettings,
 *             currentNoteIndex of type int
 *             hashedPassword of type string
 *             passwordHint of type string
 *             password of type string
 *             isLoggedIn of type bool
 *             salt of type byte array
 * 
 * Properties: CurrentNoteindex 
 *             HashedPassword
 *             PasswordHint
 *             Password
 *             IsLoggedIn
 *             Salt
 * 
 * Methods   : None
 *
 * Changes or enhancements: The password, hashedpassword, hint, salt which are useful for
 *                          encryption is stored here, which are retrieved further in the program
 * 
 * This class has ObservableCollection of Notes and it has IsolatedStorageSetting
 * and also currentNoteIndex which is used in retreving the note from NotesList
 **************************************************************************/
namespace PhoneApp1
{
    // class Settings
    public static class Settings
    {
        public static ObservableCollection<Note> NotesList = new ObservableCollection<Note>();            // NoteList which is an Observable Collection of Note
        public static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;     // settings which is a IsolatedStorageSettings variable
        private static int currentNoteIndex;                                                              // currentNoteIndex field of type int
        private static string hashedPassword;                                                             // hashed password field of type string
        private static string passwordHint;                                                               // password hint field of type string
        private static string password;                                                                   // password field of type string, which stores clear text
        private static bool isLoggedIn;                                                                   // isLoggedIn field of type bool
        private static byte[] salt;                                                                       // salt field of type byte array
        public static int CurrentNoteIndex                                                                // CurrentNoteIndex property which sets currentNoteIndex field
        {
            get                                             // getting value
            {
                return currentNoteIndex;
            }
            set                                              // setting value
            {
                currentNoteIndex = value;
            }
        }
        public static string HashedPassword
        {
            get                                             // getting value
            {
                return hashedPassword;                     
            }
            set                                            // setting value
            {
                hashedPassword = value;
            }
        }
        public static string PasswordHint
        {
            get                                           // getting value
            {
                return passwordHint;
            }
            set                                          // setting value
            {
                passwordHint = value;
            }
        }
        public static string Password
        {
            get                                          // getting value
            {
                return password; 
            }
            set                                         // setting value
            {
                password = value;
            }
        }
        public static bool IsLoggedIn
        {
            get                                         // getting value
            {
                return isLoggedIn;
            }
            set                                        // setting value
            {
                isLoggedIn = value;
            }
        }
        public static byte[] Salt
        {
            get                                       // getting value
            {
                return salt;
            }
            set                                       // setting value
            {
                salt = value;
            }
        }

        }
}
