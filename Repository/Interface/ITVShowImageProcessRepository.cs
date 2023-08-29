namespace OTTMyPlatform.Repository.Interface
{
    public interface ITVShowImageProcessRepository
    {
        Task RemoveTVShowImage(int showId);
        bool UploadeFiles(IFormFileCollection uploadeFile);
        string GetTVShowImage(string tvShowImage);
    }
}
