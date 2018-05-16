using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintBoard
{
    public class Board
    {
        public List<Story> Stories;
        public List<StoryTask> StoryTasks;
        public List<TeamMember> TeamMembers;
        public Board()
        {

          Stories= new List<Story>();
          StoryTasks=new List<StoryTask>();
          TeamMembers= new List<TeamMember>();
        }
    }
}
