using System;
using System.Text.Json;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Core.Handler;
using TwitchLib.EventSub.Websockets.Core.Models;

namespace TwitchLib.EventSub.Websockets.Handler.Channel.Moderators
{
    /// <summary>
    /// Handler for 'channel.moderate' notifications
    /// </summary>
    public class ChannelModerate : INotificationHandler
    {
        /// <inheritdoc />
        public string SubscriptionType => "channel.moderate";

        /// <inheritdoc />
        public void Handle(EventSubWebsocketClient client, string jsonString, JsonSerializerOptions serializerOptions)
        {
            try
            {
                var data = JsonSerializer.Deserialize<EventSubNotification<TwitchLib.EventSub.Core.SubscriptionTypes.Channel.ChannelModerate>>(jsonString.AsSpan(), serializerOptions);

                if (data is null)
                    throw new InvalidOperationException("Parsed JSON cannot be null!");

                client.RaiseEvent("ChannelModerate", new ChannelModerateArgs { Notification = data });
            }
            catch (Exception ex)
            {
                client.RaiseEvent("ErrorOccurred", new ErrorOccuredArgs { Exception = ex, Message = $"Error encountered while trying to handle {SubscriptionType} notification! Raw Json: {jsonString}" });
            }
        }
    }
}

namespace TwitchLib.EventSub.Core.SubscriptionTypes.Channel
{
    public sealed class ChannelModerate
    {
        /// <summary>
        /// The user ID of the broadcaster in whose chat the moderation
        /// action was performed.
        /// </summary>
        public string BroadcasterUserId { get; set; } = string.Empty;

        /// <summary>
        /// The user login of the broadcaster in whose chat the moderation
        /// action was performed.
        /// </summary>
        public string BroadcasterUserLogin { get; set; } = string.Empty;

        /// <summary>
        /// The display name of the broadcaster in whose chat the
        /// moderation action was performed.
        /// </summary>
        public string BroadcasterUserName { get; set; } = string.Empty;

        /// <summary>
        /// The user ID of the moderator who performed the action.
        /// </summary>
        public string ModeratorUserId { get; set; } = string.Empty;

        /// <summary>
        /// The user login of the moderator who performed the action.
        /// </summary>
        public string ModeratorUserLogin { get; set; } = string.Empty;

        /// <summary>
        /// The display name of the moderator who performed the action.
        /// </summary>
        public string ModeratorUserName { get; set; } = string.Empty;

        /// <summary>
        /// The action performed. Possible values are: ban, timeout,
        /// unban, untimeout, clear, emoteonly, emoteonlyoff, followers,
        /// followersoff, uniquechat, uniquechatoff, slow, slowoff,
        /// subscribers, subscribersoff, unraid, delete, unvip, vip, raid,
        /// add_blocked_term, add_permitted_term, remove_blocked_term,
        /// remove_permitted_term, mod, unmod, approve_unban_request,
        /// deny_unban_request, warn
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Metadata associated with the followers command.
        /// </summary>
        public FollowersModeInfo Followers { get; set; }

        /// <summary>
        /// Metadata associated with the slow command.
        /// </summary>
        public SlowModeInfo Slow { get; set; }

        /// <summary>
        /// Metadata associated with the vip command.
        /// </summary>
        public UserTargetInfo Vip { get; set; }

        /// <summary>
        /// Metadata associated with the unvip command.
        /// </summary>
        public UserTargetInfo Unvip { get; set; }

        /// <summary>
        /// Metadata associated with the mod command.
        /// </summary>
        public UserTargetInfo Mod { get; set; }

        /// <summary>
        /// Metadata associated with the unmod command.
        /// </summary>
        public UserTargetInfo Unmod { get; set; }

        /// <summary>
        /// Metadata associated with the ban command.
        /// </summary>
        public BanInfo Ban { get; set; }

        /// <summary>
        /// Metadata associated with the unban command.
        /// </summary>
        public UserTargetInfo Unban { get; set; }

        /// <summary>
        /// Metadata associated with the timeout command.
        /// </summary>
        public TimeoutInfo Timeout { get; set; }

        /// <summary>
        /// Metadata associated with the untimeout command.
        /// </summary>
        public UserTargetInfo Untimeout { get; set; }

        /// <summary>
        /// Metadata associated with the raid command.
        /// </summary>
        public RaidInfo Raid { get; set; }

        /// <summary>
        /// Metadata associated with the unraid command.
        /// </summary>
        public UserTargetInfo Unraid { get; set; }

        /// <summary>
        /// Metadata associated with the delete command.
        /// </summary>
        public MessageDeleteInfo Delete { get; set; }

        /// <summary>
        /// Metadata associated with the automod terms changes.
        /// </summary>
        public AutomodTermsInfo AutomodTerms { get; set; }

        /// <summary>
        /// Metadata associated with an unban request.
        /// </summary>
        public UnbanRequestInfo UnbanRequest { get; set; }

        /// <summary>
        /// Metadata associated with the warn command.
        /// </summary>
        public ChatWarningInfo Warn { get; set; }
    }

    public class FollowersModeInfo
    {
        /// <summary>
        /// The length of time, in minutes, that the followers must have
        /// followed the broadcaster to participate in the chat room.
        /// </summary>
        public int FollowDurationMinutes { get; set; }
    }

    public class SlowModeInfo
    {
        /// <summary>
        /// The amount of time, in seconds, that users need to wait
        /// between sending messages.
        /// </summary>
        public int WaitTimeSeconds { get; set; }
    }

    public class UserTargetInfo
    {
        /// <summary>
        /// The ID of the user targeted by this command.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The login of the user targeted by this command.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// The display name of the user targeted by this command.
        /// </summary>
        public string UserName { get; set; }
    }

    public class BanInfo : UserTargetInfo
    {
        /// <summary>
        /// The stated reason for the ban/timeout.
        /// </summary>
        public string Reason { get; set; }
    }

    public class TimeoutInfo : BanInfo
    {
        /// <summary>
        /// The time at which the timeout expires.
        /// </summary>
        public string ExpiresAt { get; set; }
    }

    public class RaidInfo : UserTargetInfo
    {
        /// <summary>
        /// The viewer count.
        /// </summary>
        public int ViewerCount { get; set; }
    }

    public class MessageDeleteInfo : UserTargetInfo
    {
        /// <summary>
        /// The ID of the message being deleted.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// The content of the message being deleted.
        /// </summary>
        public string MessageBody { get; set; }
    }

    public class AutomodTermsInfo
    {
        /// <summary>
        /// Either "add" or "remove".
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Either "blocked" or "permitted".
        /// </summary>
        public string List { get; set; }

        /// <summary>
        /// TTerms being added or removed.
        /// </summary>
        public string[] Terms { get; set; }

        /// <summary>
        /// Whether or not the terms were added due to an Automod message
        /// approve/deny action.
        /// </summary>
        public bool FromAutomod { get; set; }
    }

    public class UnbanRequestInfo : UserTargetInfo
    {
        /// <summary>
        /// Whether the unban request was approved (<c>true</c>) or denied
        /// (<c>false</c>).
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// The message included by the moderator explaining their
        /// approval or denial.
        /// </summary>
        public string ModeratorMessage { get; set; }
    }

    public class ChatWarningInfo : BanInfo
    {
        /// <summary>
        /// Chat rules sited for the warning.
        /// </summary>
        public string[] ChatRulesCited { get; set; }
    }
}