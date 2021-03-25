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

                FileStream myFile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(myFile);

                writer.Write(textBoxEditor.Text);

                writer.Close();
            }
        }

        /// <summary>
        /// Clears the textbox editor and opens a new file. Keeps the older file open too!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNew(object sender, EventArgs e)
        {
            textBoxEditor.Clear();
            filePath = String.Empty;
        }
    }
}
