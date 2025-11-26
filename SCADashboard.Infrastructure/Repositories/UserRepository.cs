using Microsoft.EntityFrameworkCore;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SCADashboardDbContext context) : base(context)
        {

        }

        public async Task<bool> GetUserEmailAndUserNameAsync(string UserName, string Email,int UserId)
        {
            try
            {

                // ✅ Validate inputs early
                if (string.IsNullOrWhiteSpace(UserName) && string.IsNullOrWhiteSpace(Email))
                    return true;

                // ✅ Query only once, filter by name or email
                var existingUser = await _context.Users.AsNoTracking()
                    .Where(u => (u.Username == UserName || u.Email == Email) && u.UserId != UserId)
                    .Select(u => new { u.Username, u.Email })
                    .FirstOrDefaultAsync();

                // ✅ If no match found
                if (existingUser == null)
                    return true;

                // ✅ Return proper message depending on which field matches
                string message = existingUser.Username == UserName
                    ? "Username already exists."
                    : "Email already exists.";

                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            


        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int TenantId)
        {
            return await _context.Users
                .Where(u => u.IsActive && u.TenantId==TenantId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<User> GetUsersIncludeTablesByIdAsync(int id)
        {
            return await _context.Users
                .Where(u => u.IsActive && u.UserId == id)
            .Include(u => u.Userkundes)
            .ThenInclude(uk=>uk.Kunde)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        public override async Task<User> AddAsync(User user)
        {
            if (user.UserId != 0)
            {
                //2️ Update to UserRoles table
                var existingUserRole = await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(ur => ur.UserId == user.UserId);
                if (existingUserRole != null)
                {
                    existingUserRole.RoleId = user.RoleId;
                    _context.UserRoles.Update(existingUserRole);
                }
                //2️ Update to UserKundes table
                var existingKunde = await _context.Userkundes.AsNoTracking().Where(uk => uk.UserId == user.UserId).ToListAsync();
                if (existingKunde != null)
                {
                    //existingKunde.KundeId = user.KundeId;
                    _context.Userkundes.RemoveRange(existingKunde);

                    List<Userkunde> userkunde = new List<Userkunde>();
                    foreach (var id in user.KundeId)
                    {
                        Userkunde _userKunde = new Userkunde();
                        _userKunde.UserId = user.UserId;
                        _userKunde.KundeId = Convert.ToInt32(id);
                        userkunde.Add(_userKunde);

                    }
                    await _context.Userkundes.AddRangeAsync(userkunde);
                    //_context.Userkundes.Update(existingKunde);
                }

                await _context.SaveChangesAsync();

            }
            else
            {
                user.CreatedDate = DateTime.Now;
                user.IsActive = true;

                await _dbSet.AddAsync(user);
                await _context.SaveChangesAsync();

                //2️⃣ Add to UserRoles table
                var userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = Convert.ToInt32(user.RoleId)
                };
                await _context.UserRoles.AddAsync(userRole);

                ////2️⃣ Add to userKunde table
                //var userKunde = new Userkunde
                //{
                //    UserId = user.UserId,
                //    KundeId = Convert.ToInt32(user.KundeId)
                //};
                //await _context.Userkundes.AddAsync(userKunde);
                List<Userkunde> userkunde = new List<Userkunde>();
                foreach (var id in user.KundeId)
                {
                    Userkunde _userKunde = new Userkunde();
                    _userKunde.UserId = user.UserId;
                    _userKunde.KundeId = Convert.ToInt32(id);
                    userkunde.Add(_userKunde);

                }
                await _context.Userkundes.AddRangeAsync(userkunde);
                await _context.SaveChangesAsync();

            }
            return user;
        }
        #region
        //public override async Task<User> AddAsync(User _user)
        //{
        //    //try
        //    //{
        //    //    // 1️⃣ Save User
        //    //    await _dbSet.AddAsync(_user);
        //    //    await _context.SaveChangesAsync();

        //    //    // 2️⃣ Add to UserRoles table
        //    //    var userRole = new UserRole
        //    //    {
        //    //        UserId = _user.UserId,
        //    //        RoleId = Convert.ToInt32(_user.RoleId)
        //    //    };
        //    //    await _context.UserRoles.AddAsync(userRole);
        //    //    await _context.SaveChangesAsync();

        //    //    return _user;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // Log and rethrow or handle
        //    //    //_logger.LogError(ex, "Error saving user and user role");
        //    //    throw;
        //    //}
        //    var strategy = _context.Database.CreateExecutionStrategy();

        //    return await strategy.ExecuteAsync(async () =>
        //    {
        //        // Use a transaction inside the strategy
        //        await using var transaction = await _context.Database.BeginTransactionAsync();
        //        try
        //        {
        //            // 1️⃣ Save User
        //            await _context.Users.AddAsync(_user);
        //            await _context.SaveChangesAsync();

        //            // 2️⃣ Save UserRole
        //            var userRole = new UserRole
        //            {
        //                UserId = _user.UserId,
        //                RoleId = Convert.ToInt32(_user.RoleId)
        //            };
        //            await _context.UserRoles.AddAsync(userRole);
        //            await _context.SaveChangesAsync();

        //            // Commit transaction
        //            await transaction.CommitAsync();

        //            return _user;
        //        }
        //        catch
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    });
        //}
        #endregion

    }
}
