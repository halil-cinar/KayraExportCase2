using KayraExportCase2.Application.Result;
using KayraExportCase2.Domain.Dtos;
using KayraExportCase2.Domain.Entities;
using KayraExportCase2.Domain.ListDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Abstract
{
    public interface IAuthService
    {
        public Task<SystemResult<UserListDto>> SignUp(UserDto dto);
        public Task<SystemResult<SessionListDto>> Login(IdentityDto ıdentity);
    }
}
