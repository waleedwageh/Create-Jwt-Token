using jwt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt.Servies
{
   public interface IAuthservies
    {
        Task<AuthModel> RegisterAsync(RegisterModel registerModel);
    }
}
