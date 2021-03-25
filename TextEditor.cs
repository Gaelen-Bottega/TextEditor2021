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
        string filePath = string.Empty;
        bool isConfirmed = true;
        
        public formTextEditor()
        {
            InitializeComponent();
        }

        #region "Event Handlers"

        #region "File Menu"

        /// <summary>
        /// Clears the textbox editor and opens a new file. Keeps the older file open too!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNew(object sender, EventArgs e)
        {
            ConfirmClose();
            if (isConfirmed == true)
            {
                textBoxEditor.Clear();
                filePath = String.Empty;
                UpdateTitle();
            }
            
        }

        /// <summary>
        /// Opens a text file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileOpen(object sender, EventArgs e)
        {
            ConfirmClose();
            if (isConfirmed == true)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*txt"; // Do we need this? 

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    FileStream openFile = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(openFile);

                    textBoxEditor.Text = reader.ReadToEnd();

                    reader.Close();
                }
            }
            
        }

        /// <summary>
        /// Saves the file.
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
            ConfirmClose();
            if (isConfirmed == true)
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
            Clipboard.SetText(textBoxEditor.SelectedText);

            textBoxEditor.SelectedText = "";
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
        /// 
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
        /// </summary>
        public void UpdateTitle()
        {
            this.Text = "Gaelen's Text Editor";
            
            if (filePath != String.Empty)
            {
                this.Text += " - " + filePath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void SaveTextFile(string path)
        {
            FileStream myFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(myFile);

            writer.Write(textBoxEditor.Text);

            writer.Close();
        }

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

        #endregion

       
    }
}
