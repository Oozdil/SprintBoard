using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SprintBoard
{
    public class DbSql
    {
        private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=sprintboarddb;
                Integrated Security=true";
        public SqlConnection conn = new SqlConnection(connectionString);

        public int InsertNewTeamMember(TeamMember member)
        {
            int Aktif = 1;
            int lastid = -1;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO TeamMembers (Name,  Surname, Title, ImagePath, Aktif, Color)
                                        VALUES (@Name, @Surname, @Title, @ImagePath,@Aktif, @Color );
                                        SELECT CAST(scope_identity() AS int)";


                command.Parameters.AddWithValue("@Name", member.Name);
                command.Parameters.AddWithValue("@Surname", member.Surname);
                command.Parameters.AddWithValue("@Title", member.Title);
                command.Parameters.AddWithValue("@ImagePath", member.ImagePath);
                command.Parameters.AddWithValue("@Aktif", Aktif);
                command.Parameters.AddWithValue("@MemberID", member.MemberID);
                command.Parameters.AddWithValue("@Color", member.ColorCode);
                conn.Open();
                try
                {

                    lastid = (int)command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
            return lastid;
        }
        public void UpdateMember(TeamMember member)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE TeamMembers SET Name=@Name, Surname=@Surname,   Title=@Title ,
                                        ImagePath=@ImagePath, Aktif=@Aktif, Color=@Color WHERE MemberID=@MemberID";
                command.Parameters.AddWithValue("@Name", member.Name);
                command.Parameters.AddWithValue("@Surname", member.Surname);
                command.Parameters.AddWithValue("@Title", member.Title);
                command.Parameters.AddWithValue("@ImagePath", member.ImagePath);
                command.Parameters.AddWithValue("@Aktif", member.Aktif);
                command.Parameters.AddWithValue("@Color", member.ColorCode);
                command.Parameters.AddWithValue("@MemberID", member.MemberID);
                conn.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }


        }

        public int InsertNewStory(Story story)
        {
            int Status = 0;
            int lastid = -1;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Stories (StoryName, StoryText, StoryStart, Last, Status)
                                            VALUES (@StoryName, @StoryText, @StoryStart, @Last, @Status);SELECT CAST(scope_identity() AS int)";
                command.Parameters.AddWithValue("@StoryName", story.StoryName);
                command.Parameters.AddWithValue("@StoryText", story.StoryText);
                command.Parameters.AddWithValue("@StoryStart", story.StoryStart);
                command.Parameters.AddWithValue("@Last", story.Last);
                command.Parameters.AddWithValue("@Status", Status);
                conn.Open();
                try
                {

                    lastid = (int)command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
            return lastid;
        }
        public void UpdateStory(Story story)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE Stories SET StoryName=@StoryName,StoryText=@StoryText,Last=@Last,Status=@Status 
                                           WHERE StoryID=@StoryID";
                command.Parameters.AddWithValue("@StoryName", story.StoryName);
                command.Parameters.AddWithValue("@StoryText", story.StoryText);              
                command.Parameters.AddWithValue("@Last", story.Last);
                command.Parameters.AddWithValue("@Status", story.Status);
                command.Parameters.AddWithValue("@StoryID", (int)story.StoryID);
                conn.Open();
                try
                {

                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        public int InsertNewTask(StoryTask storytask)
        {
            int Status = 1;
            int lastid = -1;
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT into Tasks (StoryID, TeamMemberID, TaskName, TaskText, Start, Last,Status)
                                        VALUES (@StoryID, @TeamMemberID, @TaskName, @TaskText,  @Start, @Last,@Status);
                                        SELECT CAST(scope_identity() AS int)";
                command.Parameters.AddWithValue("@StoryID", storytask.StoryID);
                command.Parameters.AddWithValue("@TeamMemberID", storytask.MemberID);
                command.Parameters.AddWithValue("@TaskName", storytask.TaskName);
                command.Parameters.AddWithValue("@TaskText", storytask.TaskText);
               
                command.Parameters.AddWithValue("@Start", storytask.Start);
                command.Parameters.AddWithValue("@Last", storytask.Last);
                
                command.Parameters.AddWithValue("@Status", Status);
                conn.Open();
                try
                {

                    lastid = (int)command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();
            }
            return lastid;
        }
        public void UpdateTask(StoryTask storytask)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE Tasks SET StoryID=@StoryID, TeamMemberID=@TeamMemberID, TaskName=@TaskName,
                                       TaskText=@TaskText, Start=@Start, Last=@Last,Status=@Status
                                           WHERE TaskID=@TaskID";
                command.Parameters.AddWithValue("@StoryID", storytask.StoryID);
                command.Parameters.AddWithValue("@TeamMemberID", storytask.MemberID);
                command.Parameters.AddWithValue("@TaskName", storytask.TaskName);
                command.Parameters.AddWithValue("@TaskText", storytask.TaskText);              
                command.Parameters.AddWithValue("@Start", storytask.Start);
                command.Parameters.AddWithValue("@Last", storytask.Last);               
                command.Parameters.AddWithValue("@Status", storytask.Status);
                command.Parameters.AddWithValue("@TaskID", storytask.TaskID);
                conn.Open();
                try
                {

                    int recordsAffected = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }


        }

        public List<TeamMember> AllTeamMembers()
        {
            string queryString = "SELECT * FROM TeamMembers";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);

            DataSet memberdata = new DataSet();
            adapter.Fill(memberdata);
            TeamMember teammember = null;
            List<TeamMember> TeamMembers = new List<TeamMember>();
            foreach (DataRow row in memberdata.Tables[0].Rows)
            {
                teammember = new TeamMember
                {
                    MemberID = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    Surname = row[2].ToString(),
                    ImagePath = row[4].ToString(),
                    Title = row[3].ToString(),
                    Aktif = Convert.ToBoolean(row[5]),
                    ColorCode = row[6].ToString()
                };
                TeamMembers.Add(teammember);
            }
            return TeamMembers;

        }
        public List<Story> AllStories()
        {
            string queryString;
            queryString = "SELECT * FROM Stories ORDER BY StoryID DESC ";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
            DataSet storydata = new DataSet();
            adapter.Fill(storydata);
            Story story = null;
            List<Story> stories = new List<Story>();
            foreach (DataRow row in storydata.Tables[0].Rows)
            {
                story = new Story
                {
                    StoryID = Convert.ToInt32(row[0]),
                    StoryName = row[1].ToString(),
                    StoryText = row[2].ToString(),
                    StoryStart = row[3].ToString(),
                    Last = row[4].ToString(),
                    Status = Convert.ToInt32(row[5])
                };
                stories.Add(story);
            }
            return stories;
        }
        public List<StoryTask> AllTasks()
        {
            List<StoryTask> tasks = new List<StoryTask>();
            string queryString = @"SELECT dbo.Tasks.*, dbo.TeamMembers.Color FROM  dbo.Tasks INNER JOIN
                                  dbo.TeamMembers ON dbo.Tasks.TeamMemberID = dbo.TeamMembers.MemberID ";
            SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
            DataSet taskdata = new DataSet();
            adapter.Fill(taskdata);
            foreach (DataRow row in taskdata.Tables[0].Rows)
            {
                StoryTask task = new StoryTask()
                {
                    TaskID = Convert.ToInt32(row[0]),
                    StoryID = Convert.ToInt32(row[1]),
                    MemberID = Convert.ToInt32(row[2]),
                    TaskName = row[3].ToString(),
                    TaskText = row[4].ToString(),                    
                    Start = row[5].ToString(),
                    Last = row[6].ToString(),                    
                    Status = Convert.ToInt32(row[7]),
                    Color = row[8].ToString()

                };
                tasks.Add(task);
            }
            return tasks;
        }

    }
}