using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using perla_metro_user.src.Data;

namespace perla_metro_user.src.Helpers
{
    public class GuidHelper
    {
        public static async Task<Guid> GenerateUniqueUserIdAsync(ApplicationDBContext context)
        {
            Guid newId;
            bool exists;

            do
            {
                newId = Guid.NewGuid();
                exists = await context.Users.AnyAsync(u => u.Id == newId);
            } 
            while (exists);

            return newId;
        }
    }
}