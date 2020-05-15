namespace YTViewer.Infrastructure.Interfaces
{
    public interface IAddonRepository
    {
        void VerifyIpAddress();
        void Connect();
        void Refresh();
    }
}
