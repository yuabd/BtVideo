using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtVideo.Models.Admin
{
	public class BaseObject
	{
		public int Tag { get; set; }

		public string Message { get; set; }

		public BaseObject()
		{
		}

		public BaseObject(int tag)
		{
			Tag = tag;
		}

		public BaseObject(int tag, string message)
		{
			Tag = tag;
			Message = message;
		}
	}
	public class BaseObject<T>
	{
		public int Tag { get; set; }

		public string Message { get; set; }

		public T Result { get; set; }

		public BaseObject()
		{
		}

		public BaseObject(int tag, string message, T result)
		{
			Tag = tag;
			Message = message;
			Result = result;
		}
	}
}