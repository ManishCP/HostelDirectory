using HostelDirectoryMvvM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelDirectoryMvvM.ViewModels
{
    public class DeleteMessage
    {
        public string StudentID { get; }

        public DeleteMessage(string studentID)
        {
            StudentID = studentID;
        }
    }
}
