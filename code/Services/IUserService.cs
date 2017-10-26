namespace UserAuditReport.Services
{
    interface IUserService
    {
        bool IsUserInRole(string username);
    }
}