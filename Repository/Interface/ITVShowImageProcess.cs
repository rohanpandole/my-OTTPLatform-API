namespace OTTMyPlatform.Repository.Interface
{
    public interface ITVShowImageProcess
    {
        Task RemoveTVShowImage(int showId);
        bool UploadeFiles(IFormFileCollection uploadeFile);
        string GetTVShowImage(string tvShowImage);
    }
}
