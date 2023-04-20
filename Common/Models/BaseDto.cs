using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
	//基础Dto，通用，继承
    public class BaseDto
    {

		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private DateTime createTime;

		public DateTime CreateTime
		{
			get { return createTime; }
			set { createTime = value; }
		}

		private DateTime updateTime;

		public DateTime UpdateTIME
		{
			get { return updateTime; }
			set { updateTime = value; }
		}
	}
}
