using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SprintBoard.Forms
{
    public partial class ReportForm : Form
    {
        public Board sprintboard;
        List<String> Stats;
        public ReportForm()
        {
            InitializeComponent();
        }

      

        private void DocumentToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringReader reader = new StringReader(richTextBox1.Text);
            float LinesPerPage = 0;
            float YPosition = 0;
            int Count = 0;
            float LeftMargin = e.MarginBounds.Left;
            float TopMargin = e.MarginBounds.Top;
            string Line = null;
            Font PrintFont = this.richTextBox1.Font;
            SolidBrush PrintBrush = new SolidBrush(Color.Black);

            LinesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);

            while (Count < LinesPerPage && ((Line = reader.ReadLine()) != null))
            {
                YPosition = TopMargin + (Count * PrintFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(Line, PrintFont, PrintBrush, LeftMargin, YPosition, new StringFormat());
                Count++;
            }

            if (Line != null)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            PrintBrush.Dispose();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            Stats = new List<string>() { "In progress", "Completed", "Deleted" };
            dGVStories.ColumnCount = 3;
            dGVStories.Columns[0].HeaderText = "Story ID";
            dGVStories.Columns[1].HeaderText = "Story Name";
            dGVStories.Columns[2].HeaderText = "Story Status";
            foreach (Story story in sprintboard.Stories)
            {
                dGVStories.Rows.Add(story.StoryID, story.StoryName, Stats[story.Status]);

            }
            dGVStories.ClearSelection();
            richTextBox1.Text = "No story selected . . .";
           
        }

        private void dGVStories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selected_index = dGVStories.SelectedRows[0].Index;
            Font headerfont = new Font("Arial", 12, FontStyle.Underline);
            Font normalfont = new Font("Arial", 12, FontStyle.Regular);

            Story st = sprintboard.Stories[selected_index];

            richTextBox1.Text = "";
            richTextBox1.SelectionFont = headerfont;
            richTextBox1.SelectedText += "Story :" + st.StoryName + Environment.NewLine;
            richTextBox1.SelectedText += Environment.NewLine;

            richTextBox1.SelectionFont = normalfont;
            richTextBox1.SelectedText += "Details :" + st.StoryText + Environment.NewLine;
            richTextBox1.SelectedText += Environment.NewLine;

            foreach (StoryTask stT in sprintboard.StoryTasks)
            {
                if (stT.StoryID == st.StoryID)
                {
                    richTextBox1.SelectionFont = headerfont;
                    richTextBox1.SelectedText += "Task :" + stT.TaskName + Environment.NewLine;
                    richTextBox1.SelectionFont = normalfont;
                    richTextBox1.SelectedText += "Details :" + stT.TaskText + Environment.NewLine;
                    richTextBox1.SelectedText += Environment.NewLine;
                }

            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument documentToPrint = new PrintDocument();
            printDialog.Document = documentToPrint;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                StringReader reader = new StringReader(richTextBox1.Text);
                
                documentToPrint.PrintPage += new PrintPageEventHandler(DocumentToPrint_PrintPage);
                documentToPrint.Print();
            }
        }
    }
}
