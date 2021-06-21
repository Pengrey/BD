using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_form
{

	[Serializable()]
	public class Anime
	{

		private String _Name;
		private String _Description;
		private decimal  _Avaliation;
		private String _ReleaseDate;
		private String _Progresso;
		private String _Estado;
		private String _Estudio;
		private String _Autor;
		public String Name
		{
			get { return _Name; }
			set 
			{
				if (value == null | String.IsNullOrEmpty(value))
				{
					throw new Exception("Anime Name field can’t be empty");
					return;
				}
				_Name = value; 
			}
		}

		public String Autor
		{
			get { return _Autor; }
			set { _Autor = value; }
		}

		public String Progresso
		{
			get { return _Progresso; }
			set { _Progresso = value; }
		}

		public String Estudio
		{
			get { return _Estudio; }
			set { _Estudio = value; }
		}

		public String Estado
		{
			get { return _Estado; }
			set { _Estado = value; }
		}


		public String Description
		{
			get { return _Description; }
			set {_Description = value;}
		}

		public decimal Avaliation
		{
			get { return _Avaliation; }
			set { _Avaliation = value; }
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

		public Anime() : base()
		{
		}

		public Anime(String Name) : base()
		{
			this.Name = Name;
		}
	}
}


