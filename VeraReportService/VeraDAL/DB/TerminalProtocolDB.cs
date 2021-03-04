using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class TerminalProtocolDB
	{
		private static TerminalProtocolDB instance;
		public static TerminalProtocolDB GetInstance()
		{
			if (instance == null)
			{
				instance = new TerminalProtocolDB();
			}
			return instance;
		}
		public TerminalProtocol AddNewTerminalProtocol(TerminalProtocol _TerminalProtocol)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.TerminalProtocol.Add(_TerminalProtocol);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _TerminalProtocol : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<TerminalProtocol> GetAllTerminalProtocols()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalProtocolList = context.TerminalProtocol.ToList();
					return TerminalProtocolList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public TerminalProtocol GetTerminalProtocolById(int _TerminalProtocolId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalProtocol = context.TerminalProtocol.FirstOrDefault(a => a.Id == _TerminalProtocolId && a.Status == 1);
					return TerminalProtocol != null ? TerminalProtocol : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteTerminalProtocol(int _TerminalProtocolId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalProtocol = context.TerminalProtocol.FirstOrDefault(a => a.Id == _TerminalProtocolId);
					if (TerminalProtocol != null)
					{
						TerminalProtocol.Status = 0;
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
		private void UpdateObject(TerminalProtocol _newTerminalProtocol, ref TerminalProtocol _oldTerminalProtocol)
		{
			try
			{

				foreach (PropertyInfo TerminalProtocolPropInfo in _newTerminalProtocol.GetType().GetProperties().ToList())
				{
					_oldTerminalProtocol.GetType().GetProperty(TerminalProtocolPropInfo.Name).SetValue(_oldTerminalProtocol, _newTerminalProtocol.GetType().GetProperty(TerminalProtocolPropInfo.Name).GetValue(_newTerminalProtocol));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public TerminalProtocol UpdateTerminalProtocol(TerminalProtocol _TerminalProtocol)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldTerminalProtocol = context.TerminalProtocol.FirstOrDefault(u => u.Id == _TerminalProtocol.Id);
					if (oldTerminalProtocol != null)
					{
						UpdateObject(_TerminalProtocol, ref oldTerminalProtocol);
						var numberOfUpdatedTerminalProtocol = context.SaveChanges();
						return numberOfUpdatedTerminalProtocol > 0 ? _TerminalProtocol : null;
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

		public TerminalProtocol GetTerminalProtocolByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalProtocol = context.TerminalProtocol.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (TerminalProtocol == null)
					{
						TerminalProtocol = new TerminalProtocol()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.TerminalProtocol.Add(TerminalProtocol);
						context.SaveChanges();
						return TerminalProtocol;
					}

					return TerminalProtocol;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
