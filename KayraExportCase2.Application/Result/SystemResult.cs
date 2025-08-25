using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Result
{
    public class SystemResult<T>
    {
        public T? Data { get; set; }
        public List<StateMessage> Messages { get; set; }

        public EPriority Status
        {
            get
            {
                int max = 0;
                foreach (var message in Messages)
                {
                    if ((int)message.Priority > max)
                    {
                        max = (int)message.Priority;
                    }
                }
                return (EPriority)max;
            }
        }
        public bool IsSuccess
        {
            get
            {
                return Status == EPriority.None || Status == EPriority.Information || Status == EPriority.Warning;
            }
        }

        public SystemResult()
        {
            Messages = new List<StateMessage>();
        }
        public SystemResult(string message)
        {
            Messages = new List<StateMessage>()
            {
                new StateMessage
                {
                    Message = message,
                    Priority = EPriority.Error
                }
            };
        }

        public void AddMessage<T2>(SystemResult<T2> result)
        {
            Messages.AddRange(result.Messages);
        }
        public void AddMessage(string message, EPriority priority = EPriority.Error)
        {
            Messages.Add(new StateMessage
            {
                Message = message,
                Priority = priority
            });
        }
        public void AddDefaultErrorMessage(Exception? ex, string message = "")
        {
            Messages.Add(new StateMessage
            {
                Message = "An Error has occuraced; " + message,
                Priority = EPriority.SystemError
            });
        }

        public List<string> GetMessages()
        {
            return Messages?.Select(x => x.Message ?? "")?.ToList() ?? new List<string>();
        }

    }
    public class StateMessage
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        public EPriority Priority { get; set; }
    }

    public enum EPriority
    {
        None = 0,
        Information,
        Warning,
        Error,
        SystemError
    }
}
