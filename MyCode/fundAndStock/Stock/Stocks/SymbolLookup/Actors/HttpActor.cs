using System;

using Akka.Actor;

using SymbolLookup.Actors.Messages;


using Common.Logging;
using Spring.Rest.Utils;

namespace SymbolLookup.Actors
{
    public class HttpActor : TypedActor, IHandle<HttpGetCommand>, IHandle<HttpPostCommand>
    {
        protected static ILog log = LogManager.GetLogger(typeof(HttpActor));
        public void Handle(HttpPostCommand message)
        {
            throw new NotImplementedException();
        }

        public void Handle(HttpGetCommand message)
        {
            HttpGetCommand(message);
        }

        protected void HttpGetCommand(HttpGetCommand command)
        {

            if (!string.IsNullOrWhiteSpace(command.url))
            {
                var template = RestTemplateUtils.BuildRestTemplate(command.url, command.encoding);

                template.GetForMessageAsync<string>(command.url, r =>
                {
                    if (r.Error == null)
                    {
                        if (!string.IsNullOrEmpty(r.Response.Body))
                        {
                            Context.Sender.Tell(new HttpCommandResponseData()
                            {
                                Command = command,
                                Body = r.Response.Body
                            });
                        }
                    }
                    else
                    {
                        log.ErrorFormat("{0}", r.Error, string.Empty);
                    }
                });

            }
        }
    }

}
