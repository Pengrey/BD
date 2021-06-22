using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_form
{
	class Author
	{
		private string _ID;
		private string _Nome;


		public String ID
		{
			get { return _ID; }
			set
			{
				if (value == null | String.IsNullOrEmpty(value))
				{
					throw new Exception("ID field can’t be empty");
					return;
				}
				_ID = value;
			}
		}

		public String Nome
		{
			get { return _Nome; }
			set { _Nome = value; }
		}

		public override String ToString()
		{
			return _Nome;
		}
	}
}
