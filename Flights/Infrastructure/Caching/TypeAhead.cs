using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Flights
{
    // Provides search dropdown typeahead

    #region Interface

    public interface ITypeahead
    {
        string Projects { get; }
        string Issues { get; }

        string Owners { get; }
    }

    #endregion

    public class Typeahead : ITypeahead
    {
        #region Dependency Injection

        private readonly ICache _cache;

        public Typeahead(ICache cache)
        {
            _cache = cache;
        }

        #endregion

        #region Items

        public string Projects
        {
            get
            {
                var list = new List<TypeaheadItem>();
                foreach (var project in _cache.Projects.Values)
                    list.Add(new TypeaheadItem { id = project.Id, name = project.Title, icon = _cache.ProjectIcon, type = "Project" });

                return JsonConvert.SerializeObject(list);
            }
        }

        public string Issues
        {
            get
            {
                var list = new List<TypeaheadItem>();
                foreach (var issue in _cache.Issues.Values)
                    list.Add(new TypeaheadItem { id = issue.Id, name = issue.Title, icon = _cache.IssueIcon, type = "Issue" });

                return JsonConvert.SerializeObject(list);
            }
        }

        public string Owners
        {
            get
            {
                var list = new List<TypeaheadItem>();
                foreach (var user in _cache.Users.Values.Where(a => a.IsDeleted == false))
                    list.Add(new TypeaheadItem { id = user.Id, name = user.Name, icon = _cache.UserIcon, type = "User" });

                return JsonConvert.SerializeObject(list);
            }
        }

        private class TypeaheadItem
        {
            public int id { get; set; }
            public string name { get; set; }
            public string icon { get; set; }
            public string type { get; set; }
        }

        #endregion
    }
}
