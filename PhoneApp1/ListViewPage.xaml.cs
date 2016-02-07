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
 * Page description: this is code behind page of ListViewPage.xaml
 *
 **************************************************************************/
namespace PhoneApp1
{
    public partial class ListViewPage : PhoneApplicationPage
    {
        ApplicationBarMenuItem sortmenuitem;
        public ListViewPage()
        {
            InitializeComponent();
            // Application bar is initialized
            ApplicationBar = new ApplicationBar();
            sortmenuitem = new ApplicationBarMenuItem("Sort notes");
            // ApplicationBar mode is set to default
            ApplicationBar.Mode = ApplicationBarMode.Default;
            // ApplicationBar opacity is set 1
            ApplicationBar.Opacity = 1.0;
            // ApplicationBar visibility is set to true
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            // ApplicationBar button is initilized
            ApplicationBarIconButton addbtn = new ApplicationBarIconButton();
            ApplicationBarIconButton changepass = new ApplicationBarIconButton();
            // Application button icon is set
            addbtn.IconUri = new Uri("/Assets/AppbarIcons/add.png", UriKind.Relative);
            changepass.IconUri = new Uri("/Assets/AppbarIcons/key.png", UriKind.Relative);
            // Text to ApplicationBar is set
            addbtn.Text = "add notes";
            changepass.Text = "Change password";
            // ApplicationBar click event is registered
            addbtn.Click += addbtn_Click;
            changepass.Click += Changepass_Click;
            sortmenuitem.Click += Sortmenuitem_Click;
            // Button is added to ApplicationBar
            ApplicationBar.Buttons.Add(addbtn);
            ApplicationBar.Buttons.Add(changepass);
            ApplicationBar.MenuItems.Add(sortmenuitem);
            
        }

        // Sortmenuitem_Click is click event handler for Sort button which sort the NoteList items
        private void Sortmenuitem_Click(object sender, EventArgs e)
        {
            Settings.NotesList= new ObservableCollection<Note>(Settings.NotesList.OrderBy(i=>i.Title));
            this.DataContext = Settings.NotesList;
        }

        // Changepass_Click is click event handler for change password button which navigates to ChangePassword.xaml
        private void Changepass_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ChangePassword.xaml", UriKind.Relative));
        }

        // Event handler for ApplicationButton click 
        private void addbtn_Click(object sender, EventArgs e)
        {
            Note new1 = new Note();                  // New note is created
            new1.Title = null;                        // Note title is set to null
            new1.EncryptedContent = null;                      // Note content is set to null
            Settings.NotesList.Insert(0, new1);     // Note is inserted to NoteList at 0 position
            Settings.CurrentNoteIndex = 0;          // CurrentNoteIndex is set to 0
            // Navigate to Details page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
        }

        // Overrided onNavigatedTo which is invoked when the Page is loaded and becomes the current source 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //
            base.OnNavigatedTo(e);
            this.DataContext = null;                      // DataContext of this page is set to null
            this.DataContext = Settings.NotesList;        // DataContext of this page is set to NoteList collection
            if (Settings.NotesList.Count == 0)            // If NoteList count is 0 then make the textblock which shows No notes to visible
            { 
                no.Visibility = Visibility.Visible;
                sortmenuitem.IsEnabled = false;
            }
            else
            { 
                no.Visibility = Visibility.Collapsed;     // textblock visibility is set to collapsed if count is not 0
                sortmenuitem.IsEnabled = true;

            }
            Settings.CurrentNoteIndex = -1;               // Current note index is set to -1
            Noteslistbox.SelectedIndex = -1;              // Listbox selected index is set to -1
           
        }

        // Event handler for Listbox section changed 
        private void Noteslistbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Noteslistbox.SelectedIndex >= 0)           // Check if listbox selected index if greater than or equal to 0
            {
                Settings.CurrentNoteIndex = Noteslistbox.SelectedIndex;                      // Set current note index to selected item index
                NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));  // Navigate to DetailsPage
            }
        }

        // Event handler for back button press which removes back entry from Navigation stack
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }
    }
}