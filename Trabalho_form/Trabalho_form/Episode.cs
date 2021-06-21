using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_form
{
    class Episode
    {
		private String _Name;
		private String _SeasonName;
		private String _AnimeName;
		private String _Description;
		private decimal _Avaliation;
		private String _Estudio;
		private String _ReleaseDate;

		public String Name
		{
			get { return _Name; }
			set
			{
				if (value == null | String.IsNullOrEmpty(value))
				{
					throw new Exception("Episode Name field can’t be empty");
					return;
				}
				_Name = value;
			}
		}

		public String SeasonName
		{
			get { return _SeasonName; }
			set { _SeasonName = value; }
		}

		public String AnimeName
		{
			get { return _AnimeName; }
			set { _AnimeName = value; }
		}

		public String Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		public decimal Avaliation
		{
			get { return _Avaliation; }
			set { _Avaliation = value; }
		}

		public String Estudio
		{
			get { return _Estudio; }
			set { _Estudio = value; }
		}

		public String ReleaseDate
		{
			get { return _ReleaseDate; }
			set { _ReleaseDate = value; }
		}

		public override String ToString()
		{
			return _Name;
		}
	}
}
