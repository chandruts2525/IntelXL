using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class Chat
{
    [JsonPropertyName("chatId")]
    public int ChatId { get; set; }
    [JsonPropertyName("fromId")]
    public int FromId { get; set; }
    [JsonPropertyName("toId")]
    public int ToId { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }
    [JsonPropertyName("mediaUrl")]
    public string? MediaUrl { get; set; }
    [JsonPropertyName("isDelivered")]
    public bool IsDelivered { get; set; }=false;
    [JsonPropertyName("sentAt")]
    public DateTime SentAt { get; set; }
    [JsonPropertyName("isRead")]
    public bool IsRead { get; set; } = false;
    [JsonPropertyName("isArchived")]
    public bool IsArchived { get; set; } = false;
    [JsonPropertyName("conversationId")]
    public string? ConversationId { get; set; }
    [JsonPropertyName("fromUser")]
    public virtual AppUser? FromUser { get; set; }
    [JsonPropertyName("toUser")]
    public virtual AppUser? ToUser { get; set; }
}
