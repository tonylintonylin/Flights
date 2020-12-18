using Flights.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flights
{
    #region Interface

    public interface ICache
    {
        Dictionary<string, MetaType> MetaTypes { get; }
        
        string ProjectIcon { get; }
        string IssueIcon { get; }

        string AdminIcon { get; }
        string UserIcon { get; }
        string PeopleIcon { get; }

        Dictionary<int, Project> Projects { get; }
        Dictionary<int, Issue> Issues { get; }

        Dictionary<int, User> Users { get; }

        void ClearProjects();
        void MergeProject(Project project);
        void DeleteProject(Project project);

        void ClearIssues();
        void MergeIssue(Issue issue);
        void DeleteIssue(Issue issue);

        void ClearUsers();
        void MergeUser(User user);
        void DeleteUser(User user);

        void Clear();
    }

    #endregion

    public class Cache : ICache
    {
        #region Dependency Injection

        private readonly ICurrentUser _currentUser;
        private readonly IMemoryCache _memoryCache;
        private readonly FlightsContext _db;

        public Cache(ICurrentUser currentUser, IMemoryCache memoryCache, FlightsContext db)
        {
            _currentUser = currentUser;
            _memoryCache = memoryCache;
            _db = db;
        }

        #endregion

        #region Cache management

        private static object locker = new object();
        private static object mocker = new object(); // modifying locker

        private const string ProjectsKey = nameof(ProjectsKey);
        private const string IssuesKey = nameof(IssuesKey);

        private const string UsersKey = nameof(UsersKey);
        private const string MetaTypesKey = nameof(MetaTypesKey);

        // Keeps track of keys used
        private static readonly HashSet<string> UsedKeys = new HashSet<string>();

        #endregion

        #region MetaTypes

        public Dictionary<string, MetaType> MetaTypes
        {
            get
            {
                // ** Lazy load pattern 

                var dictionary = _memoryCache.Get(MetaTypesKey) as Dictionary<string, MetaType>;
                if (dictionary == null)
                {
                    lock (locker)
                    {
                        dictionary = new Dictionary<string, MetaType>();

                        dictionary.Add("Project", new MetaType { Name = "Project", Icon = "icon-folder icon-square icon-project", Url= "/projects" });
                        dictionary.Add("Issue", new MetaType { Name = "Issue", Icon = "icon-pin icon-square icon-issue", Url= "/issues" });

                        dictionary.Add("People", new MetaType { Name = "People", Icon = "icon-people icon-square icon-persons", Url = "/people" });

                        dictionary.Add("Admin", new MetaType { Name = "Admin", Icon = "icon-settings icon-square icon-admin", Url = "/admin" });
                        dictionary.Add("Owner", new MetaType { Name = "Owner", Icon = "icon-user icon-square icon-owner", Url = "/people" });
                        dictionary.Add("User", new MetaType { Name = "User", Icon = "icon-user icon-square icon-owner", Url = "/admin/users" });

                        Add(MetaTypesKey, dictionary, DateTime.Now.AddHours(24));
                    }
                }

                return dictionary;
            }
        }

        public string ProjectIcon { get { return MetaTypes["Project"].Icon; } }
        public string IssueIcon { get { return MetaTypes["Issue"].Icon; } }
        
        public string AdminIcon { get { return MetaTypes["Admin"].Icon; } }
        public string UserIcon { get { return MetaTypes["User"].Icon; } }
        public string PeopleIcon { get { return MetaTypes["People"].Icon; } }

        #endregion

        #region Projects

        public Dictionary<int, Project> Projects
        {
            get
            {
                // ** Lazy load pattern 

                if (!(_memoryCache.Get(ProjectsKey) is Dictionary<int, Project> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _db.Project.ToDictionary(c => c.Id);
                        Add(ProjectsKey, dictionary, DateTime.Now.AddHours(4));
                    }
                }

                return dictionary;
            }
        }

        // Clear Project cache

        public void ClearProjects()
        {
            lock (locker)
            {
                Clear(ProjectsKey);
            }
        }

        public void MergeProject(Project Project)
        {
            lock (mocker)
            {
                if (Project != null)
                    Projects[Project.Id] = Project;
            }
        }

        public void DeleteProject(Project Project)
        {
            lock (mocker)
            {
                if (Project != null)
                    Projects.Remove(Project.Id);
            }
        }

        #endregion

        #region Issues

        public Dictionary<int, Issue> Issues
        {
            get
            {
                // ** Lazy load pattern 

                if (!(_memoryCache.Get(IssuesKey) is Dictionary<int, Issue> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _db.Issue.ToDictionary(c => c.Id);
                        Add(IssuesKey, dictionary, DateTime.Now.AddHours(4));
                    }
                }

                return dictionary;
            }
        }

        // Clear Issue cache

        public void ClearIssues()
        {
            lock (locker)
            {
                Clear(IssuesKey);
            }
        }

        public void MergeIssue(Issue Issue)
        {
            lock (mocker)
            {
                if (Issue != null)
                    Issues[Issue.Id] = Issue;
            }
        }

        public void DeleteIssue(Issue Issue)
        {
            lock (mocker)
            {
                if (Issue != null)
                    Issues.Remove(Issue.Id);
            }
        }

        #endregion

        #region Users

        public Dictionary<int, User> Users
        {
            get
            {
                // ** Lazy load pattern 

                if (!(_memoryCache.Get(UsersKey) is Dictionary<int, User> dictionary))
                {
                    lock (locker)
                    {
                        dictionary = _db.User.OrderBy(u => u.LastName).ToDictionary(c => c.Id);
                        Add(UsersKey, dictionary, DateTime.Now.AddHours(4));
                    }
                }

                return dictionary;
            }
        }

        // Clear users cache

        public void ClearUsers()
        {
            lock (locker)
            {
                Clear(UsersKey);
            }
        }

        public void MergeUser(User user)
        {
            lock (mocker)
            {
                Users[user.Id] = user;
            }
        }

        public void DeleteUser(User user)
        {
            lock (mocker)
            {
                // No hard deletes
                Users[user.Id] = user;
            }
        }

        #endregion

        #region Cache Helpers

        // clears single cache entry

        private void Clear(string key)
        {
            lock (locker)
            {
                _memoryCache.Remove(key);

                UsedKeys.Remove(key);
            }
        }

        // clears entire cache

        public void Clear()
        {
            // only host is allowed to clear entire cache

            if (!_currentUser.IsAdmin) return;

            lock (locker)
            {
                foreach (var usedKey in UsedKeys)
                    _memoryCache.Remove(usedKey);

                UsedKeys.Clear();
            }
        }

        // add to cache 

        private void Add(string key, object value, DateTimeOffset expiration)
        {
            _memoryCache.Set(key, value,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration));

            UsedKeys.Add(key);
        }

        #endregion
    }

    public class MetaType
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
    }
}
