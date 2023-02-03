using Event.Bll.Models;

namespace Event.Bll.Services.Interfaces;

public interface IUserService
{
    User GetCurrentUser();
}