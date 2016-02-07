using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

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
 * Enhacements: This app implements cryptography methods like Encryption, Decryption
 *
 * Page description: this is code behind page of DetailsPage.xaml
 *
 **************************************************************************/
namespace PhoneApp1
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        string inittitile = "";                     // Initial title variable of string type to hold title
        string inittext = "";                       // Initial content variable of string type to hold content
        int cur = 0;                                // cur variable to hold current index
        bool saveflag = false;                      // flag to be set when save AppButton is clicked
        bool deleteflag = false;                    // flag to be set when delete AppButton is clicked
        // Constructor for DetailsPage class.
        public DetailsPage()
        {
            InitializeComponent();
            // Application bar is initialized
            ApplicationBar = new ApplicationBar();
            // ApplicationBar mode is set to default
            ApplicationBar.Mode = ApplicationBarMode.Default;
            // ApplicationBar opacity is set 1
            ApplicationBar.Opacity = 1.0;
            // ApplicationBar visibility is set to true
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            // ApplicationBar buttons are initilized
            ApplicationBarIconButton deletebtn = new ApplicationBarIconButton();
            ApplicationBarIconButton emailbtm = new ApplicationBarIconButton();
            ApplicationBarIconButton savebtn = new ApplicationBarIconButton();
            deletebtn.IconUri = new Uri("/Assets/AppbarIcons/delete.png", UriKind.Relative);
            // Text to ApplicationBar is set
            deletebtn.Text = "delete notes";
            // ApplicationBar click event is registered
            deletebtn.Click += deletebtn_Click;
            emailbtm.IconUri = new Uri("/Assets/AppbarIcons/feature.email.png", UriKind.Relative);
            // Text to ApplicationBar is set
            emailbtm.Text = "email notes";
            // ApplicationBar click event is registered
            emailbtm.Click += emailbtm_Click;
            savebtn.IconUri = new Uri("/Assets/AppbarIcons/check.png", UriKind.Relative);
            // Text to ApplicationBar is set
            savebtn.Text = "save notes";
            // ApplicationBar click event is registered
            savebtn.Click += savebtn_Click;
            // Buttons are added to ApplicationBar
            ApplicationBar.Buttons.Add(deletebtn);
            ApplicationBar.Buttons.Add(emailbtm);
            ApplicationBar.Buttons.Add(savebtn);
        }

        // Event handler for ApplicationButton click, called when delete button is clicked
        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (titletb.Text != "" || contenttb.Text != "")
                if (MessageBox.Show("Do you want to delete this?", "Delete", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Settings.NotesList.RemoveAt(cur);            // Remove Note from curren tIndex
                }
            deleteflag = true;                                  // Set deleteflag to true
            if (NavigationService.CanGoBack == true)            // Checking if Navigation service has back in stack
                NavigationService.GoBack();                     // If it is true then go back

        }

        // Event handler for ApplicationButton click, called when email button is clicked
        private void emailbtm_Click(object sender, EventArgs e)
        {
            EmailComposeTask em = new EmailComposeTask();       // Email compose task is initiated 
            em.To = "forwardemail@passwordapp.com";             // setting to property         
            em.Subject = "Emailing Contents of the note";       // setting subject property
            em.Body = titletb.Text + "\n" + contenttb.Text;     // setting body property
            em.Show();                                          // fire email

        }

        // Event handler for ApplicationButton click, called when save button is clicked
        private void savebtn_Click(object sender, EventArgs e)
        {
            saveflag = true;
            if (titletb.Text == "" || contenttb.Text == "")   // Check if both textbox are empty or not
            {
                MessageBox.Show("Both fields are required");  // If empty then display the message
                return;
            }
            if (inittitile != titletb.Text || inittext != contenttb.Text)

                if (MessageBox.Show("Are you sure to save the contents", "Save", MessageBoxButton.OKCancel) != MessageBoxResult.Cancel)
                {
                    SaveNote();
                    // If can go back goto previos page
                    if (NavigationService.CanGoBack == true)
                        NavigationService.GoBack();
                }

        }

        // Overrided onNavigatedTo which is invoked when the Page is loaded and becomes the current source 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           // if (PhoneApplicationService.Current.State.ContainsKey("newpage"))   // Checking State for key newpage
            if (State.ContainsKey("detailsPagetitle") && State.ContainsKey("detailsPagecontent") && State.ContainsKey("cur")) // Checking Page State for keys detailsPagetitle, detailsPagecontent, cur
                {
                contenttb.Text = State["detailsPagecontent"] as string; // Retriving from Page state
                titletb.Text = State["detailsPagetitle"] as string;
                cur = int.Parse(State["cur"].ToString());
                return;
            }
            cur = Settings.CurrentNoteIndex;                            // Get current note index
            if (cur == 0 && Settings.NotesList.ElementAt<Note>(cur).Title==null)
            {
                Dispatcher.BeginInvoke(() => titletb.Focus());            // Set focus to textbox
                return;
            }                                                      

            else if (Settings.NotesList.Count != 0 && Settings.NotesList.ElementAt<Note>(cur).Title != null)                  //
            {                                                                                                                 //
                contenttb.Text = Crypto.Decrypt(Settings.NotesList.ElementAt<Note>(cur).EncryptedContent,Settings.Password);  // Assigning current note to textboxes
                inittext = contenttb.Text;                                                                                    //
                titletb.Text = Settings.NotesList.ElementAt<Note>(cur).Title;                                                 //
                inittitile = titletb.Text;                                                                                    //
                contenttb.FontSize = titletb.FontSize = Settings.NotesList.ElementAt<Note>(cur).TextSize;
            }
            
        }

        // Overrided onNavigatedFrom which is invoked when the Page is navigated to other pages 
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if ((deleteflag || saveflag) && (titletb.Text != "" || contenttb.Text != ""))                                  // Checking flags
            {
                State.Remove("detailsPagetitle");                                                                          // 
                State.Remove("detailsPagecontent");                                                                        // Removing content, title, cur from page state
                State.Remove("cur");                                                                                       //
                PhoneApplicationService.Current.State.Remove("newpage");
                return;
            }
            State["detailsPagetitle"] = titletb.Text;                                                                      // 
            State["detailsPagecontent"] = contenttb.Text;                                                                  // Saving content, title, cur to page state
            State["cur"] = cur;                                                                                            //
            if ((inittitile != titletb.Text || inittext != contenttb.Text))
            {
                if (MessageBox.Show("Do you want to save this?", "Save", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveNote();
                }
            }
          
        }

        //Function SaveNote has no arguments, it saves the title and encrypted contents to noteslist
        private void SaveNote()
        {
            Settings.NotesList.ElementAt<Note>(cur).Title = titletb.Text;                                                  // Updating all the contents of the Note
            Settings.NotesList.ElementAt<Note>(cur).EncryptedContent = Crypto.Encrypt(contenttb.Text, Settings.Password);  // Encrypting and storing contents          
        }

        //Function PhoneApplicationPage_BackKeyPress is called when backbutton is pressed. It will delete empty notes
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cur == 0)  //To remove the blank note which will be at position 0.
                if (Settings.NotesList.ElementAt<Note>(0).Title == null)
                    Settings.NotesList.RemoveAt(0);
        }
    }
}