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
            textBoxEditor.Clear();
            filePath = String.Empty;
            UpdateTitle();
        }

        /// <summary>
        /// Opens a text file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileOpen(object sender, EventArgs e)
        {

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
            Close();
        }
        #endregion

        #region "Edit Menu"

        /// <summary>
        /// Copies the contents of the textbox to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCopy(object sender, EventArgs e)
        {

            Clipboard.SetText(textBoxEditor.SelectedText);
            //if (textBoxEditor.Text != null)
            //{
            //    Clipboard.SetText(textBoxEditor.Text);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditCut(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxEditor.SelectedText);

            textBoxEditor.SelectedText = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPaste(object sender, EventArgs e)
        {
            textBoxEditor.Paste();
        }

        #endregion

        #region "Help Menu"

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



        #endregion

    }
}
