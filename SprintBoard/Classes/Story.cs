using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SprintBoard
{
    public class Story
    {
        public int StoryID;
        public string StoryName;
        public string StoryText;
        public string StoryStart;
        public string Last;
        public List<StoryTask> Tasks;
        public int Status;


        public Story()
        {
            Tasks = new List<StoryTask>();
        }
       
    }
}
