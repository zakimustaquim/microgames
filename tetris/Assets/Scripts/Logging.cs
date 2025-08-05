public class Logging
{
    protected TaggedLogger logger;
    protected void log(string message, int level = TaggedLogger.DefaultLogLevel) => logger.Debug(message, level);
    protected void warn(string message, int level = TaggedLogger.DefaultLogLevel) => logger.Warn(message, level);
    protected void err(string message, int level = TaggedLogger.DefaultLogLevel) => logger.Error(message, level);

    protected void log(string message, string tagOverride, int level = TaggedLogger.DefaultLogLevel) => logger.Debug(message, tagOverride, level);
    protected void warn(string message, string tagOverride, int level = TaggedLogger.DefaultLogLevel) => logger.Warn(message, tagOverride, level);
    protected void err(string message, string tagOverride, int level = TaggedLogger.DefaultLogLevel) => logger.Error(message, tagOverride, level);

    public Logging()
    {
        logger = new TaggedLogger(GetType().Name);
    }

    protected void SetTag(string newTag)
    {
        logger = new TaggedLogger(newTag);
    }
}
