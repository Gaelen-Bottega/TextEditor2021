// Project:     Text Editor 2021
// Author:      Gaelen Rhoads and Kyle Chapman
// Start Date:  March 25, 2021
// Last Date:   March 25, 2021
// Description:
// This application showcases very basic file management practices. The user can create and edit, save, and close a file. They
// can also copy, cut and paste text as they see fit. This application also has protection against users closing the file by accident by prompting them to save
// when needed. The form title will show when a file has been changed and react accordingly to ensure the user does not lose work.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor2021
{
    public partial class formTextEditor : Form
    {
        // String to hold the file path of a file.
        string filePath = string.Empty;
        // Boolean to confirm if a user wants to exit an unsaved file or not.
        bool isConfirmed = true;
        // Boolean to represent changes to a file.
        bool isUnchanged = true;
        
        public formTextEditor()
        {
            InitializeComponent();
        }

        #region "Event Handlers"

        /// <summary>
        /// This will fire whenever the text has been changed in the forms text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextModified(object sender, EventArgs e)
        {
            // Whenever text is changed, set isUnchanged to false
            isUnchanged = false;
            // Update the title to show the "*", meaning a user should save.
            UpdateTitle();
        }

        #region "File Menu"

        /// <summary>
        /// Clears the textbox editor and opens a new file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNew(object sender, EventArgs e)
        {
            // If the file is unchanged since last save, just open a new file without bothering the user!
            if (isUnchanged == true)
            {
                // Function is at bottom of this file, to avoid repeat code.
                NewFile();
            }
            // If it has unsaved changes, ask the user if they would like to save.
            else
            {
                ConfirmClose();
                if (isConfirmed == true)
                {
                    NewFile();
                }
            }
            
            
        }

        /// <summary>
        /// Opens a text file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileOpen(object sender, EventArgs e)
        {
            if (isUnchanged == true)
            {
                // Function is at bottom region, to avoid repeat code!
                OpenFile();
            }
            else
            {
                ConfirmClose();
                if (isConfirmed == true)
                {
                    OpenFile();
                }
            }
            
            
        }

        /// <summary>
        /// Saves the file. If a file path for the file already exists, that is. If no file path exists, the FileSaveAs event handler is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSave(object sender, EventArgs e)
        {
            if (filePath == String.Empty)
            {
                // Then call the Save As... event handler!
                FileSaveAs(sender, e);
            }
            // If there IS a filepath...
            else
            {
                // Then save it.
                SaveTextFile(filePath);
                isUnchanged = true;
            }
        }

        /// <summary>
        /// Opens a save dialog window with filters and allows the user to choose where they want to save their file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSaveAs(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*txt|All files (*.*)|*.*";

            if(saveDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = saveDialog.FileName;

                SaveTextFile(filePath);

                isUnchanged = true;

                UpdateTitle();
            }
        }

        /// <summary>
        /// Closes the form. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileExit(object sender, EventArgs e)
        {
            if (isUnchanged == false)
            {
                ConfirmClose();
                if (isConfirmed == true)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }
        #endregion

        #region "Edit Menu"

        /// <summary>
        /// Copies the contents of the textbox to the clipboard. If the users selection is empty, the clipboard will hold the value
        /// that was last copied and do nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCopy(object sender, EventArgs e)
        {
            if (textBoxEditor.SelectedText != String.Empty)
            {
                Clipboard.SetText(textBoxEditor.SelectedText);
            }
        }

        /// <summary>
        /// This will do the same thing as EditCopy but set the users selected text equal to an empty string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCut(object sender, EventArgs e)
        {
            if (textBoxEditor.SelectedText != String.Empty)
            {
                Clipboard.SetText(textBoxEditor.SelectedText);

                textBoxEditor.SelectedText = "";
            }
                
        }

        /// <summary>
        /// Pastes what is on the clipboard to the text editor. Thanks .NET Framework!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPaste(object sender, EventArgs e)
        {
            textBoxEditor.Paste();
        }

        #endregion

        #region "Help Menu"

        /// <summary>
        /// Shows information about the application to the user. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpAbout(object sender, EventArgs e)
        {
            MessageBox.Show("Text Editor\n" + "By Gaelen Rhoads\n\n" + "For NETD 2202\n" + "March 2021\n\n" +
                            "This text editor showcases basic functionality of menu items. This includes opening, " +
                            "saving and closing and text file. The user can also copy, cut and paste text as they " +
                            "please. This simple text editor will be included in a larger application in a couple of weeks.", "About this application");
        }
        #endregion

        #endregion

        #region "Functions"

        /// <summary>
        /// Updates the title. If there is a file path present, (file has been saved) then it will show the file path of the file.
        /// If the file has changes, the title will have an "*" to tell the user to save. It will disappear when they save.
        /// </summary>
        public void UpdateTitle()
        {
            this.Text = "Gaelen's Text Editor";
            
            if (filePath != String.Empty)
            {
                this.Text += " - " + filePath;
            }

            if (!isUnchanged)
            {
                this.Text += "*";
            }
        }

        /// <summary>
        /// Saves a file to a path specified by the user.
        /// </summary>
        /// <param name="path"></param>
        public void SaveTextFile(string path)
        {
            FileStream myFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(myFile);

            writer.Write(textBoxEditor.Text);

            writer.Close();
        }

        /// <summary>
        /// Confirms if the user wants to save before closing, or just close without saving. If they hit yes, the program will fire the save event handler. If
        /// they hit no, it will just proceed with whatever action they wanted to do in the first place. If they hit cancel, it will abort the operation they 
        /// were trying to do.
        /// </summary>
        public void ConfirmClose()
        {
            var confirm = MessageBox.Show("There are unsaved changes. Do you want to save before you close this file?\n", "Confirming Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                menuFileSave.PerformClick();
                if (filePath != String.Empty)
                {
                    MessageBox.Show("File Saved. The file will now close.", "File Saved Sucessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                isConfirmed = true;
            }
            else if (confirm == DialogResult.No)
            {
                isConfirmed = true;
            }
            else
            {
                isConfirmed = false;
                MessageBox.Show("Operation Cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Opens an existing file. This function was made to avoid repeat code.
        /// </summary>
        public void OpenFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*txt"; 

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream openFile = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(openFile);

                textBoxEditor.Text = reader.ReadToEnd();

                reader.Close();
                filePath = openDialog.FileName;
                isUnchanged = true;
                UpdateTitle();
            }
        }

        /// <summary>
        /// Opens a new file. This function was made to avoid repeat code. (Ironic how I repeated that comment though..)
        /// </summary>
        public void NewFile()
        {
            textBoxEditor.Clear();
            filePath = String.Empty;
            isUnchanged = true;
            UpdateTitle();
        }

        #endregion
    }
}
