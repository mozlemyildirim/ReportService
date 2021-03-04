using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class TerminalTypeDB
	{
		private static TerminalTypeDB instance;
		public static TerminalTypeDB GetInstance()
		{
			if (instance == null)
			{
				instance = new TerminalTypeDB();
			}
			return instance;
		}
		public TerminalType AddNewTerminalType(TerminalType _TerminalType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.TerminalType.Add(_TerminalType);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _TerminalType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<TerminalType> GetAllTerminalTypes()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalTypeList = context.TerminalType.ToList();
					return TerminalTypeList;
				}
			}
			catch (Exception exc)
			{

				throw exc;
			}
		}
		public TerminalType GetTerminalTypeById(int _TerminalTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalType = context.TerminalType.FirstOrDefault(a => a.Id == _TerminalTypeId && a.Status == 1);
					return TerminalType != null ? TerminalType : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteTerminalType(int _TerminalTypeId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var TerminalType = context.TerminalType.FirstOrDefault(a => a.Id == _TerminalTypeId);
					if (TerminalType != null)
					{
						TerminalType.Status = 0;
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
		private void UpdateObject(TerminalType _newTerminalType, ref TerminalType _oldTerminalType)
		{
			try
			{

				foreach (PropertyInfo TerminalTypePropInfo in _newTerminalType.GetType().GetProperties().ToList())
				{
					_oldTerminalType.GetType().GetProperty(TerminalTypePropInfo.Name).SetValue(_oldTerminalType, _newTerminalType.GetType().GetProperty(TerminalTypePropInfo.Name).GetValue(_newTerminalType));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public TerminalType UpdateTerminalType(TerminalType _TerminalType)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldTerminalType = context.TerminalType.FirstOrDefault(u => u.Id == _TerminalType.Id);
					if (oldTerminalType != null)
					{
						UpdateObject(_TerminalType, ref oldTerminalType);
						var numberOfUpdatedTerminalType = context.SaveChanges();
						return numberOfUpdatedTerminalType > 0 ? _TerminalType : null;
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

		public TerminalType GetTerminalTypeByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var terminalType = context.TerminalType.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (terminalType == null)
					{
						terminalType = new TerminalType()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.TerminalType.Add(terminalType);
						context.SaveChanges();
						return terminalType;
					}

					return terminalType;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}

	}
}
