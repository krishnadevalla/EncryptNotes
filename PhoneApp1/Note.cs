using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**************************************************************************
 * Programmer : Leela Krishna Devalla
 * Date       : 11/18/2015
 * 
 * Class Name: Note (has default constructor)
 * Fields    : title of type string, content of type string, textSize of type int
 * 
 * Properties: Title, EncryptedContent, TextSize 
 * 
 * Methods   : None
 * 
 * Changes or enhacement : Modified property is removed, Content property is modified to 
 *                         EncryptedContent which stores encrypted data
 *
 * This class is a collects of notes which has title, content and textsize and date modified
 **************************************************************************/
namespace PhoneApp1
{
    //Note class
    public class Note
    {
        private string title;                               // title field of class Note
        private string content;                             // content field of class Note
        private int textSize=32;                            // textSize field of class Note
        public string Title                                 // Property name Title which sets and gets title field of Note class
        {
            get                                             // getting value
            {
                return title;
            }
            set                                             // setting value
            {
                title = value;
            }
        }
        public string EncryptedContent                      // Property name EncryptedContent which sets and gets content field of Note class
        {
            get                                             // getting value
            {
                return content;
            }
            set                                             // setting value
            {
                content = value;
            }
        }
        public int TextSize                                 // Property name TextSize which sets and gets textSize field of Note class
        {
            get                                              // getting value
            {
                return textSize;
            }
            set                                              // setting value
            {
                textSize = value;
            }
        }
        public Note()                                       // Default constructor
        { }
    }
}
