namespace Convai.Scripts.Runtime.Addons
{
    /// <summary>
    ///     Enumeration defining various types of in-app notifications.
    ///     Each enum value represents a specific scenario or issue that can trigger a notification.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        ///     Indicates a notification related to microphone problems.
        /// </summary>
        MicrophoneIssue,

        /// <summary>
        ///     Indicates a notification related to network reachability issues.
        /// </summary>
        NetworkReachabilityIssue,

        /// <summary>
        ///     Indicates a notification when the user is not in close proximity to initiate a conversation.
        /// </summary>
        NotCloseEnoughForConversation,

        /// <summary>
        ///     Indicates a notification when a user releases the talk button prematurely during a conversation.
        /// </summary>
        TalkButtonReleasedEarly,

        /// <summary>
        ///     Indicates that no microphone device was detected in the system
        /// </summary>
        NoMicrophoneDetected,

        /// <summary>
        ///     Indicates that no API key was found.
        /// </summary>
        APIKeyNotFound
    }
}