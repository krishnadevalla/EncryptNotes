using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
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
 * Page description: this is code behind page of MainPage.xaml
 *
 **************************************************************************/
namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ApplicationBar = new ApplicationBar();
            // ApplicationBar mode is set to default
            ApplicationBar.Mode = ApplicationBarMode.Default;
            // ApplicationBar opacity is set 1
            ApplicationBar.Opacity = 1.0;
            // ApplicationBar visibility is set to true
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            // ApplicationBar buttons are initilized
            ApplicationBarIconButton hintbtn = new ApplicationBarIconButton();
            hintbtn.IconUri = new Uri("/Assets/AppbarIcons/info.png", UriKind.Relative);
            hintbtn.Text = "Hint";
            // Text to ApplicationBar is set
            hintbtn.Click += Hintbtn_Click;
            ApplicationBar.Buttons.Add(hintbtn);
            Thread fetch = new Thread(Fetch);
            fetch.Start();
        }

        //Function fetch has on argument, object. It will be called by a thread
        private void Fetch(object obj)
        {
            if(Settings.HashedPassword!=null) // Checking if hashedpassword is null or not
            { 
                Settings.PasswordHint= Settings.settings["passhint"] as string;                         //
                Settings.Salt= Settings.settings["saltval"] as byte[];                                  //  retriving from IsolatedStorage
                Settings.NotesList = Settings.settings["notelistiso"] as ObservableCollection<Note>;    //
            }
        }

        //Button Click even handler for hint button to display messagebox containing hint
        private void Hintbtn_Click(object sender, EventArgs e)
        {
            if (Settings.PasswordHint != "")                // Is hint isnot  empty
                MessageBox.Show("Password hint: " + Settings.PasswordHint);   // Display messagebox
            else
                MessageBox.Show("Password hint is not set");
        }

        //Button Click even handler for Okay button which works for two buttons
        private void Okay_click(object sender, RoutedEventArgs e)
        {
            // Validating the  hasedpasswords and is loggedin
            if(Settings.HashedPassword != null || Settings.IsLoggedIn==true)
            {
                if (Crypto.Hash(password.Password).Equals(Settings.HashedPassword)) // Checking hash of entered password and existing hasedpassword
                {
                    Settings.Password = password.Password;
                    NavigationService.Navigate(new Uri("/ListViewPage.xaml", UriKind.Relative)); // Navigate to ListViewPage
                }
                else
                {
                    MessageBox.Show("Wrong Password");
                    return;
                }
            }
            else 
            {
                if (newpassword.Password != "" && repeatpassword.Password != "" && newpassword.Password.Equals(repeatpassword.Password))
                {
                    Settings.Salt = Crypto.GenerateNewSalt(16);                                 // Generating new salt
                    Settings.HashedPassword = Crypto.Hash(newpassword.Password);                // Hashing new password
                    Settings.PasswordHint = passhint.Text;                                      // updating password hint
                    Settings.Password = newpassword.Password;                                   // updating password
                    Settings.IsLoggedIn = true;                                                 // setting isloggedin
                    NavigationService.Navigate(new Uri("/ListViewPage.xaml", UriKind.Relative));// Navigate to ListViewPage
                }
                else
                    MessageBox.Show("Required values missing or does not match");
            }
        }

        // Page Load event handler which is called when the page is loaded
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Checking for hashed password
            if(Settings.HashedPassword!=null)
            {
                Loginstack.Visibility = Visibility.Visible;             //
                Registrationstack.Visibility = Visibility.Collapsed;    //  Enabling Login stack panel
                ApplicationBar.IsVisible = true;                        //
            }
            else
            {
                Loginstack.Visibility = Visibility.Collapsed;             //
                Registrationstack.Visibility = Visibility.Visible;        //  Enabling Registration stack panel
                ApplicationBar.IsVisible = false;                         //
            }
        }
    }
}