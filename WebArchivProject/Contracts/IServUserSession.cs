using WebArchivProject.Models;

namespace WebArchivProject.Contracts
{
    public interface IServUserSession
    {
        SessionUser User { get; }

        void RemoveUserSession();
        void UpdateUserSession(SessionUser user);
    }
}
