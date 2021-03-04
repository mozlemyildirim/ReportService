using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class MessagingDB
	{
		private static MessagingDB instance;
		public static MessagingDB GetInstance()
		{
			if (instance == null)
			{
				instance = new MessagingDB();
			}
			return instance;
		}
		public Messaging AddNewMessaging(Messaging _Messaging)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Messaging.Add(_Messaging);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _Messaging : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<Messaging> GetAllMessagings()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var MessagingList = context.Messaging.Where(a => a.Status == 1).ToList();
					return MessagingList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Messaging GetMessagingById(int _MessagingId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Messaging = context.Messaging.FirstOrDefault(a => a.Id == _MessagingId && a.Status == 1);
					return Messaging != null ? Messaging : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteMessaging(int _MessagingId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Messaging = context.Messaging.FirstOrDefault(a => a.Id == _MessagingId);
					if (Messaging != null)
					{
						Messaging.Status = 0;
						int numOfDeleted = context.SaveChanges();
						return numOfDeleted > 0;
					}
				}
				return false;
			}
			catch (Exception)
			{

				throw;
			}

		}
		private void UpdateObject(Messaging _newMessaging, ref Messaging _oldMessaging)
		{
			try
			{

				foreach (PropertyInfo MessagingPropInfo in _newMessaging.GetType().GetProperties().ToList())
				{
					_oldMessaging.GetType().GetProperty(MessagingPropInfo.Name).SetValue(_oldMessaging, _newMessaging.GetType().GetProperty(MessagingPropInfo.Name).GetValue(_newMessaging));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Messaging UpdateMessaging(Messaging _Messaging)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldMessaging = context.Messaging.FirstOrDefault(u => u.Id == _Messaging.Id);
					if (oldMessaging != null)
					{
						UpdateObject(_Messaging, ref oldMessaging);
						var numberOfUpdatedMessaging = context.SaveChanges();
						return numberOfUpdatedMessaging > 0 ? _Messaging : null;
					}

					return null;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Messaging GetMessagingByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Messaging = context.Messaging.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (Messaging == null)
					{
						Messaging = new Messaging()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.Messaging.Add(Messaging);
						context.SaveChanges();
						return Messaging;
					}

					return Messaging;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
