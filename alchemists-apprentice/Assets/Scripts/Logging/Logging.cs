/// <summary>
/// An implementation of the core logging methods using
/// a [TaggedLogger] for non-MonoBehaviour based classes.
/// 
/// Implementing classes must instantiate the [TaggedLogger]
/// object before using any of the methods.
/// </summary>
public class Logging
{
    public Logging()
    {
        logger = new TaggedLogger(GetType().Name);
    }

    protected TaggedLogger logger;
    protected void log(string message, int level = TaggedLogger.DefaultLogLevel) => logger.Debug(message, level);
    protected void warn(string message, int level = TaggedLogger.DefaultLogLevel) => logger.Warn(message, level);
    protected void err(string message, int level = TaggedLogger.DefaultLogLevel) => logger.Error(message, level);

    protected void log(string message, string tagOverride, int level = TaggedLogger.DefaultLogLevel) => logger.Debug(message, tagOverride, level);
    protected void warn(string message, string tagOverride, int level = TaggedLogger.DefaultLogLevel) => logger.Warn(message, tagOverride, level);
    protected void err(string message, string tagOverride, int level = TaggedLogger.DefaultLogLevel) => logger.Error(message, tagOverride, level);

    protected void SetTag(string newTag)
    {
        logger = new TaggedLogger(newTag);
    }
}
