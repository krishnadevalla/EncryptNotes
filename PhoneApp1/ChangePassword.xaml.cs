using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

/**************************************************************************
 *
 * This app will take notes from the user and stores them. The notes can be 
 * passwords or other secrets. 
 * 
 * This app will save the list of notes in isolated storage so that the next time the app is launched
 * , it can reach into isolated storage and fetch the list for display here on this page.
 *  
 * Programmer : Leela Krishna Devalla
 * Date       : 11/18/2015
 *
 * Enhacements: This app implements cryptography methods like Encryption, Decryption and contains authentication
 *
 * Page description: this is code behind page of ChangePassword.xaml
 *
 **************************************************************************/
namespace PhoneApp1
{
    public partial class ChangePassword : PhoneApplicationPage
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        //Button Click even handler for Okay button which sets new password and modify content using new password
        private void Okay_click(object sender, RoutedEventArgs e)
        {
            // Validating text boxes
            if (oldpassword.Password != "" || newpassword.Password != "")
            {
                if (Crypto.Hash(oldpassword.Password) != Settings.HashedPassword)
                {
                    MessageBox.Show("Old password incorrect");
                    return;
                }
                if(newpassword.Password!=repeatpassword.Password)   
                {
                    MessageBox.Show("New Password not matching");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Passwords missing");
                return;
            }
            Settings.PasswordHint = passhint.Text;                         // Setting hint
            Settings.Password = newpassword.Password;                      // updating password
            List<Note> newlist = new List<Note>(Settings.NotesList);       // new list
            newlist.ToList().ForEach(i=>i.EncryptedContent=Crypto.Decrypt(i.EncryptedContent,oldpassword.Password)); // Decrypting the contents to new list
            Settings.Salt = Crypto.GenerateNewSalt(16);                                                              // Creating new salt
            newlist.ToList().ForEach(i => i.EncryptedContent = Crypto.Encrypt(i.EncryptedContent, newpassword.Password)); //Encrypting content
            Settings.NotesList.Clear();                                                                               // clearing Notelist
            foreach (var item in newlist)                                                                             // Copying new list to NoteList
            {
                Settings.NotesList.Add(item);
            }
            Settings.HashedPassword = Crypto.Hash(newpassword.Password);                                            // Updating hasedpassword
            MessageBox.Show("Password Changed successfully");
            NavigationService.GoBack();                                // Go back
        }     
    }
}