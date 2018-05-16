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

    public partial class TaskForm : Form
    {
        public int storyId;
        public string storyName;
        public StoryTask storyTask;
        public Board sprintboard;
        public List<TeamMember> members;
        public TaskForm()
        {
            InitializeComponent();
        }

        private void TaskForm_Load(object sender, EventArgs e)
        {
            lblHeaderText.Text = "Story Name :" + storyName;
            members = new List<TeamMember>();
            cmBxMembers.Items.Add("Please select a member . . .");
            foreach (TeamMember tm in sprintboard.TeamMembers)
            {
                if (tm.Aktif == true)
                {
                    members.Add(tm);
                    cmBxMembers.Items.Add(tm.Name + " " + tm.Surname);
                }
            }
            cmBxMembers.SelectedIndex = 0;
            if (storyTask != null)
            {
                foreach (Story sty in sprintboard.Stories)
                {
                    if (sty.StoryID == storyTask.StoryID)
                        lblHeaderText.Text = "Story Name :" + sty.StoryName;
                }

                txBxName.Text = storyTask.TaskName;
                rTxBxText.Text = storyTask.TaskText;
                lbl_start.Text = storyTask.Start;
                lbl_lastupdate.Text = storyTask.Last;
                for (int i = 0; i < members.Count; i++)
                {
                    if (members[i].MemberID == storyTask.MemberID)
                        cmBxMembers.SelectedIndex = i + 1;
                }
            }
            else
            {
                lbl_start.Text = "Not started";
                lbl_lastupdate.Text = "Not started";
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            storyTask = null;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            storyTask = null;
            this.Close();
        }



        private void UpdateStory()
        {
            storyTask.TaskName = txBxName.Text;
            storyTask.TaskText = rTxBxText.Text;
            storyTask.Last = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            storyTask.Color = members[cmBxMembers.SelectedIndex - 1].ColorCode;
            storyTask.MemberID = members[cmBxMembers.SelectedIndex - 1].MemberID;
        }

        private void SaveStory()
        {
            storyTask = new StoryTask()
            {
                TaskName = txBxName.Text,
                TaskText = rTxBxText.Text,
                Start = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                Last = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(),
                StoryID = storyId,
                Color = members[cmBxMembers.SelectedIndex - 1].ColorCode,
                MemberID = members[cmBxMembers.SelectedIndex - 1].MemberID

            };
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txBxName.Text == "" || rTxBxText.Text == "")
            {
                MessageBox.Show("Story name or explanation text can not be empty!");
                return;
            }




            if (cmBxMembers.SelectedIndex == 0)
            {
                MessageBox.Show("Please check member selection!");
                return;
            }

            if (storyTask == null)
            {
                SaveStory();
            }
            else
            {
                UpdateStory();

            }


            this.Close();
        }
    }
}
