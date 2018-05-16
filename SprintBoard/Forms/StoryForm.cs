using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SprintBoard
{

    public partial class StoryForm : Form
    {
        public Story story;
        public StoryForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            story = null;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txBxName.Text == "" || rTxBxText.Text == "")
            {
                MessageBox.Show("Story name or explanation text can not be empty!");
                return;
            }




            if (story == null)
            {
                SaveStory();
            }
            else
            {
                UpdateStory();

            }


            this.Close();
        }

        private void UpdateStory()
        {
            story.StoryName = txBxName.Text;
            story.StoryText = rTxBxText.Text;
            story.Last = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void SaveStory()
        {
            story = new Story()
            {
                StoryName = txBxName.Text,
                StoryText = rTxBxText.Text,
                StoryStart = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                Last = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                Status = 0

            };
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            story = null;
            this.Close();
        }

     

        private void StoryForm_Load(object sender, EventArgs e)
        {
            if (story != null)
            {

                txBxName.Text = story.StoryName;
                rTxBxText.Text = story.StoryText;
                lbl_start.Text = story.StoryStart;
                lbl_lastupdate.Text = story.Last;
            }
            else
            {
                lbl_start.Text = "Not started!";
                lbl_lastupdate.Text = "Not started!";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (story != null)
            {
                DialogResult result = MessageBox.Show("Story Delete Action !!!", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    story.Status = 2;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Story is not saved !");
            }

        }
    }
}
