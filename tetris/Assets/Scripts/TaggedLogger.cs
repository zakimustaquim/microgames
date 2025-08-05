using System.Diagnostics;
using UnityEngine;

public class TaggedLogger
{
    private readonly string scriptTag;
    private readonly Object context;
    public const int DefaultLogLevel = 3;

    public TaggedLogger(string tag, Object context = null)
    {
        scriptTag = tag;
        this.context = context;
    }

    private string Tag(string tag, int level) => $"{tag}--l{level}";

    [Conditional("UNITY_EDITOR")]
    public void Debug(string message, int level = DefaultLogLevel) => UnityEngine.Debug.Log($"[{Tag(scriptTag, level)}] {message}", context);

    [Conditional("UNITY_EDITOR")]
    public void Warn(string message, int level = DefaultLogLevel) => UnityEngine.Debug.LogWarning($"[{Tag(scriptTag, level)}] {message}", context);

    [Conditional("UNITY_EDITOR")]
    public void Error(string message, int level = DefaultLogLevel) => UnityEngine.Debug.LogError($"[{Tag(scriptTag, level)}] {message}", context);

    [Conditional("UNITY_EDITOR")]
    public void Debug(string message, string tagOverride, int level = DefaultLogLevel) => UnityEngine.Debug.Log($"[{Tag(tagOverride, level)}] {message}", context);

    [Conditional("UNITY_EDITOR")]
    public void Warn(string message, string tagOverride, int level = DefaultLogLevel) => UnityEngine.Debug.LogWarning($"[{Tag(tagOverride, level)}] {message}", context);

    [Conditional("UNITY_EDITOR")]
    public void Error(string message, string tagOverride, int level = DefaultLogLevel) => UnityEngine.Debug.LogError($"[{Tag(tagOverride, level)}] {message}", context);
}
