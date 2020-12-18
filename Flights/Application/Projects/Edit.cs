using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Flights.Application.Projects
{
    public class Edit : BaseModel
    {
        #region Data

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public int? OwnerId { get; set; }
        public string OwnerAlias { get; set; }
        public string OwnerName { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            if (Id == 0)
            {
                OwnerId = _currentUser.Id;
                OwnerName = _currentUser.Name;
            }
            else
            {
                _mapper.Map(await _db.Project.SingleOrDefaultAsync(c => c.Id == Id), this);
            }

            return View(this);
        }

        public override async Task<IActionResult> PostAsync()
        {
            if (!ModelState.IsValid) return View(this);

            if (Id == 0) // new item
            {
                var project = _mapper.Map<Project>(this);
                project.CreatedDate = DateTime.Now;

                _db.Project.Add(project);
                await _db.SaveChangesAsync();

                await SettleInsertAsync(project);
            }
            else
            {
                var project = await _db.Project.SingleOrDefaultAsync(c => c.Id == Id);
                var originalProject = new OriginalProject(project);

                _mapper.Map(this, project);

                _db.Project.Update(project);
                await _db.SaveChangesAsync();

                await SettleUpdateAsync(originalProject, project);
            }

            return LocalRedirect(Referer ?? "/projects");
        }

        #endregion

        #region Helpers

        private async Task SettleInsertAsync(Project project)
        {
            _cache.MergeProject(project);

            await _rollup.RollupUserAsync(project.OwnerId);
        }

        private async Task SettleUpdateAsync(OriginalProject original, Project project)
        {
            _cache.MergeProject(project);

            if (original.OwnerId != project.OwnerId)
            {
                await _rollup.RollupUserAsync(original.OwnerId);
                await _rollup.RollupUserAsync(project.OwnerId);
            }
        }

        private class OriginalProject
        {
            // Used to temporarily hold a copy of the relevant fields

            public int OwnerId { get; set; }

            public OriginalProject(Project project)
            {
                OwnerId = project.OwnerId;
            }
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<Project, Edit>()
                   .Map(dest => dest.OwnerName, opt => opt.MapFrom(src => src.OwnerId == 0 ? "" : _cache.Users[src.OwnerId].Name));

                CreateMap<Edit, Project>()
                   .Map(dest => dest.OwnerAlias, opt => opt.MapFrom(src => _cache.Users[src.OwnerId.Value].Alias));
            }
        }

        #endregion
    }
}
