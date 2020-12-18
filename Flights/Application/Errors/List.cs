using Flights.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Application.Errors
{
    public class List : PagedModel<Detail>
    {
        #region Data

        public string DeleteCount { get; set; }

        #endregion

        #region Handlers

        public override async Task<IActionResult> GetAsync()
        {
            TotalRows = await _db.Error.CountAsync();

            var items = _db.Error
                           .Include(e => e.User)
                           .OrderByDescending(e => e.ErrorDate)
                           .Skip(Skip).Take(Take);

            _mapper.Map(items, Items);

            return View(this);
        }

        #endregion

        #region Mapping

        public class MapperProfile : BaseProfile
        {
            public MapperProfile()
            {
                CreateMap<Error, Detail>()
                  .Map(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.ErrorDate.ToDateTime()))
                  .Map(dest => dest.UserName, opt => opt.MapFrom(src => src.UserId != null ? _cache.Users[src.UserId.Value].Name : ""));
            }
        }

        #endregion
    }
}