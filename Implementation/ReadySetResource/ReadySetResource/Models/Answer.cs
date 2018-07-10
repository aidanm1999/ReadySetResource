//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the shift 

namespace ReadySetResource.Models
{

    using System;
    using System.Collections.Generic;
    
    public partial class Answer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Description { get; set; }
        public int Points { get; set; }
        public Nullable<int> Question_Id { get; set; }
        public string User_Id { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Question Question { get; set; }

    }
}
