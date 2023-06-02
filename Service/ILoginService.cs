

using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService 
    {
        Task<ApiResponse<UserDto>> LoginAsync(UserDto userDto);

        Task<ApiResponse> RegisterAsync(UserDto userDto);
    }

}
