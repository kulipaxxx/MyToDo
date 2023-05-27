using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService 
    {
        Task<ApiResponse> LoginAsync(UserDto userDto);

        Task<ApiResponse> RegisterAsync(UserDto userDto);
    }

}
