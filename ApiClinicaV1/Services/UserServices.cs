using ApiClinicaV1.Dtos;
using ApiClinicaV1.Models.config;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaV1.Services
{
   
       public interface IUserService
        {
            Task<UserDto> GetUserByIdAsync(int userId);
        }

        public class UserServices : IUserService
        {

        private readonly AppDbContext _context;

        public UserServices(  AppDbContext context)
            {
                _context = context;
            }

            public async Task<UserDto> GetUserByIdAsync(int userId)
            {
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        Name = u.Name
                    })
                    .FirstOrDefaultAsync();

                return user;
            }
        }
    }
