using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Flights
{
    // Lookup (dropdown) items. 

    #region Interface

    public interface ILookup
    {
        List<SelectListItem> ProjectTypes { get; }
        List<SelectListItem> IssueTypes { get; }
        List<SelectListItem> IssuePriorities { get; }
        List<SelectListItem> IssueStatuses { get; }

        List<SelectListItem> UserRoles { get; }
    }
    #endregion

    public class Lookup : ILookup
    {
        #region Items

        public List<SelectListItem> ProjectTypes
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- None --", Selected = true });
                list.Add(new SelectListItem { Value = "Business", Text = "Business" });
                list.Add(new SelectListItem { Value = "Software", Text = "Software" });
                list.Add(new SelectListItem { Value = "Service", Text = "Service" });

                return list;
            }
        }
        
        public List<SelectListItem> IssueTypes
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- None --", Selected = true });
                list.Add(new SelectListItem { Value = "Bug", Text = "Bug" });
                list.Add(new SelectListItem { Value = "Task", Text = "Task" });
                list.Add(new SelectListItem { Value = "Epic", Text = "Epic" });
                list.Add(new SelectListItem { Value = "New Feature", Text = "New Feature" });
                list.Add(new SelectListItem { Value = "Improvement", Text = "Improvement" });

                return list;
            }
        }

        public List<SelectListItem> IssuePriorities
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- None --", Selected = true });
                list.Add(new SelectListItem { Value = "Low", Text = "Low" });
                list.Add(new SelectListItem { Value = "Medium", Text = "Medium" });
                list.Add(new SelectListItem { Value = "High", Text = "High" });
                list.Add(new SelectListItem { Value = "Urgent", Text = "Urgent" });

                return list;
            }
        }

        public List<SelectListItem> IssueStatuses
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- None --", Selected = true });
                list.Add(new SelectListItem { Value = "New", Text = "New" });
                list.Add(new SelectListItem { Value = "In Progress", Text = "In Progress" });
                list.Add(new SelectListItem { Value = "In Review", Text = "In Review" });
                list.Add(new SelectListItem { Value = "Done", Text = "Done" });

                return list;
            }
        }
        
        public List<SelectListItem> UserRoles
        {
            get
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Value = "", Text = "-- None --", Selected = true });
                list.Add(new SelectListItem { Value = "User", Text = "User" });
                list.Add(new SelectListItem { Value = "Admin", Text = "Admin" });

                return list;
            }
        }

        #endregion
    }
}
