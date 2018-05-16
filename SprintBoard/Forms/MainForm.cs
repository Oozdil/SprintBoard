using SprintBoard.Forms;
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
    public partial class MainForm : Form
    {
        //Variables
        DbSql dbsql;
        Board sprintBoard;
        Panel[,] boardpanels;
        Label[,] boardlabels;
        bool showCompletedStories;
        string storySearchfilter;
        int currentPage;
        int totalPages;
        int storyrecordcount;
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            dbsql = new DbSql();
            sprintBoard = new Board();
            BoardSettings();
            showCompletedStories = false;
            storySearchfilter = "";
            currentPage = 1;
            totalPages = 0;
            storyrecordcount = 0;
            LoadMembers();

            RefreshBoard();
        }

      

        /*-----Buton Clicks--------------------------------------------------------*/
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutform = new AboutForm();
            aboutform.ShowDialog();
        }
        private void btnNewMember_Click(object sender, EventArgs e)
        {
            MemberForm newmemberform = new MemberForm();
            newmemberform.sprintBoard = sprintBoard;
            newmemberform.ShowDialog();
            TeamMember tm = newmemberform.teammember;
            if (tm != null)
            {
                dbsql.InsertNewTeamMember(tm);
                MessageBox.Show("Member added succesfully");

                LoadMembers();
                RefreshBoard();
            }

        }
        private void btnAddNewStory_Click(object sender, EventArgs e)
        {
            StoryForm storyform = new StoryForm();
            storyform.ShowDialog();
            if (storyform.story != null)
            {
                dbsql.InsertNewStory(storyform.story);
                MessageBox.Show("Story saved !");
                RefreshBoard();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            storySearchfilter = txBxSearch.Text;
            currentPage = 1;
            RefreshBoard();
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (currentPage != 1)
            {
                currentPage = 1;
                RefreshBoard();
            }

        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                RefreshBoard();
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                RefreshBoard();
            }
        }
        private void btnLast_Click(object sender, EventArgs e)
        {
            if (currentPage != totalPages)
            {
                currentPage = totalPages;
                RefreshBoard();
            }

        }
        private void team_Member_Details_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;
            int memberid = Convert.ToInt32(clickedPictureBox.Name.Replace("pic_", ""));
            MemberForm memberform = new MemberForm();
            memberform.sprintBoard = sprintBoard;
            foreach (TeamMember t in sprintBoard.TeamMembers)
            {
                if (t.MemberID == memberid)
                {
                    memberform.teammember = t;
                }
            }
            memberform.ShowDialog();
            if (memberform.teammember != null)
            {
                if (memberform.teammember.Aktif)
                {
                    MessageBox.Show("Member updated succesfully");
                }
                else
                {
                    MessageBox.Show("Member deleted succesfully");
                }
                dbsql.UpdateMember(memberform.teammember);
                currentPage = 1;
                LoadMembers();
                RefreshBoard();

            }

        }
        private void btn_report_Click(object sender, EventArgs e)
        {
            ReportForm reportform = new ReportForm();
            reportform.sprintboard = sprintBoard;
            reportform.ShowDialog();
        }


        /*-------------------------------------------------------------*/
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToShortDateString();
        }
        private void BoardSettings()
        {
            boardpanels = new Panel[,]
                {
                 { pnl_story_1,pnl_todo_1,pnl_inprog_1,pnl_done_1},
                 { pnl_story_2,pnl_todo_2,pnl_inprog_2,pnl_done_2},
                 { pnl_story_3,pnl_todo_3,pnl_inprog_3,pnl_done_3},
                 { pnl_story_4,pnl_todo_4,pnl_inprog_4,pnl_done_4},
                };
            boardlabels = new Label[,]
                {
                 { lbl_story_id_1,lbl_story1_summary},
                 { lbl_story_id_2,lbl_story2_summary},
                 { lbl_story_id_3,lbl_story3_summary},
                 { lbl_story_id_4,lbl_story4_summary}
                };
            toolTip1.SetToolTip(btn_add_task1, "Add New Task");
            toolTip2.SetToolTip(btn_add_task2, "Add New Task");
            toolTip3.SetToolTip(btn_add_task3, "Add New Task");
            toolTip4.SetToolTip(btn_add_task4, "Add New Task");
            toolTip5.SetToolTip(btn_delete_story1, "Delete Story");
            toolTip6.SetToolTip(btn_delete_story2, "Delete Story");
            toolTip7.SetToolTip(btn_delete_story3, "Delete Story");
            toolTip8.SetToolTip(btn_delete_story4, "Delete Story");
            MakePanelsDraggable();
            ClearBoard();
        }
        private void MakePanelsDraggable()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    boardpanels[i, j].AllowDrop = true;
                    boardpanels[i, j].DragEnter += panel_DragEnter;
                    boardpanels[i, j].DragDrop += panel_DragDrop;
                }
            }
            pnl_Trash.AllowDrop = true;
            pnl_Trash.DragEnter += panel_DragEnter;
            pnl_Trash.DragDrop += panel_DragDrop;
        }
        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void ClearBoard()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    boardpanels[i, j].Controls.Clear();
                }

                for (int k = 0; k < 2; k++)
                {
                    boardlabels[i, k].Text = "";
                }

                foreach (Control c in boardpanels[i, 0].Controls)
                {
                    c.Visible = false;
                }
                boardpanels[i, 0].Enabled = false;
                boardpanels[i, 1].Enabled = false;
                boardpanels[i, 2].Enabled = false;
                boardpanels[i, 3].Enabled = false;
                lblPageStatus.Text = "No record";
            }
        }
        private void chBxCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (chBxCompleted.Checked)
            {
                showCompletedStories = true;
            }
            else
            {
                showCompletedStories = false;
            }
            RefreshBoard();
        }
        private void LoadMembers()
        {
            sprintBoard.TeamMembers.Clear();
            pnlTeamMembers.Controls.Clear();
            sprintBoard.TeamMembers = dbsql.AllTeamMembers();
            foreach (TeamMember tm in sprintBoard.TeamMembers)
            {
                if (tm.Aktif != false)
                    AddTeamMemberToPanel(tm);
            }
        }
        private void AddTeamMemberToPanel(TeamMember tm)
        {
            Panel newmember = new Panel();
            newmember.Width = 237;
            newmember.Height = 100;


            //convert rgb to Color
            string colorcode = tm.ColorCode;
            int r = Convert.ToInt32(colorcode.Split(',')[0]);
            int g = Convert.ToInt32(colorcode.Split(',')[1]);
            int b = Convert.ToInt32(colorcode.Split(',')[2]);
            Color tmColor = Color.FromArgb(r, g, b);



            newmember.BackColor = tmColor;
            newmember.Dock = DockStyle.Top;
            newmember.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            PictureBox pic = new PictureBox();
            pic.Name = "pic_" + tm.MemberID.ToString();
            pic.Width = 82;
            pic.Height = 86;

            if (tm.ImagePath == "")
            {
                pic.BackgroundImage = Properties.Resources.member;
            }
            else
            {
                pic.BackgroundImage = Image.FromFile(@"MemberImages\" + tm.ImagePath);
            }


            pic.Left = 5;
            pic.Top = 5;
            pic.BackgroundImageLayout = ImageLayout.Stretch;
            newmember.Controls.Add(pic);
            pic.Click += team_Member_Details_Click;

            Label name = new Label();
            name.Text = tm.Name + " " + tm.Surname;
            name.AutoSize = false;
            name.Width = 134;
            name.Height = 40;
            name.BackColor = System.Drawing.Color.White;
            name.Left = 93;
            name.Top = 5;
            name.TextAlign = ContentAlignment.MiddleCenter;
            newmember.Controls.Add(name);

            Label title = new Label();
            title.Text = tm.Title;
            title.AutoSize = false;
            title.Width = 134;
            title.Height = 40;
            title.BackColor = System.Drawing.Color.White;
            title.Left = 93;
            title.Top = 50;
            title.TextAlign = ContentAlignment.MiddleCenter;
            newmember.Controls.Add(title);


            pnlTeamMembers.Controls.Add(newmember);

        }
        private void ShowStoryDetails(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            int story_id = 0;
            foreach (Control c in panel.Controls)
            {
                if (c.Name.Length > 13 && c.Name.Substring(0, 13) == "lbl_story_id_")
                    story_id = Convert.ToInt32(c.Text);
            }
            StoryForm storyform = new StoryForm();
            foreach (Story st in sprintBoard.Stories)
            {
                if (st.StoryID == story_id)
                    storyform.story = st;
            }
            storyform.ShowDialog();
            if (storyform.story != null)
            {
                dbsql.UpdateStory(storyform.story);
                RefreshBoard();
            }
        }
        private void DeleteStory(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to move story to the trash? ", "Story Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Button addbutton = sender as Button;
                int story_rank = Convert.ToInt32(addbutton.Name.Replace("btn_delete_story", ""));
                int story_id = Convert.ToInt32(boardlabels[story_rank - 1, 0].Text);

                foreach (Story st in sprintBoard.Stories)
                {
                    if (st.StoryID == story_id)
                    {
                        Story story = st;
                        st.Status = 2;
                        dbsql.UpdateStory(story);
                    }
                }
                currentPage = 1;
                RefreshBoard();
            }
        }
        private void RefreshBoard()
        {
            ClearBoard();
            LoadTasks();
            LoadStories();
        }
        private void LoadTasks()
        {
            sprintBoard.StoryTasks.Clear();
            sprintBoard.StoryTasks = dbsql.AllTasks();
        }
        private void LoadStories()
        {

            sprintBoard.Stories.Clear();
            storySearchfilter = storySearchfilter.Trim();
            foreach (Story st in dbsql.AllStories())
            {
                if (st.StoryName.Contains(storySearchfilter) || st.StoryText.Contains(storySearchfilter))
                {
                    if (showCompletedStories)
                    {
                        sprintBoard.Stories.Add(st);
                    }
                    else
                    {
                        if (st.Status < 1)
                            sprintBoard.Stories.Add(st);
                    }
                }
            }
            storyrecordcount = sprintBoard.Stories.Count;
            totalPages = Convert.ToInt32(Math.Ceiling(storyrecordcount / (double)4));

            if (totalPages > 0)
                lblPageStatus.Text = (currentPage).ToString() + " / " + totalPages.ToString() + " Pages";
            else
                lblPageStatus.Text = "No record";


            int firstrecord = (currentPage - 1) * 4;
            int lastrecord = (currentPage - 1) * 4 + 3;
            if (lastrecord >= storyrecordcount)
                lastrecord = storyrecordcount - 1;

            for (int i = firstrecord; i <= lastrecord; i++)
            {

                boardlabels[i % 4, 0].Text = sprintBoard.Stories[i].StoryID.ToString();
                boardlabels[i % 4, 1].Text = sprintBoard.Stories[i].StoryName;
                int story_satus = sprintBoard.Stories[i].Status;

                if (story_satus < 1)
                {
                    boardpanels[i % 4, 0].Enabled = true;
                    boardpanels[i % 4, 1].Enabled = true;
                    boardpanels[i % 4, 2].Enabled = true;
                    boardpanels[i % 4, 3].Enabled = true;
                }
                else
                {
                    boardlabels[i % 4, 0].Text = " Completed";
                }
                foreach (Control c in boardpanels[i % 4, 0].Controls)
                {
                    c.Visible = true;
                }

                foreach (StoryTask st in sprintBoard.StoryTasks)
                {
                    if (st.StoryID == sprintBoard.Stories[i].StoryID && st.Status != 4)
                    {
                        AddTaskToPanel(boardpanels[i % 4, Convert.ToInt32(st.Status)], st);
                    }
                }
            }
        }
        private void AddTaskToPanel(Panel panel, StoryTask st)
        {
            Panel newtask = new Panel();

            newtask.Name = panel.Name.Split('_')[2] + "_" + st.StoryID + "_" + st.TaskID;
            newtask.Width = 50;
            newtask.Height = 30;

            //convert rgb to Color
            string colorcode = st.Color;
            int r = Convert.ToInt32(colorcode.Split(',')[0]);
            int g = Convert.ToInt32(colorcode.Split(',')[1]);
            int b = Convert.ToInt32(colorcode.Split(',')[2]);
            Color stColor = Color.FromArgb(r, g, b);
            Button detay = new Button();
            detay.Name = st.TaskID.ToString();
            detay.Text = "?";
            detay.Click += task_Detail_Click;
            detay.Width = 30;
            detay.Height = 30;
            detay.Font = new Font("Arial", 15, FontStyle.Bold);
            detay.Dock = DockStyle.Right;
            newtask.Controls.Add(detay);
            Label Text = new Label();
            Text.Dock = DockStyle.Left;
            Text.Text = st.TaskName;
            Text.Font = new Font("Arial", 12, FontStyle.Bold);
            newtask.Controls.Add(Text);

            newtask.BackColor = stColor;
            newtask.Dock = DockStyle.Top;
            newtask.Text = st.TaskName;

            newtask.MouseDown += task_panel_MouseDown;
            newtask.AllowDrop = true;
            panel.Controls.Add(newtask);
        }
        private void task_panel_MouseDown(object sender, MouseEventArgs e)
        {
            Panel button = sender as Panel;
            button.DoDragDrop(button, DragDropEffects.Move);
        }
        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            Panel pnl_host = (Panel)sender;
            Panel pnl_task = ((Panel)e.Data.GetData(typeof(Panel)));

            int taskid = Convert.ToInt32(pnl_task.Name.ToString().Split('_')[2]);

            StoryTask storytaskToUpdate = null;
            foreach (StoryTask stT in sprintBoard.StoryTasks)
            {
                if (stT.TaskID == taskid)
                    storytaskToUpdate = stT;
            }


            if (pnl_host.Name == "pnl_Trash")
            {

                DialogResult result = MessageBox.Show("Do you want to move the task to the trash? ", "Task Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    pnl_task.Parent = pnl_host;
                    pnl_host.Controls.RemoveAt(1);
                    //deleted status
                    storytaskToUpdate.Status = 4;
                }
            }


            else
            {
                string pnl_host_id = pnl_host.Name.Split('_')[2];
                string pnl_task_id = pnl_task.Name.ToString().Split('_')[0];
                if (pnl_host_id == pnl_task_id)
                {
                    pnl_task.Parent = pnl_host;
                    string new_status = pnl_host.Name.Split('_')[1];
                    switch (new_status)
                    {
                        case "todo":
                            storytaskToUpdate.Status = 1;
                            break;
                        case "inprog":
                            storytaskToUpdate.Status = 2;
                            break;
                        case "done":
                            storytaskToUpdate.Status = 3;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    storytaskToUpdate = null;
                }
            }


            if (storytaskToUpdate != null)
            //update task    
            {
                dbsql.UpdateTask(storytaskToUpdate);
                checkIfStoryIsFinished(storytaskToUpdate);
            }



        }
        private void checkIfStoryIsFinished(StoryTask stT)
        {
            int count_unfinishedtasks = 0;

            foreach (StoryTask stTask in sprintBoard.StoryTasks)
            {
                if (stTask.StoryID == stT.StoryID && stTask.Status < 3)
                    count_unfinishedtasks++;

            }
            if (count_unfinishedtasks == 0)
            {
                DialogResult result = MessageBox.Show("Do you want to change the story to completed? ",
                    "Story Finished", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Story story = new Story();
                    foreach (Story st in sprintBoard.Stories)
                    {
                        if (st.StoryID == stT.StoryID)
                            story = st;
                    }
                    story.Status = 1;
                    dbsql.UpdateStory(story);
                    currentPage = 1;
                    RefreshBoard();
                }

            }
        }
        private void AddNewTaskClick(object sender, EventArgs e)
        {
            Button addbutton = sender as Button;
            int row_ind = Convert.ToInt32(addbutton.Name.Replace("btn_add_task", "")) - 1;
            int story_id = Convert.ToInt32(boardlabels[row_ind, 0].Text);
            string storyname = boardlabels[row_ind, 1].Text;
            TaskForm taskform = new TaskForm();
            taskform.storyId = story_id;
            taskform.storyName = storyname;
            taskform.sprintboard = sprintBoard;
            taskform.ShowDialog();

            StoryTask stt = taskform.storyTask;
            if (stt != null)
            {
                if (stt.StoryID > 0)
                {
                    MessageBox.Show("Task inserted successfully!");
                    dbsql.InsertNewTask(stt);
                    LoadMembers();
                    RefreshBoard();
                }
            }            
        }
        private void task_Detail_Click(object sender, EventArgs e)
        {
            Button details_button = sender as Button;
            int taskid = Convert.ToInt32(details_button.Name.Replace("btn_add_task", ""));
            TaskForm taskform = new TaskForm();
            foreach (StoryTask stt in sprintBoard.StoryTasks)
            {
                if (stt.TaskID == taskid)
                    taskform.storyTask = stt;

            }
            taskform.sprintboard = sprintBoard;
            taskform.ShowDialog();

            if (taskform.storyTask != null)
            {
                dbsql.UpdateTask(taskform.storyTask);
                LoadMembers();
                RefreshBoard();
            }

        }

       



    }
}
