﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using Typesafe.Mailgun.Extensions.Json;
using Typesafe.Mailgun.Mailboxes;
using Typesafe.Mailgun.Routing;
using Typesafe.Mailgun.Statistics;

namespace Typesafe.Mailgun
{
	/// <summary>
	/// Provides access to the Mailgun REST API.
	/// </summary>
	public class MailgunClient : IMailgunAccountInfo
	{
		/// <summary>
		/// Initializes a new client for the specified domain and api key.
		/// </summary>
		public MailgunClient(string domain, string apiKey)
		{
			DomainBaseUrl = new Uri("https://api.mailgun.net/v2/" + domain + "/");
			ApiKey = apiKey;
		}

		public Uri DomainBaseUrl { get; private set; }

		public string ApiKey { get; private set; }


		public CommandResult SendMail(MailMessage mailMessage)
		{
			return new SendMailCommand(this, mailMessage).Invoke();
		}


		public IEnumerable<Route> GetRoutes(int skip, int take, out int count)
		{
			return new MailgunRouteQuery(this).Execute(skip, take, out count);
		}

		public Route CreateRoute(int priority, string description, RouteFilter expression, params RouteAction[] actions)
		{
			return new CreateRouteCommand(this, priority, description, expression, actions).Invoke().Route;
		}

		public CommandResult DeleteRoute(string routeId)
		{
			return new DeleteCommand(this, "../routes/" + routeId).Invoke();
		}


		public IEnumerable<MailgunStatEntry> GetStats(int skip, int take, MailgunEventTypes eventTypes, out int count)
		{
			return new MailgunStatsQuery(this, eventTypes).Execute(skip, take, out count);
		}


		public CommandResult CreateMailbox(string name, string password)
		{
			return new CreateMailboxCommand(this, name, password).Invoke();
		}

		public CommandResult DeleteMailbox(string name)
		{
			return new DeleteCommand(this, "mailboxes/" + name).Invoke();
		}

		public IEnumerable<Mailbox> GetMailboxes(int skip, int take, out int count)
		{
			return new MailgunMailboxQuery(this).Execute(skip, take, out count);
		}
	}
}
