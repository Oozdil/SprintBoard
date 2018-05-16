using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SprintBoard
{
    public partial class MemberForm : Form
    {
        string folderPath = "MemberImages";
        Image file;
        public TeamMember teammember;       
        public Board sprintBoard;
        public List<string> taskStat;




        public MemberForm()
        {
            InitializeComponent();
        }

        private void NewMemberForm_Load(object sender, EventArgs e)
        {
          
            System.IO.Directory.CreateDirectory(folderPath);

            taskStat = new List<string>()
            {
            "Not Started",
            "To Do",
            "In Progress",
            "Done",
            "Deleted"
            };


            /*Teammember'a tıklanarak form çağrıldı
             *Silme veya güncelleme yapılacak
             */
            if (teammember != null)
            {
                txBxName.Text = teammember.Name;
                txBxSurName.Text = teammember.Surname;
                txBxTitle.Text = teammember.Title;

                string colorcode = teammember.ColorCode;
                int r = Convert.ToInt32(colorcode.Split(',')[0]);
                int g = Convert.ToInt32(colorcode.Split(',')[1]);
                int b = Convert.ToInt32(colorcode.Split(',')[2]);
                Color tmColor = Color.FromArgb(r, g, b);

                picBxColor.BackColor = tmColor;
                colorDialog1.Color = tmColor;

                if (teammember.ImagePath == "")
                {
                    pixBxImage.BackgroundImage = Properties.Resources.member;
                }
                else
                {
                    pixBxImage.BackgroundImage = Image.FromFile(@"MemberImages\" + teammember.ImagePath);
                }

                
                dataGridView1.ColumnCount = 3;
                dataGridView1.Columns[0].HeaderText = "Story ID";
                dataGridView1.Columns[1].HeaderText = "Task Name";
                dataGridView1.Columns[2].HeaderText = "Status";
                dataGridView1.Columns[0].Width = 125;
                dataGridView1.Columns[1].Width = 125;
                dataGridView1.Columns[2].Width = 100;
                int counter = 0;
                foreach (StoryTask stTsk in sprintBoard.StoryTasks)
                {
                    if (stTsk.MemberID == teammember.MemberID)
                    {
                        dataGridView1.Rows.Add(stTsk.StoryID, stTsk.TaskName, taskStat[(int)stTsk.Status]);
                        dataGridView1.Rows[counter].DefaultCellStyle.BackColor = Color.LightBlue;
                        counter++;
                    }

                }
                dataGridView1.ClearSelection();
            }
        }

        //Form kapatılıyor ve tüm işlemler sıfırlanıyor
        private void btnClose_Click(object sender, EventArgs e)
        {
            teammember = null;
            this.Close();
        }

        //Colordialog açılıyor
        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            picBxColor.BackColor = colorDialog1.Color;



        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string imagename = "";

            //Mevcut kayıt güncelleniyor
            if (teammember == null)
            {


                if (openFileDialog1.FileName != "openFileDialog1")
                {
                    imagename = System.IO.Path.GetFileName(openFileDialog1.FileName);
                    file.Save(folderPath + "/" + imagename);
                }

                if (txBxName.Text == "" || txBxSurName.Text == "" || txBxTitle.Text == "" || picBxColor.BackColor == Color.White)
                {
                    MessageBox.Show("Name,Surname,Color selection or Title can not be empty!");
                }

                else
                {
                    teammember = new TeamMember
                    {
                        Name = txBxName.Text,
                        Surname = txBxSurName.Text,
                        ImagePath = imagename,
                        Title = txBxTitle.Text,
                        ColorCode = colorDialog1.Color.R.ToString() + "," + colorDialog1.Color.G.ToString() + "," + colorDialog1.Color.B.ToString(),
                        Aktif = true
                    };

                    this.Close();
                }
            }
            //Yeni kayıt giriliyor
            else
            {
                if (openFileDialog1.FileName != "openFileDialog1")
                {

                    imagename = System.IO.Path.GetFileName(openFileDialog1.FileName);
                    MessageBox.Show(teammember.ImagePath);
                    try
                    {
                        File.Delete(folderPath + "/" + teammember.ImagePath);
                        file.Save(folderPath + "/" + teammember.MemberID.ToString() + imagename);
                        teammember.ImagePath = teammember.MemberID.ToString() + imagename;

                    }
                    catch
                    { }

                }

                teammember.Name = txBxName.Text;
                teammember.Surname = txBxSurName.Text;
                teammember.Title = txBxTitle.Text;
                teammember.ColorCode = colorDialog1.Color.R.ToString() + "," + colorDialog1.Color.G.ToString() + "," + colorDialog1.Color.B.ToString();
                teammember.Aktif = true;
                this.Close();

            }

        }

        //İşlemlerden vazgeçiliyor
        private void btnCancel_Click(object sender, EventArgs e)
        {
            teammember = null;
            this.Close();
        }

        //picturebox içine seçilen resim atanıyor
        private void btnUpload_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                pixBxImage.BackgroundImage = file;
            }
        }

        //Teammember silme
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure ?", "Delete Operation!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int aktiftasks = dataGridView1.Rows.Count;
                //Eğer tamamlanmamış görev varsa teammember silinemeze
                if (aktiftasks > 0)
                {
                    MessageBox.Show("Member has undone task to do!");
                }

                else
                {
                    teammember.Aktif = false;
                    this.Close();
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
